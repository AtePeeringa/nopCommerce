using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class ConfigurationGroupEntityBuilder : NopEntityBuilder<ConfigurationGroup>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ConfigurationGroup.ProductConfigurationId)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationGroup.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(ConfigurationGroup.DisplayName)).AsString(400).NotNullable()
            .WithColumn(nameof(ConfigurationGroup.Description)).AsString(4000).Nullable()
            .WithColumn(nameof(ConfigurationGroup.IsRequired)).AsBoolean().NotNullable()
            .WithColumn(nameof(ConfigurationGroup.DisplayOrder)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationGroup.GroupType)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationGroup.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(ConfigurationGroup.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}