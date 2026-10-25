using InventoryApp.Models;
using SQLite;

namespace InventoryApp.Services;

public class ProductService : IProductService
{
    private SQLiteAsyncConnection? _database;

    public async Task InitializeAsync()
    {
        if (_database is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "inventory.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Product>();

        // Ha üres az adatbázis, töltsünk fel tesztadatokat a több ezer elemes teszteléshez
        var count = await _database.Table<Product>().CountAsync();
        if (count == 0)
        {
            await SeedDatabaseAsync(2500);
        }
    }

    public async Task<List<Product>> GetProductsAsync(string? searchQuery = null)
    {
        await InitializeAsync();

        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return await _database!.Table<Product>()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        var query = searchQuery.Trim().ToLowerInvariant();

        // Név és vonalkód alapú keresés
        return await _database!.Table<Product>()
            .Where(p => p.Name.ToLower().Contains(query) || p.Barcode.ToLower().Contains(query))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await InitializeAsync();
        return await _database!.Table<Product>()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> SaveProductAsync(Product product)
    {
        await InitializeAsync();

        if (product.Id != 0)
        {
            return await _database!.UpdateAsync(product);
        }

        return await _database!.InsertAsync(product);
    }

    public async Task<int> DeleteProductAsync(Product product)
    {
        await InitializeAsync();
        return await _database!.DeleteAsync(product);
    }

    public async Task SeedDatabaseAsync(int count = 2500)
    {
        await InitializeAsync();

        var random = new Random();
        var categories = new[] { "Tejtermék", "Pékáru", "Elektronika", "Ital", "Irodaszer", "Tisztítószer" };
        var demoProducts = new List<Product>(count);

        for (int i = 1; i <= count; i++)
        {
            var category = categories[random.Next(categories.Length)];
            // Véletlenszerűen 0 készlet generálása a piros kiemelés teszteléséhez (~20% eséllyel)
            var quantity = random.Next(0, 5) == 0 ? 0 : random.Next(1, 150);

            demoProducts.Add(new Product
            {
                Name = $"{category} Termék #{i}",
                Barcode = $"590{random.Next(100000000, 999999999)}",
                Price = Math.Round((decimal)(random.NextDouble() * 8000 + 150), 2),
                Quantity = quantity
            });
        }

        // Tömeges beszúrás tranzakcióban a villámgyors betöltésért
        await _database!.InsertAllAsync(demoProducts);
    }
}