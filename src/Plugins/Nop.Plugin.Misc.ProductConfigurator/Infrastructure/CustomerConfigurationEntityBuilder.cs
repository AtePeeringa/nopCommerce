using FluentMigrator.Builders.Create.Table;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

public partial class CustomerConfigurationEntityBuilder : NopEntityBuilder<CustomerConfiguration>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerConfiguration.CustomerId)).AsInt32().NotNullable().ForeignKey<Nop.Core.Domain.Customers.Customer>()
            .WithColumn(nameof(CustomerConfiguration.ProductId)).AsInt32().NotNullable().ForeignKey<Nop.Core.Domain.Catalog.Product>()
            .WithColumn(nameof(CustomerConfiguration.ConfigurationData)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(CustomerConfiguration.TotalPrice)).AsDecimal(18, 4).NotNullable()
            .WithColumn(nameof(CustomerConfiguration.ConfigurationSummary)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(CustomerConfiguration.IsCompleted)).AsBoolean().NotNullable()
            .WithColumn(nameof(CustomerConfiguration.CreatedOnUtc)).AsDateTime2().NotNullable()
            .WithColumn(nameof(CustomerConfiguration.UpdatedOnUtc)).AsDateTime2().NotNullable();
    }
}