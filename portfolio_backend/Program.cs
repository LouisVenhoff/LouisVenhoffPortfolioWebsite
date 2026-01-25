using portfolio_backend.Data;
using portfolio_backend.Services;
using dotenv.net;
using System;
using portfolio_backend.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SecretProvider>();

builder.Services.AddSingleton<GithubApi>();

builder.Services.AddSingleton<RepoUpdateService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
           policy.WithOrigins(CorsHelper.GetAllowedOrigins()).AllowAnyHeader().AllowAnyMethod(); 
        }
    });
});

//Load Environment variables from .env file here and write it in native environement vars
DotEnv.Load();

foreach(KeyValuePair<string, string> entry in DotEnv.Read()){
    Environment.SetEnvironmentVariable(entry.Key, entry.Value);
}

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ApplicationDbContext>((options) => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
