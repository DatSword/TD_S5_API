using TD1.Models.DataManager;
using TD1.Models.Repository;
using TD1.Models;
using Microsoft.EntityFrameworkCore;
using TD1.Models.Mapper;
using TD1.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IDataRepository<Marque, MarqueDto, MarqueDetailDto>, MarqueManager>();
builder.Services.AddScoped<IDataRepository<Produit, ProduitDto, ProduitDetailDto>, ProduitManager>();
builder.Services.AddScoped<IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto>, TypeProduitManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ProduitsDBContext>(options =>
                options.UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres; password=postgres;"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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

app.UseCors("AllowAllOrigins");

app.Run();
