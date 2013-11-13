using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;

namespace MBS.Auth0.Models
{
    [OrchardFeature("MBS.Auth0")]
    public class Auth0SettingsPart : ContentPart<Auth0SettingsPartRecord>
    {
        [Required(ErrorMessage = "ClientId is required")]
        public string ClientId { get { return Record.ClientId; } set { Record.ClientId = value; } }
        [Required(ErrorMessage = "Domain is required")]
        public string Domain { get { return Record.Domain; } set { Record.Domain = value; } }
        public string ClientSecret { get; set; }
    }
}
