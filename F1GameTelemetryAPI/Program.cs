using F1GameTelemetryAPI.Helper;
using F1GameTelemetryAPI.Providers;
using F1GameTelemetry.Listener;
using FireSharp;
using FireSharp.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(_ =>
{
    // Default to using the emulator for now
    SetupFirebase();

    return new FirebaseProvider(new FirebaseClient(new FirebaseConfig
    {
         BasePath = Environment.GetEnvironmentVariable("FIRESTORE_EMULATOR_HOST")
    }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

RunListener();

app.Logger.LogInformation("App Starting.");
app.Run();
app.Logger.LogInformation("App Started.");

void RunListener()
{
    TelemetryListenerHelper.TelementryListener = new TelemetryListener(20777);
    
    app.Logger.LogInformation("Listener Starting.");
    TelemetryListenerHelper.TelementryListener.Start();
    app.Logger.LogInformation("Listener Started.");
}

void SetupFirebase()
{
    foreach(var line in File.ReadLines($"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\..\\credentials.txt"))
    {
        var keyval = line.Split('\t');
        var key = keyval[0].Trim().ToLower();
        var value = keyval[1].Trim();
        switch (key)
        {
            case "emulator host":
                Environment.SetEnvironmentVariable("FIRESTORE_EMULATOR_HOST", value);
                break;
            case "secret":
                Environment.SetEnvironmentVariable("FIRESTORE_SECRET", value);
                break;
            default:
                break;
        }
    }
}
