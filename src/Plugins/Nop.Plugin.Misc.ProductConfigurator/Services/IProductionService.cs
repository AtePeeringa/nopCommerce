using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

/// <summary>
/// Service for production planning and CAM data generation
/// </summary>
public partial interface IProductionService
{
    /// <summary>
    /// Generates production plan for a configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Generated production plan</returns>
    Task<ProductionPlan> GenerateProductionPlanAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Gets all production operations for a configurable product
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <returns>List of production operations</returns>
    Task<IList<ProductionOperation>> GetProductionOperationsAsync(int configurableProductId);
    
    /// <summary>
    /// Gets applicable production operations based on configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>List of applicable operations</returns>
    Task<IList<ProductionOperation>> GetApplicableOperationsAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Calculates operation time based on formula
    /// </summary>
    /// <param name="operation">Production operation</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <param name="isSetupTime">Whether to calculate setup or cycle time</param>
    /// <returns>Calculated time in minutes</returns>
    Task<decimal> CalculateOperationTimeAsync(ProductionOperation operation, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions, bool isSetupTime);
    
    /// <summary>
    /// Generates CAM/CNC program for an operation
    /// </summary>
    /// <param name="operation">Production operation</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Generated CAM program</returns>
    Task<CamProgram> GenerateCamProgramAsync(ProductionOperation operation, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Estimates total production time
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Estimated production time in minutes</returns>
    Task<decimal> EstimateProductionTimeAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Calculates production cost (labor + machine time)
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <param name="dimensions">Calculated dimensions</param>
    /// <returns>Estimated production cost</returns>
    Task<decimal> CalculateProductionCostAsync(int configurableProductId, Dictionary<string, object> configuration, Dictionary<string, decimal> dimensions);
    
    /// <summary>
    /// Exports production plan to various formats
    /// </summary>
    /// <param name="productionPlan">Production plan</param>
    /// <param name="format">Export format</param>
    /// <returns>Exported data</returns>
    Task<string> ExportProductionPlanAsync(ProductionPlan productionPlan, ProductionExportFormat format);
    
    /// <summary>
    /// Validates production feasibility
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration values</param>
    /// <returns>Validation result</returns>
    Task<ProductionValidationResult> ValidateProductionAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Creates a production operation
    /// </summary>
    /// <param name="operation">Production operation to create</param>
    Task InsertProductionOperationAsync(ProductionOperation operation);
    
    /// <summary>
    /// Updates a production operation
    /// </summary>
    /// <param name="operation">Production operation to update</param>
    Task UpdateProductionOperationAsync(ProductionOperation operation);
    
    /// <summary>
    /// Deletes a production operation
    /// </summary>
    /// <param name="operation">Production operation to delete</param>
    Task DeleteProductionOperationAsync(ProductionOperation operation);
}