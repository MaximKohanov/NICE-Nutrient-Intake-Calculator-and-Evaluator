using System.Collections.ObjectModel;
using System.Globalization;

namespace NICE;

public partial class MainPage : ContentPage
{
    private Food? _selected;   // aktuell ausgewähltes Lebensmittel

    // Diese zwei Listen sind im XAML an die CollectionViews gebunden
    public ObservableCollection<Food> Matches { get; } = new();      // Treffer der Suche
    public ObservableCollection<LogEntry> Entries { get; } = new();  // eingetragene Lebensmittel

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        ApplyFilter("");
        UpdateTotal();
    }

    // Beim Anzeigen der Seite die gespeicherten Einträge vom Backend holen
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAsync();
    }

    private async void OnReloadClicked(object? sender, EventArgs e) => await LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            var list = await ApiClient.GetAllAsync();
            Entries.Clear();
            foreach (var entry in list) Entries.Add(entry);
            MessageLabel.Text = "";
        }
        catch (Exception)
        {
            MessageLabel.Text = $"Backend nicht erreichbar ({ApiClient.BaseUrl}). Läuft das Backend?";
        }
        UpdateTotal();
    }

    // ---------- Suchfeld und Dropdown ----------

    // Suchfeld angetippt: Liste öffnen
    private void OnSearchFocused(object? sender, FocusEventArgs e)
    {
        SearchBox.Text = "";
        ApplyFilter("");
        FoodList.IsVisible = true;
    }

    // Suchfeld verlassen: Liste schliessen (kurze Wartezeit, damit ein Tipp auf die Liste noch ankommt)
    private async void OnSearchUnfocused(object? sender, FocusEventArgs e)
    {
        await Task.Delay(250);
        if (!SearchBox.IsFocused) FoodList.IsVisible = false;
    }

    // Enter/Fertig auf der Tastatur: Liste schliessen
    private void OnSearchCompleted(object? sender, EventArgs e)
    {
        FoodList.IsVisible = false;
        SearchBox.Unfocus();
    }

    // Bei jedem Buchstaben die Liste neu filtern
    private void OnSearchChanged(object? sender, TextChangedEventArgs e) => ApplyFilter(e.NewTextValue);

    private void ApplyFilter(string? text)
    {
        var query = (text ?? "").Trim();
        Matches.Clear();
        foreach (var food in FoodCatalog.All)
        {
            if (food.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase))
                Matches.Add(food);
        }
    }

    // Lebensmittel in der Liste angetippt
    private void OnFoodSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Food food) return;

        _selected = food;
        SelectedLabel.Text = $"Ausgewählt: {food.Label}";
        QuantityEntry.Placeholder = food.Unit == "Stück"
            ? "Anzahl in Stück, z.B. 2"
            : "Menge in Gramm, z.B. 200";
        MessageLabel.Text = "";

        FoodList.SelectedItem = null;   // damit man dasselbe Lebensmittel später wieder wählen kann
        FoodList.IsVisible = false;
        SearchBox.Unfocus();
        QuantityEntry.Focus();
    }

    // ---------- Eintragen und Löschen ----------

    private async void OnAddClicked(object? sender, EventArgs e)
    {
        if (_selected is null)
        {
            MessageLabel.Text = "Bitte zuerst ein Lebensmittel aus der Liste wählen.";
            return;
        }

        var text = (QuantityEntry.Text ?? "").Trim().Replace(',', '.');
        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var quantity)
            || quantity <= 0 || quantity > 5000)
        {
            MessageLabel.Text = "Bitte eine Menge grösser als 0 eingeben.";
            return;
        }

        try
        {
            var saved = await ApiClient.CreateAsync(_selected.ToEntry(quantity));  // im Backend speichern
            Entries.Insert(0, saved);
        }
        catch (Exception)
        {
            MessageLabel.Text = "Speichern fehlgeschlagen: Backend nicht erreichbar.";
            return;
        }

        UpdateTotal();

        // Formular zurücksetzen
        MessageLabel.Text = "";
        _selected = null;
        SelectedLabel.Text = "Noch kein Lebensmittel ausgewählt";
        QuantityEntry.Text = "";
        QuantityEntry.Placeholder = "Menge";
        QuantityEntry.Unfocus();
        SearchBox.Text = "";
    }

    // Löschen-Knopf eines Eintrags
    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button || button.BindingContext is not LogEntry entry) return;

        try
        {
            await ApiClient.DeleteAsync(entry.Id);   // im Backend löschen
            Entries.Remove(entry);
            MessageLabel.Text = "";
        }
        catch (Exception)
        {
            MessageLabel.Text = "Löschen fehlgeschlagen: Backend nicht erreichbar.";
        }
        UpdateTotal();
    }

    private void UpdateTotal()
    {
        TotalLabel.Text = Entries.Count == 0
            ? "Noch nichts eingetragen."
            : $"Total: {Entries.Sum(x => x.Kcal):0} kcal  ·  Eiweiss {Entries.Sum(x => x.Protein):0} g  ·  " +
              $"Kohlenhydrate {Entries.Sum(x => x.Carbs):0} g  ·  Fett {Entries.Sum(x => x.Fat):0} g";
    }
}