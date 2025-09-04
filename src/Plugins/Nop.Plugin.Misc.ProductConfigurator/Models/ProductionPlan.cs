namespace Nop.Plugin.Misc.ProductConfigurator.Models;

/// <summary>
/// Generated production plan for a configuration
/// </summary>
public partial class ProductionPlan
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the configuration summary
    /// </summary>
    public string ConfigurationSummary { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the generated date
    /// </summary>
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the production operations in sequence
    /// </summary>
    public List<ProductionStep> Operations { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the total production time in minutes
    /// </summary>
    public decimal TotalProductionTime { get; set; }
    
    /// <summary>
    /// Gets or sets the total setup time in minutes
    /// </summary>
    public decimal TotalSetupTime { get; set; }
    
    /// <summary>
    /// Gets or sets the total production cost
    /// </summary>
    public decimal TotalProductionCost { get; set; }
    
    /// <summary>
    /// Gets or sets the estimated delivery date
    /// </summary>
    public DateTime EstimatedDeliveryDate { get; set; }
    
    /// <summary>
    /// Gets or sets the work centers involved
    /// </summary>
    public List<string> WorkCenters { get; set; } = new();
    
    /// <summary>
    /// Gets or sets critical path operations
    /// </summary>
    public List<int> CriticalPath { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the configuration used to generate this plan
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the dimensions used in calculations
    /// </summary>
    public Dictionary<string, decimal> Dimensions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets production planning warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// Individual production step with timing and requirements
/// </summary>
public partial class ProductionStep
{
    /// <summary>
    /// Gets or sets the operation ID
    /// </summary>
    public int OperationId { get; set; }
    
    /// <summary>
    /// Gets or sets the operation name
    /// </summary>
    public string OperationName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the operation type
    /// </summary>
    public OperationType OperationType { get; set; }
    
    /// <summary>
    /// Gets or sets the work center
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the sequence order
    /// </summary>
    public int SequenceOrder { get; set; }
    
    /// <summary>
    /// Gets or sets the setup time in minutes
    /// </summary>
    public decimal SetupTime { get; set; }
    
    /// <summary>
    /// Gets or sets the cycle time in minutes
    /// </summary>
    public decimal CycleTime { get; set; }
    
    /// <summary>
    /// Gets or sets the total time (setup + cycle) in minutes
    /// </summary>
    public decimal TotalTime { get; set; }
    
    /// <summary>
    /// Gets or sets the cost for this operation
    /// </summary>
    public decimal OperationCost { get; set; }
    
    /// <summary>
    /// Gets or sets whether this operation can run in parallel
    /// </summary>
    public bool CanRunInParallel { get; set; }
    
    /// <summary>
    /// Gets or sets dependencies on other operations
    /// </summary>
    public List<int> Dependencies { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the generated CAM program
    /// </summary>
    public CamProgram? CamProgram { get; set; }
    
    /// <summary>
    /// Gets or sets machining parameters
    /// </summary>
    public Dictionary<string, object> MachiningParameters { get; set; } = new();
    
    /// <summary>
    /// Gets or sets quality requirements
    /// </summary>
    public Dictionary<string, object> QualityRequirements { get; set; } = new();
    
    /// <summary>
    /// Gets or sets calculation details
    /// </summary>
    public string CalculationDetails { get; set; } = string.Empty;
}

/// <summary>
/// Generated CAM/CNC program
/// </summary>
public partial class CamProgram
{
    /// <summary>
    /// Gets or sets the program name
    /// </summary>
    public string ProgramName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the operation type this program is for
    /// </summary>
    public OperationType OperationType { get; set; }
    
    /// <summary>
    /// Gets or sets the generated G-code or program content
    /// </summary>
    public string ProgramContent { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets tool requirements
    /// </summary>
    public List<ToolRequirement> ToolRequirements { get; set; } = new();
    
    /// <summary>
    /// Gets or sets machining parameters
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    
    /// <summary>
    /// Gets or sets estimated program run time
    /// </summary>
    public decimal EstimatedRunTime { get; set; }
    
    /// <summary>
    /// Gets or sets safety notes and warnings
    /// </summary>
    public List<string> SafetyNotes { get; set; } = new();
}

/// <summary>
/// Tool requirement for CAM program
/// </summary>
public partial class ToolRequirement
{
    /// <summary>
    /// Gets or sets the tool number
    /// </summary>
    public int ToolNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the tool description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the tool diameter
    /// </summary>
    public decimal Diameter { get; set; }
    
    /// <summary>
    /// Gets or sets the tool material
    /// </summary>
    public string Material { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets cutting parameters
    /// </summary>
    public Dictionary<string, object> CuttingParameters { get; set; } = new();
}

/// <summary>
/// Production export formats
/// </summary>
public enum ProductionExportFormat
{
    Json = 1,
    Xml = 2,
    Csv = 3,
    Pdf = 4,
    GCode = 5,
    Excel = 6
}

/// <summary>
/// Production validation result
/// </summary>
public partial class ProductionValidationResult
{
    /// <summary>
    /// Gets or sets whether production is feasible
    /// </summary>
    public bool IsFeasible { get; set; } = true;
    
    /// <summary>
    /// Gets or sets validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();
    
    /// <summary>
    /// Gets or sets validation warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();
    
    /// <summary>
    /// Gets or sets conflicting requirements
    /// </summary>
    public List<string> Conflicts { get; set; } = new();
    
    /// <summary>
    /// Gets or sets missing capabilities
    /// </summary>
    public List<string> MissingCapabilities { get; set; } = new();
}