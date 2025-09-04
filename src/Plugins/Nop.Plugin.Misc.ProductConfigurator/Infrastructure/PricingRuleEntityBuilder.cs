using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class PricingRuleEntityBuilder : NopEntityBuilder<PricingRule>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(PricingRule.ProductConfigurationId)).AsInt32().NotNullable()
            .WithColumn(nameof(PricingRule.Name)).AsString(400).NotNullable()
            .WithColumn(nameof(PricingRule.Description)).AsString(4000).Nullable()
            .WithColumn(nameof(PricingRule.RuleType)).AsInt32().NotNullable()
            .WithColumn(nameof(PricingRule.Conditions)).AsString(4000).Nullable()
            .WithColumn(nameof(PricingRule.Formula)).AsString(1000).Nullable()
            .WithColumn(nameof(PricingRule.MinimumPrice)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(PricingRule.MaximumPrice)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(PricingRule.IsActive)).AsBoolean().NotNullable()
            .WithColumn(nameof(PricingRule.Priority)).AsInt32().NotNullable()
            .WithColumn(nameof(PricingRule.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(PricingRule.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}