using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a production operation/step in manufacturing
/// </summary>
public partial class ProductionOperation : BaseEntity
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the operation name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the operation type
    /// </summary>
    public OperationType OperationType { get; set; }
    
    /// <summary>
    /// Gets or sets the work center/machine required
    /// </summary>
    public string WorkCenter { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the conditions when this operation is required (JSON)
    /// </summary>
    public string RequiredConditions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the setup time formula (minutes)
    /// Examples: "15" (fixed), "holes * 2" (2 min per hole)
    /// </summary>
    public string SetupTimeFormula { get; set; } = "0";
    
    /// <summary>
    /// Gets or sets the cycle time formula (minutes)
    /// Examples: "area * 5" (5 min per m²), "perimeter * 0.5" (0.5 min per meter)
    /// </summary>
    public string CycleTimeFormula { get; set; } = "0";
    
    /// <summary>
    /// Gets or sets the cost per minute for this operation
    /// </summary>
    public decimal CostPerMinute { get; set; }
    
    /// <summary>
    /// Gets or sets CAM/CNC program template (JSON)
    /// </summary>
    public string CamProgram { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets machining parameters (JSON)
    /// Feed rates, spindle speeds, tool info, etc.
    /// </summary>
    public string MachiningParameters { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets quality control requirements (JSON)
    /// </summary>
    public string QualityRequirements { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the sequence order
    /// </summary>
    public int SequenceOrder { get; set; }
    
    /// <summary>
    /// Gets or sets whether this operation can run in parallel
    /// </summary>
    public bool CanRunInParallel { get; set; }
    
    /// <summary>
    /// Gets or sets dependencies on other operations
    /// </summary>
    public string Dependencies { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the operation is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the created date
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
    
    /// <summary>
    /// Gets or sets the updated date
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}

/// <summary>
/// Types of production operations
/// </summary>
public enum OperationType
{
    /// <summary>
    /// Material cutting (laser, plasma, water jet)
    /// </summary>
    Cutting = 1,
    
    /// <summary>
    /// CNC milling/machining
    /// </summary>
    Milling = 2,
    
    /// <summary>
    /// Drilling holes
    /// </summary>
    Drilling = 3,
    
    /// <summary>
    /// Bending/forming
    /// </summary>
    Forming = 4,
    
    /// <summary>
    /// Welding/joining
    /// </summary>
    Welding = 5,
    
    /// <summary>
    /// Surface finishing (grinding, polishing)
    /// </summary>
    Finishing = 6,
    
    /// <summary>
    /// Coating/painting
    /// </summary>
    Coating = 7,
    
    /// <summary>
    /// Assembly
    /// </summary>
    Assembly = 8,
    
    /// <summary>
    /// Quality inspection
    /// </summary>
    Inspection = 9,
    
    /// <summary>
    /// Packaging
    /// </summary>
    Packaging = 10,
    
    /// <summary>
    /// Heat treatment
    /// </summary>
    HeatTreatment = 11,
    
    /// <summary>
    /// Marking/engraving
    /// </summary>
    Marking = 12
}