using Edge;
using Edge.Cloud;
using Edge.Features.Clients;
using Edge.Features.Flagging;
using Edge.Simulating;
using Edge.Users;

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

builder.Services.AddSingleton<ICloudClient, CloudClient>().
    AddHostedService(services => (CloudClient)services.GetService<ICloudClient>()!);

builder.Services.AddSingleton<IUserManager, UserManager>();
builder.Services.AddSingleton<IFeatureFlagger, UnleashFeatureFlagger>().
    AddHostedService(services => (UnleashFeatureFlagger)services.GetService<IFeatureFlagger>()!);
builder.Services.AddSingleton<IClientFeatureCoordinator, ClientFeatureCoordinator>();

builder.Services.AddHostedService<Simulator>();


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

app.Services.GetService<IClientFeatureCoordinator>();

app.Run();