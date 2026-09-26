using System;

class Payment
{
    public virtual void Process() => Console.WriteLine("Some processing");
}

class CreditCard : Payment
{
    public override void Process() => Console.WriteLine("Processing payment by credit card");
}

class Cash : Payment
{
    public new void Process() => Console.WriteLine("Processing payment by cash");
}

class Program()
{
    static void Main()
    {
        CreditCard obj1 = new CreditCard();
        Cash obj2 = new Cash();

        Payment paymentCreditCard = obj1;
        Payment paymentCash = obj2;

        Console.WriteLine("--- Using base class reference ---");
        paymentCreditCard.Process();
        paymentCash.Process();

        Console.WriteLine("\n--- Using derived class reference ---");
        obj1.Process();
        obj2.Process();

        Console.WriteLine("\n--- Using explicit casting ---");
        ((CreditCard)paymentCreditCard).Process();
        ((Cash)paymentCash).Process();

        Console.WriteLine("\nOverride provides polymorphism:");
        Console.WriteLine("CreditCard calls its own Process() even through a Payment reference.");

        Console.WriteLine("\nNew does not provide polymorphism:");
        Console.WriteLine("Cash calls the base class Process() when accessed through a Payment reference.");
    }
}