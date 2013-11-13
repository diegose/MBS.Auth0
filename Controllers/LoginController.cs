using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Auth0;
using MBS.Auth0.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.UI.Notify;
using Orchard.Users.Models;
using RestSharp.Extensions;

namespace MBS.Auth0.Controllers
{
    [OrchardFeature("MBS.Auth0")]
    public class LoginController : Controller
    {
        readonly IOrchardServices _services;
        readonly IAuthenticationService _authenticationService;
        readonly IMembershipService _membershipService;
        readonly IOrchardServices _orchardServices;
        readonly IEncryptionService _encryptionService;

        public Localizer T { get; set; }

        public LoginController(IOrchardServices services,
            IAuthenticationService authenticationService,
            IMembershipService membershipService,
            IOrchardServices orchardServices,
            IEncryptionService encryptionService) {
            T = NullLocalizer.Instance;
            _services = services;
            _authenticationService = authenticationService;
            _membershipService = membershipService;
            _orchardServices = orchardServices;
            _encryptionService = encryptionService;
        }

        public ActionResult Callback(string code, string state) {
            if (string.IsNullOrEmpty(code))
                return Error("Invalid code");
            var part = _services.WorkContext.CurrentSite.As<Auth0SettingsPart>();
            var clientSecret = Encoding.UTF8.GetString(_encryptionService.Decode(Convert.FromBase64String(part.Record.EncryptedClientSecret)));
            var client = new Client(part.ClientId, clientSecret, part.Domain);
            TokenResult token;
            try {
                token = client.ExchangeAuthorizationCodePerAccessToken(code, state);
            }
            catch (OAuthException exception)
            {
                return Error(exception.Description);
            }
            var profile = client.GetUserInfo(token);
            if (string.IsNullOrEmpty(profile.Email))
                return Error("Invalid email");
            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser != null) 
                _authenticationService.SignOut();
            var email = profile.Email.ToLower();
            var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == email).List().FirstOrDefault();
            if (user == null)
            {
                user = _membershipService.CreateUser(new CreateUserParams(email, Guid.NewGuid().ToString(), email, null, null, true)) as UserPart;
                if (user == null) {
                    return Error("Could not create user");
                }
            }
            if (user.RegistrationStatus != UserStatus.Approved) {
                return Error("Approval needed");
            }
            _authenticationService.SignIn(user, true);
            return this.RedirectLocal(state);
        }

        ActionResult Error(string text) {
            _services.Notifier.Add(NotifyType.Error, T(text));
            return Redirect("~/");
        }
    }
}