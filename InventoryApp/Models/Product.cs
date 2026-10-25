using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

namespace InventoryApp.Models;

[Table("Products")]
public class Product
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string Name { get; set; } = string.Empty;

    [Indexed]
    public string Barcode { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal Quantity { get; set; }
}