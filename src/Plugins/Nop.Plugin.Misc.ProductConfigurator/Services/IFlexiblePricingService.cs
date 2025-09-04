using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

/// <summary>
/// Service for flexible formula-based pricing calculations
/// </summary>
public partial interface IFlexiblePricingService
{
    /// <summary>
    /// Calculates the total price for a configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions (area, perimeter, etc.)</param>
    /// <returns>Calculated price breakdown</returns>
    Task<FlexiblePriceBreakdown> CalculatePriceAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Gets all price components for a configurable product
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <returns>List of price components</returns>
    Task<IList<PriceComponent>> GetPriceComponentsAsync(int configurableProductId);
    
    /// <summary>
    /// Calculates dimensions based on configuration (area, perimeter, volume, etc.)
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>Dictionary of calculated dimensions</returns>
    Task<Dictionary<string, decimal>> CalculateDimensionsAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Evaluates a pricing formula with given variables
    /// </summary>
    /// <param name="formula">Formula to evaluate</param>
    /// <param name="variables">Variables available in the formula</param>
    /// <returns>Calculated result</returns>
    Task<decimal> EvaluateFormulaAsync(string formula, Dictionary<string, object> variables);
    
    /// <summary>
    /// Gets applicable price components based on configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>List of applicable price components</returns>
    Task<IList<PriceComponent>> GetApplicablePriceComponentsAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Validates pricing formulas
    /// </summary>
    /// <param name="formula">Formula to validate</param>
    /// <returns>Validation result</returns>
    Task<bool> ValidateFormulaAsync(string formula);
    
    /// <summary>
    /// Gets quantity breaks for a component
    /// </summary>
    /// <param name="component">Price component</param>
    /// <param name="quantity">Quantity to check</param>
    /// <returns>Price per unit for the quantity</returns>
    Task<decimal> GetQuantityBreakPriceAsync(PriceComponent component, decimal quantity);
    
    /// <summary>
    /// Creates a price component
    /// </summary>
    /// <param name="component">Price component to create</param>
    Task InsertPriceComponentAsync(PriceComponent component);
    
    /// <summary>
    /// Updates a price component
    /// </summary>
    /// <param name="component">Price component to update</param>
    Task UpdatePriceComponentAsync(PriceComponent component);
    
    /// <summary>
    /// Deletes a price component
    /// </summary>
    /// <param name="component">Price component to delete</param>
    Task DeletePriceComponentAsync(PriceComponent component);
}