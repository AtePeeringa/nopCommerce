using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

/// <summary>
/// Service for Bill of Materials generation and management
/// </summary>
public partial interface IBomService
{
    /// <summary>
    /// Generates BOM for a configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Generated BOM</returns>
    Task<GeneratedBom> GenerateBomAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Gets all BOM components for a configurable product
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <returns>List of BOM components</returns>
    Task<IList<BomComponent>> GetBomComponentsAsync(int configurableProductId);
    
    /// <summary>
    /// Gets applicable BOM components based on configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>List of applicable components</returns>
    Task<IList<BomComponent>> GetApplicableBomComponentsAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Calculates component quantity based on formula
    /// </summary>
    /// <param name="component">BOM component</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Required quantity</returns>
    Task<decimal> CalculateComponentQuantityAsync(BomComponent component, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Estimates material cost for configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Estimated material cost</returns>
    Task<decimal> EstimateMaterialCostAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Exports BOM to various formats (CSV, XML, JSON)
    /// </summary>
    /// <param name="bom">Generated BOM</param>
    /// <param name="format">Export format</param>
    /// <returns>Exported data</returns>
    Task<string> ExportBomAsync(GeneratedBom bom, BomExportFormat format);
    
    /// <summary>
    /// Validates BOM completeness
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>Validation result</returns>
    Task<BomValidationResult> ValidateBomAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Creates a BOM component
    /// </summary>
    /// <param name="component">BOM component to create</param>
    Task InsertBomComponentAsync(BomComponent component);
    
    /// <summary>
    /// Updates a BOM component
    /// </summary>
    /// <param name="component">BOM component to update</param>
    Task UpdateBomComponentAsync(BomComponent component);
    
    /// <summary>
    /// Deletes a BOM component
    /// </summary>
    /// <param name="component">BOM component to delete</param>
    Task DeleteBomComponentAsync(BomComponent component);
}