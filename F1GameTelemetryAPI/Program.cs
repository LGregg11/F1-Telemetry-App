using F1GameTelemetry.Listener;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.Logger.LogInformation("App Starting.");
app.Run();
app.Logger.LogInformation("App Started.");


RunListener();


void RunListener()
{
    TelemetryListener telemetryListener = new TelemetryListener(123);
    app.Logger.LogInformation("Listener Starting.");
    telemetryListener.Start();
    app.Logger.LogInformation("Listener Started.");
}
