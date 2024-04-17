using Cloud;

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