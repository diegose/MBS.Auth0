using MBS.Auth0.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace MBS.Auth0.Drivers {
    public class Auth0Widget : ContentPartDriver<Auth0WidgetPart> {
        readonly IOrchardServices _services;
        public Auth0Widget(IOrchardServices services) {
            _services = services;
        }

        protected override DriverResult Display(Auth0WidgetPart part, string displayType, dynamic shapeHelper) {
            var settings = _services.WorkContext.CurrentSite.As<Auth0SettingsPart>();
            return ContentShape("Parts_Auth0Widget", () => shapeHelper.Parts_Auth0Widget(Settings: settings));
        }
    }
}