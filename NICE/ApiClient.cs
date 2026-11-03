using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Devices;

namespace NICE;

// Spricht mit dem Backend (REST API). Bei Fehlern wird eine Exception geworfen,
// die die Seite abfängt und als Meldung anzeigt.
public static class ApiClient
{
    // ADRESSE DES BACKENDS:
    //  - Android-Emulator:                  http://10.0.2.2:5080
    //  - Handy per USB (adb reverse) und Windows-Test: http://localhost:5080
    //  - Handy per WLAN: hier die IP deines Laptops eintragen, z.B. "http://192.168.1.50:5080"
    public static string BaseUrl { get; set; } =
        DeviceInfo.DeviceType == DeviceType.Virtual ? "http://10.0.2.2:5080" : "http://localhost:5080";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(5) };
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private static string Url(string suffix = "") => $"{BaseUrl}/api/foods{suffix}";

    // READ
    public static async Task<List<LogEntry>> GetAllAsync()
    {
        var response = await Http.GetAsync(Url());
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<LogEntry>>(Json) ?? new();
    }

    // CREATE (das Backend vergibt die Id und schickt den gespeicherten Eintrag zurück)
    public static async Task<LogEntry> CreateAsync(LogEntry entry)
    {
        var response = await Http.PostAsJsonAsync(Url(), entry, Json);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LogEntry>(Json)
               ?? throw new InvalidOperationException("Leere Antwort vom Backend.");
    }

    // DELETE (404 = gibt es dort schon nicht mehr, auch okay)
    public static async Task DeleteAsync(Guid id)
    {
        var response = await Http.DeleteAsync(Url($"/{id}"));
        if (response.StatusCode == HttpStatusCode.NotFound) return;
        response.EnsureSuccessStatusCode();
    }
}