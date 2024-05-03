using Edge;
using Edge.FeatureFlagging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost",
        builder =>
        {
            builder.WithOrigins("https://localhost:7267")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Logging.ClearProviders();
builder.Services.AddLogging(logging =>
    logging.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    })
);

builder.Services.Configure<FeatureFlaggerConfig>(
    builder.Configuration.GetSection("FeatureFlaggerConfig"));

builder.Services.AddSingleton<IClient, CloudClient>();
builder.Services.AddSingleton<IFeatureFlagger, UnleashFeatureFlagger>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection()
    .UseCors("AllowLocalHost")
    .UseAuthorization();

app.MapHub<ClientHub>("/clientHub");
app.MapGet("/", () => "Welcome to the Idlenomics Edge!");

await app.Services.GetService<IClient>().StartAsync();

var unleash = app.Services.GetService<IFeatureFlagger>();
await unleash.Initialize();

app.Run();

unleash.TearDown();