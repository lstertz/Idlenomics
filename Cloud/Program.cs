using Cloud;
using Cloud.Tracking;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalHost",
        builder =>
        {
            builder.WithOrigins("https://localhost:7285")
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

builder.Services.AddSingleton<IPlayerTracker, PlayerTracker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection()
    .UseCors("AllowLocalHost")
    .UseAuthorization();

app.MapHub<EdgeHub>("/edgeHub");
app.MapGet("/", () => "Welcome to the Idlenomics Cloud!");

app.Run();