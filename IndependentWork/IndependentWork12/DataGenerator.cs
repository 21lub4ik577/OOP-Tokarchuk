using System;
using System.Collections.Generic;
class DataGenerator
{
    public static (List<Sale> Sales, List<Product> Products, List<Customer> Customers) GenerateData()
    {
        List<Sale> sales = new List<Sale>();
        List<Product> products = new List<Product>();
        List<Customer> customers = new List<Customer>();

        Random random = new Random(26);
        String[] names = {"Ноутбук", "Телефон", "Навушники", "Монітор", "Клавіатура"};
        String[] categories = {"Електроніка", "Аксесуари", "Комп'ютери"};
        for (int i = 0; i < 70; i++)
        {
            Product product = new Product();
            product.Id = i + 1;
            product.Name = names[random.Next(names.Length)];
            product.Category = categories[random.Next(categories.Length)];
            product.UnitPrice = random.Next(100, 5000);
            products.Add(product);
        }


        string[] customer_names = {"Антон", "Василь", "Петро", "Олена", "Ірина"};
        string[] customer_cities = {"Київ", "Березне", "Лондон", "Берлін"};
        for (int i = 0; i < 70; i++)
        {
            Customer customer = new Customer();
            customer.Id = i + 1;
            customer.Name = customer_names[random.Next(customer_names.Length)];
            customer.City = customer_cities[random.Next(customer_cities.Length)];
            customers.Add(customer);
        }


        for (int i = 0; i < 100; i++)
        {
            Sale sale = new Sale();
            sale.Id = i + 1;
            sale.ProductId = random.Next(1, 71);
            sale.Quantity = random.Next(1, 11);
            sale.SaleDate = new DateTime(2025, 1, 1).AddDays(random.Next(365));
            sale.CustomerId = random.Next(1, 71);
            sales.Add(sale);
        }

        return (sales, products, customers);
    }
}