using System;
using System.Text;
using MBS.Auth0.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Security;

namespace MBS.Auth0.Drivers
{
    [OrchardFeature("MBS.Auth0")]
    public class Auth0SettingsPartDriver : ContentPartDriver<Auth0SettingsPart>
    {
        private readonly IEncryptionService _service;

        public Localizer T { get; set; }

        public Auth0SettingsPartDriver(IEncryptionService service)
        {
            _service = service;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix { get { return "FacebookSettings"; } }

        protected override DriverResult Editor(Auth0SettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Auth0Settings",
                               () => shapeHelper.EditorTemplate(TemplateName: "Parts.Auth0Settings", Model: part, Prefix: Prefix)).OnGroup("Auth0");
        }

        protected override DriverResult Editor(Auth0SettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if(updater.TryUpdateModel(part, Prefix, null, null))
            {
                if(!string.IsNullOrWhiteSpace(part.ClientSecret))
                {
                    part.Record.EncryptedClientSecret = Convert.ToBase64String(_service.Encode(Encoding.UTF8.GetBytes(part.ClientSecret)));
                }
            }
            return Editor(part, shapeHelper);
        }
    }
}
