using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class DimensionRuleEntityBuilder : NopEntityBuilder<DimensionRule>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(DimensionRule.ProductConfigurationId)).AsInt32().NotNullable()
            .WithColumn(nameof(DimensionRule.DimensionType)).AsString(100).NotNullable()
            .WithColumn(nameof(DimensionRule.MinValue)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(DimensionRule.MaxValue)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(DimensionRule.DefaultValue)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(DimensionRule.StepValue)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(DimensionRule.Unit)).AsString(50).NotNullable()
            .WithColumn(nameof(DimensionRule.PricePerUnit)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(DimensionRule.PricingFormula)).AsString(1000).Nullable()
            .WithColumn(nameof(DimensionRule.IsRequired)).AsBoolean().NotNullable()
            .WithColumn(nameof(DimensionRule.DisplayOrder)).AsInt32().NotNullable()
            .WithColumn(nameof(DimensionRule.ValidationRules)).AsString(4000).Nullable()
            .WithColumn(nameof(DimensionRule.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(DimensionRule.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}