using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class Pipeline1
{
    public static void ShowTopProductsByCategory(List<Sale> sales, List<Product> products)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var groupedSales = sales.GroupBy(s => s.ProductId);
        var productTotals = groupedSales.Select(g => new{ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity)});
        var productsInfo = productTotals.Join(products, p => p.ProductId, product => product.Id, (p, product) => new
            {
                ProductId = p.ProductId,
                Name = product.Name,
                Category = product.Category,
                TotalQuantity = p.TotalQuantity
            });
        var groupedCategories = productsInfo.GroupBy(p => p.Category);
        var topProducts = groupedCategories.Select(g => new {Category = g.Key, Products = g.OrderByDescending(p => p.TotalQuantity).Take(3).ToList()}).ToList();
        stopwatch.Stop();
        Console.WriteLine($"\nЧас виконання пайплайна 1: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");

        foreach (var category in topProducts)
        {
            Console.WriteLine($"\nКатегорія: {category.Category}");
            foreach (var product in category.Products)
            {
                Console.WriteLine($"Товар: {product.Name}, Продано: {product.TotalQuantity}");
            }
        }
    }
    
}