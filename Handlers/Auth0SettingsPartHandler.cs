using MBS.Auth0.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace MBS.Auth0.Handlers
{
    [OrchardFeature("MBS.Auth0")]
    public class Auth0SettingsPartHandler : ContentHandler
    {
        public Localizer T { get; set; }

        public Auth0SettingsPartHandler(IRepository<Auth0SettingsPartRecord> repository)
        {
            Filters.Add(new ActivatingFilter<Auth0SettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));
            T = NullLocalizer.Instance;
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Auth0")) { Id = "Auth0" });
        }
    }
}
