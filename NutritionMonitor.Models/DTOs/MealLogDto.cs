namespace NutritionMonitor.Models.DTOs;

public class MealLogDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime LogDate { get; set; }
    public string MealType { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double ProteinGrams { get; set; }
    public double CarbohydratesGrams { get; set; }
    public double FatGrams { get; set; }
    public double IronMg { get; set; }
    public double CalciumMg { get; set; }
    public double VitaminAMcg { get; set; }
    public double VitaminCMg { get; set; }
    public double WeightKg { get; set; }
    public double HeightCm { get; set; }
    public string? Notes { get; set; }
}