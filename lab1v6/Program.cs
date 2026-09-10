using System;
class BankAccount
{
    private string owner;
    private int accountNumber;
    public decimal Balance {get; private set;}
    
    public BankAccount(string owner, int accountNumber, decimal balance)
    {
        this.owner = owner;
        this.accountNumber = accountNumber;
        Balance = balance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if(amount <= Balance)
        {
            Balance -= amount;
        }
        else
        {
            Console.WriteLine("Insufficient funds");
        }
    }
}

class Program
{
    static void Main()
    {
        BankAccount account1 = new BankAccount("Семен Шишка", 123457, 1000);
        BankAccount account2 = new BankAccount("Іван Вишня", 127597, 500);
        BankAccount account3 = new BankAccount("Гриць Вулик", 890457, 2000);

        account1.Deposit(500);
        account1.Withdraw(200);

        account2.Deposit(300);
        account2.Withdraw(100);

        account3.Withdraw(500);

        Console.WriteLine($"Баланс Семена Шишки: {account1.Balance} грн");
        Console.WriteLine($"Баланс Івана Вишні: {account2.Balance} грн");
        Console.WriteLine($"Баланс Гриця Вулика: {account3.Balance} грн");        
    }
}