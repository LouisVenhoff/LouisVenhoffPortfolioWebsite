using portfolio_backend.Data;
using portfolio_backend.Services;
using dotenv.net;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<RepoUpdateService>();

//Load Environment variables from .env file here and write it in native environement vars
DotEnv.Load();

foreach(KeyValuePair<string, string> entry in DotEnv.Read()){
    Environment.SetEnvironmentVariable(entry.Key, entry.Value);
}

var connectionString = builder.Configuration.GetConnectionString("Default");
Console.WriteLine(connectionString);
builder.Services.AddDbContext<ApplicationDbContext>((options) => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
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

app.Run();
