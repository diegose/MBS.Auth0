using System.Data;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace MBS.Auth0
{
    [OrchardFeature("MBS.Auth0")]
    public class Auth0Migrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterPartDefinition(
                "Auth0WidgetPart",
                builder => builder.Attachable());

            ContentDefinitionManager.AlterTypeDefinition(
                "Auth0Widget",
                cfg => cfg
                    .WithPart("Auth0WidgetPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            SchemaBuilder.CreateTable(
                "Auth0SettingsPartRecord",
                table => table.ContentPartRecord()
                              .Column("Domain", DbType.String, command => command.WithLength(255))
                              .Column("ClientId", DbType.String, command => command.WithLength(255))
                              .Column("EncryptedClientSecret", DbType.String, command => command.WithLength(512)));
            return 1;
        }
    }
}
