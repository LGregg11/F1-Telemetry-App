using F1GameTelemetryAPI.Helper;
using F1GameTelemetry.Listener;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

SetFirebaseCredentials();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(_ =>
{
    return FirestoreDb.Create("f1telemetryapp");
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

void SetFirebaseCredentials()
{
    var path = $"{AppDomain.CurrentDomain.BaseDirectory}..\\..\\..\\..\\credentials.json";
    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
}
