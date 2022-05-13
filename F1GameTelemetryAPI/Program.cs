using F1GameTelemetryAPI.Helper;
using F1GameTelemetry.Listener;
using F1GameTelemetryAPI.Providers;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(_ =>
{
    var credentials = GetFirebaseCredentials();

    return new FirestoreProvider(
        new FirestoreDbBuilder
        {
            ProjectId = credentials["FirebaseProjectId"],
            JsonCredentials = credentials["FirebaseCredentials"]
        }
        .Build());
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

Dictionary<string,string> GetFirebaseCredentials()
{
    var credentials = new Dictionary<string,string>();

    using (StreamReader r = File.OpenText($"{Directory.GetCurrentDirectory()}\\..\\..\\..\\..\\credentials.txt"))
    {
        foreach (var line in r.ReadToEnd().Split("\n"))
        {
            if (!line.Contains(':')) continue;

            var keyval = line.Split(':');
            credentials.Add(keyval[0].Trim(), keyval[1].Trim());
        }
    }

    return credentials;
}
