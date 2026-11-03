using NICE.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FoodStore>();

var app = builder.Build();

// ---- REST API für Lebensmittel: vollständiges CRUD ----
var foods = app.MapGroup("/api/foods");

// READ (alle)
foods.MapGet("/", (FoodStore store) => Results.Ok(store.GetAll()));

// READ (einer)
foods.MapGet("/{id:guid}", (Guid id, FoodStore store) =>
    store.Get(id) is { } item ? Results.Ok(item) : Results.NotFound());

// CREATE
foods.MapPost("/", (FoodItem item, FoodStore store) =>
{
    if (!IsValid(item, out var error)) return Results.BadRequest(error);
    var created = store.Add(item);
    return Results.Created($"/api/foods/{created.Id}", created);
});

// UPDATE
foods.MapPut("/{id:guid}", (Guid id, FoodItem item, FoodStore store) =>
{
    if (!IsValid(item, out var error)) return Results.BadRequest(error);
    return store.Update(id, item) ? Results.Ok(item) : Results.NotFound();
});

// DELETE
foods.MapDelete("/{id:guid}", (Guid id, FoodStore store) =>
    store.Delete(id) ? Results.NoContent() : Results.NotFound());

// Auf allen Netzwerkkarten lauschen, damit das Handy den Laptop erreicht.
// (app.Run(url) überschreibt Einstellungen aus launchSettings.json)
app.Run("http://0.0.0.0:5080");

static bool IsValid(FoodItem item, out string error)
{
    if (string.IsNullOrWhiteSpace(item.Name)) { error = "Name fehlt."; return false; }
    if (item.Quantity <= 0) { error = "Menge muss grösser als 0 sein."; return false; }
    error = "";
    return true;
}