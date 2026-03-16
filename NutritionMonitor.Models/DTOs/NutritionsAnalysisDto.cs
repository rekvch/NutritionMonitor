namespace NutritionMonitor.Models.DTOs;

using NutritionMonitor.Models.Enums;

/// <summary>
/// Output of the BLL nutritional deficit calculation.
/// Carries per-nutrient scores and final classification.
/// </summary>
public class NutritionAnalysisDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime AnalysisDate { get; set; }

    // Per-nutrient deficit percentages (0–100%, higher = more deficient)
    public double CalorieDeficitPercent { get; set; }
    public double ProteinDeficitPercent { get; set; }
    public double CarbohydrateDeficitPercent { get; set; }
    public double FatDeficitPercent { get; set; }
    public double IronDeficitPercent { get; set; }
    public double CalciumDeficitPercent { get; set; }
    public double VitaminADeficitPercent { get; set; }
    public double VitaminCDeficitPercent { get; set; }

    // Weighted total deficit score
    public double WeightedDeficitPercent { get; set; }

    // Final DOH RENI-based classification
    public NutritionalStatus Classification { get; set; }
}