using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SievalAPI.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("SievalDB") ?? "Trust Server Certificate=True;data source=OSOGP2\\SQL2017;initial catalog=Sieval;integrated security=True";

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSqlServer<SievalDB>(connectionString);

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sieval API",
                Description = "een eenvoudige API met CRUD-methodes voor artikelen",
                Version = "v1"
            });
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sieval API V1");
        });

        app.MapGet("/", () => "SievalAPI-products; use https://[host]:[port]/swagger/ for testing");


        #region CRUD routines
        app.MapGet("/products", async (SievalDB db) => await db.Products.ToListAsync());

        app.MapPost("/product", async (SievalDB db, Product product) =>
        {
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            return Results.Created($"/product/{product.ID}", product);
        });

        app.MapGet("/product/{id}", async (SievalDB db, Guid id) => await db.Products.FindAsync(id));

        app.MapPut("/product/{id}", async (SievalDB db, Product updateproduct, Guid id) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null) return Results.NotFound();
            product.SKU = updateproduct.SKU;
            product.Name = updateproduct.Name;
            product.Price = updateproduct.Price;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        app.MapDelete("/product/{id}", async (SievalDB db, Guid id) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product is null) return Results.NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
        #endregion

        app.Run();
    }
}

