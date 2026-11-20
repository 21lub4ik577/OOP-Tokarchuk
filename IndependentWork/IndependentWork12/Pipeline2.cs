using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class Pipeline2
{
    public static void ShowRevenueByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var salesWithProducts = sales.Join(products, s => s.ProductId, p => p.Id, (s, p) => new
            {
                Quantity = s.Quantity,
                CustomerId = s.CustomerId,
                UnitPrice = p.UnitPrice
            });

        var salesWithCustomers = salesWithProducts.Join(customers, s => s.CustomerId, c => c.Id, (s, c) => new
            {
                Quantity = s.Quantity,
                UnitPrice = s.UnitPrice,
                City = c.City
            });

        var salesWithRevenue = salesWithCustomers.Select(s => new
        {
            City = s.City,
            Quantity = s.Quantity,
            Revenue = s.Quantity * s.UnitPrice
        });

        var groupedByCity = salesWithRevenue.GroupBy(s => s.City);

        var cityRevenue = groupedByCity.Select(g => new
        {
            City = g.Key,
            TotalRevenue = g.Sum(s => s.Revenue)
        });

        var sortedRevenue = cityRevenue.OrderByDescending(c => c.TotalRevenue).ToList();

        stopwatch.Stop();
        Console.WriteLine($"\nЧас виконання пайплайна 2: {stopwatch.Elapsed.TotalMilliseconds:F3} мс\n");
        
        foreach (var city in sortedRevenue)
        {
            Console.WriteLine($"Місто: {city.City}, Загальний дохід: {city.TotalRevenue}");
        }
    }
}