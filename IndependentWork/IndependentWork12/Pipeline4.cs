using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class Pipeline4
{
    public static void ShowPopularProductsByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var filteredSales = sales.Where(s => s.Quantity >= 5);

        var salesWithProducts = filteredSales.Join(products, s => s.ProductId, p => p.Id, (s, p) => new
            {
                ProductId = p.Id,
                Quantity = s.Quantity,
                ProductName = p.Name,
                CustomerId = s.CustomerId
            });

        var salesWithCustomers = salesWithProducts.Join(customers,
            s => s.CustomerId,
            c => c.Id,
            (s, c) => new
            {
                City = c.City,
                ProductId = s.ProductId,
                ProductName = s.ProductName,
                Quantity = s.Quantity
            });

        var groupedSales = salesWithCustomers.GroupBy(s => new
        {
            City = s.City,
            ProductId = s.ProductId
        });

        var productStats = groupedSales.Select(g => new
        {
            City = g.Key.City,
            ProductName = g.First().ProductName,
            TotalQuantity = g.Sum(s => s.Quantity)
        });

        var sortedStats = productStats .OrderByDescending(p => p.TotalQuantity) .ToList();

        stopwatch.Stop();
        Console.WriteLine($"\nЧас виконання пайплайна 4: {stopwatch.Elapsed.TotalMilliseconds:F3} мс");

        foreach (var product in sortedStats)
        {
            Console.WriteLine($"Місто: {product.City}, Товар: {product.ProductName}, Продано: {product.TotalQuantity}");
        }
    }
}
