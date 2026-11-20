using System;
public class Sale
{
    public int Id;
    public int ProductId;
    public int Quantity;
    public DateTime SaleDate;
    public int CustomerId;
}

public class Product
{
    public int Id;
    public string Name = " ";
    public string Category = " ";
    public decimal UnitPrice;
}

public class Customer
{
    public int Id;
    public string Name = " ";
    public string City = " ";
}