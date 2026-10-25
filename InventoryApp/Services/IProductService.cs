using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InventoryApp.Models;

namespace InventoryApp.Services;

public interface IProductService
{
    Task InitializeAsync();

    Task<List<Product>> GetProductsAsync(string? searchQuery = null);

    Task<Product?> GetProductByIdAsync(int id);

    Task<int> SaveProductAsync(Product product);

    Task<int> DeleteProductAsync(Product product);

    Task SeedDatabaseAsync(int count = 2500);
}