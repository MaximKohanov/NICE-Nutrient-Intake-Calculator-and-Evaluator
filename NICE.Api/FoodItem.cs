namespace NICE.Api;

public class FoodItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public double Quantity { get; set; }          // Anzahl Stück oder Gramm
    public string Unit { get; set; } = "g";       // "Stück" oder "g"
    public double Grams { get; set; }             // Gesamtgewicht in Gramm
    public double Kcal { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
    public DateTime EatenAt { get; set; } = DateTime.Now;
}