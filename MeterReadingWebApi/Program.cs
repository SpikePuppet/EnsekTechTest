using MeterReadingDatabase;
using MeterReadingModels.Models;
using MeterReadingServices.Interfaces;
using MeterReadingServices.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MeterReadingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MeterReadingContext")));

builder.Services.AddScoped<ICsvParsingService<RawMeterReading>, CsvParsingService<RawMeterReading>>();
builder.Services.AddScoped<IEntityService<Account>, EntityService<Account>>();
builder.Services.AddScoped<IEntityService<MeterReading>, EntityService<MeterReading>>();
builder.Services.AddScoped<IMeterReadingParsingService, MeterReadingParsingService>();

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

app.Run();
