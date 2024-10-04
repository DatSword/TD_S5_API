using TD1.Models.DataManager;
using TD1.Models.Repository;
using TD1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IDataRepository<Marque>, MarqueManager>();
builder.Services.AddScoped<IDataRepository<Produit>, ProduitManager>();
builder.Services.AddScoped<IDataRepository<TypeProduit>, TypeProduitManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ProduitsDBContext>(options =>
                options.UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres; password=postgres;"));


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
