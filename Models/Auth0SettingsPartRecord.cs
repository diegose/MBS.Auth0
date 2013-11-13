using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;

namespace MBS.Auth0.Models
{
    [OrchardFeature("MBS.Auth0")]
    public class Auth0SettingsPartRecord : ContentPartRecord
    {
        public virtual string ClientId { get; set; }
        public virtual string EncryptedClientSecret { get; set; }
        public virtual string Domain { get; set; }
    }
}
