using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;


public class Pipeline3
{
    public static void ShowSalesByMonth(List<Sale> sales, List<Product> products)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var salesWithProducts = sales.Join(products, s => s.ProductId, p => p.Id, (s, p) => new
            {
                SaleDate = s.SaleDate,
                Quantity = s.Quantity,
                UnitPrice = p.UnitPrice
            });

        var groupedByMonth = salesWithProducts.GroupBy(s => new
        {
            Year = s.SaleDate.Year,
            Month = s.SaleDate.Month
        });

        var monthlyStats = groupedByMonth.Select(g => new
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            TotalQuantity = g.Sum(s => s.Quantity),
            TotalRevenue = g.Sum(s => s.Quantity * s.UnitPrice)
        });

        var sortedStats = monthlyStats.OrderBy(s => s.Year) .ThenBy(s => s.Month) .ToList();
        
        stopwatch.Stop();
        Console.WriteLine($"\nЧас виконання пайплайна 3: {stopwatch.Elapsed.TotalMilliseconds:F3} мс\n");

        foreach (var month in sortedStats)
        {
            Console.WriteLine($"Рік: {month.Year}, Місяць: {month.Month}, Продано: {month.TotalQuantity}, Дохід: {month.TotalRevenue}");
        }
    }
}