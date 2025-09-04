using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class ProductConfigurationEntityBuilder : NopEntityBuilder<ProductConfiguration>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ProductConfiguration.ProductId)).AsInt32().NotNullable().ForeignKey<Nop.Core.Domain.Catalog.Product>()
            .WithColumn(nameof(ProductConfiguration.IsConfigurable)).AsBoolean().NotNullable()
            .WithColumn(nameof(ProductConfiguration.ConfigurationGroups)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(ProductConfiguration.PricingRules)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(ProductConfiguration.EnableCascadingParameters)).AsBoolean().NotNullable()
            .WithColumn(nameof(ProductConfiguration.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(ProductConfiguration.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}