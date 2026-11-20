using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        var data = DataGenerator.GenerateData();
        List<Sale> sales = data.Sales;
        List<Product> products = data.Products;
        List<Customer> customers = data.Customers;

        Pipeline1.ShowTopProductsByCategory(sales, products);
        Pipeline2.ShowRevenueByCity(sales, products, customers);
        Pipeline3.ShowSalesByMonth(sales, products);
        Pipeline4.ShowPopularProductsByCity(sales, products, customers);

    }
}