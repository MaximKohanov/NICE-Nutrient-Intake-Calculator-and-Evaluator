using System.Text.Json.Serialization;

namespace NICE;

// Ein auswählbares Lebensmittel. Nährwerte gelten pro 100 g.
public class Food
{
    public string Name { get; init; } = "";
    public string Label { get; init; } = "";       // Text in der Liste
    public string Unit { get; init; } = "g";       // "Stück" oder "g"
    public double GramsPerUnit { get; init; } = 1; // 1 Stück = ? Gramm (bei "g" immer 1)
    public double Kcal { get; init; }
    public double Protein { get; init; }
    public double Carbs { get; init; }
    public double Fat { get; init; }

    // Menge -> Gramm -> Nährwerte, fertig als Eintrag für die Liste/das Backend
    public LogEntry ToEntry(double quantity)
    {
        double grams = quantity * GramsPerUnit;
        double f = grams / 100.0;
        return new LogEntry
        {
            Name = Name,
            Quantity = quantity,
            Unit = Unit,
            Grams = grams,
            Kcal = Kcal * f,
            Protein = Protein * f,
            Carbs = Carbs * f,
            Fat = Fat * f,
            EatenAt = DateTime.Now
        };
    }
}

// Ein eingetragener Eintrag. Die Eigenschaften werden als JSON zum Backend gesendet.
public class LogEntry
{
    public Guid Id { get; set; }                   // vergibt das Backend
    public string Name { get; set; } = "";
    public double Quantity { get; set; }
    public string Unit { get; set; } = "g";
    public double Grams { get; set; }
    public double Kcal { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fat { get; set; }
    public DateTime EatenAt { get; set; } = DateTime.Now;

    // Nur für die Anzeige, werden nicht gespeichert
    [JsonIgnore] public string Detail => $"{Quantity:0.##} {Unit}  ·  {Grams:0} g  ·  {Kcal:0} kcal";
    [JsonIgnore] public string Macros => $"Eiweiss {Protein:0.#} g  ·  Kohlenhydrate {Carbs:0.#} g  ·  Fett {Fat:0.#} g";
}

// Hier stehen alle Lebensmittel. Neue Zeile kopieren = neues Lebensmittel.
public static class FoodCatalog
{
    public static readonly List<Food> All = new()
    {
        new Food { Name = "Eier",          Label = "Eier (1 Ei ≈ 70 g)",
                   Unit = "Stück", GramsPerUnit = 70,
                   Kcal = 143, Protein = 12.6, Carbs = 0.7,  Fat = 9.5 },

        new Food { Name = "Rinderfleisch", Label = "Rinderfleisch (in Gramm)",
                   Unit = "g", GramsPerUnit = 1,
                   Kcal = 150, Protein = 21.0, Carbs = 0.0,  Fat = 7.0 },

        new Food { Name = "Apfel",         Label = "Apfel (1 mittelgrosser ≈ 180 g)",
                   Unit = "Stück", GramsPerUnit = 180,
                   Kcal = 52,  Protein = 0.3,  Carbs = 13.8, Fat = 0.2 },

        new Food { Name = "Banane",        Label = "Banane (1 mittelgrosse ≈ 120 g)",
                   Unit = "Stück", GramsPerUnit = 120,
                   Kcal = 89,  Protein = 1.1,  Carbs = 22.8, Fat = 0.3 },
    };
}