using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class ConfigurationOptionEntityBuilder : NopEntityBuilder<ConfigurationOption>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ConfigurationOption.ConfigurationGroupId)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationOption.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(ConfigurationOption.DisplayName)).AsString(400).NotNullable()
            .WithColumn(nameof(ConfigurationOption.Description)).AsString(4000).Nullable()
            .WithColumn(nameof(ConfigurationOption.PriceAdjustment)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(ConfigurationOption.PriceAdjustmentType)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationOption.IsDefault)).AsBoolean().NotNullable()
            .WithColumn(nameof(ConfigurationOption.DisplayOrder)).AsInt32().NotNullable()
            .WithColumn(nameof(ConfigurationOption.ImageUrl)).AsString(1000).Nullable()
            .WithColumn(nameof(ConfigurationOption.AdditionalData)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(ConfigurationOption.DependsOnOptions)).AsString(4000).Nullable()
            .WithColumn(nameof(ConfigurationOption.ConflictingOptions)).AsString(4000).Nullable()
            .WithColumn(nameof(ConfigurationOption.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(ConfigurationOption.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(ConfigurationOption.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}