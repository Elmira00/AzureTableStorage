using Azure.Data.Tables;
using AzureStorageLibrary;
using AzureStorageLibrary.Services;
using AzureTableStoragePractice.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ConnectionString.AzureStorageConnectionString = builder.Configuration.GetConnectionString("StorageConStr");//burda con stringi yaziriq.
builder.Services.AddScoped(typeof(INoSqlStorage<>),typeof(TableStorageService<>));
//generic olduguna  gore yuxaridaki syntax'le inject edende avtomatik ozu tapir.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
