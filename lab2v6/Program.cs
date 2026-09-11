using System;
class BankAccount
{
    //private str
    private string _owner;
    private string _accountNumber;
    private decimal _balance;

    //properties
    public string Owner
    {
        
        get {return _owner;}
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _owner = value;
            }
        }
    }

    public string AccountNumber
    {
        
        get {return _accountNumber;}
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _accountNumber = value;
            }
        }
    }

    public decimal Balance
    {
        
        get {return _balance;}
    }

    //Constructor Overloading
    public BankAccount(string owner, string accountNumber, decimal initialBalance)
    {
        _owner = owner;
        _accountNumber = accountNumber;
        _balance = initialBalance;
    }

    public BankAccount(string owner, string accountNumber): this(owner, accountNumber, 0)
    {
        
    }

    //Methods
    public void Deposit(decimal amount)
    {

        if (amount > 0)
        {
            _balance += amount;
        }
        else
        {
            Console.WriteLine("The amount must be greater than 0.");
        }

    }

    public void Withdraw(decimal amount)
    {
        
        if (amount > 0 && _balance >= amount)
        {
            _balance -= amount;
        }
        else
        {
            Console.WriteLine("Invalid amount or insufficient funds.");
        }

    }

    //finalizer
    ~BankAccount()
    {
        Console.WriteLine("BankAccount object is being destroyed.");
    }
}

class Program
{
    static void Main()
    {
        CreateAccounts();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    
    static void CreateAccounts()
    {
        BankAccount account1 = new BankAccount("Семен Шишка", "123457", 1000);
        BankAccount account2 = new BankAccount("Іван Вишня", "127597");
        BankAccount account3 = new BankAccount("Гриць Вулик", "890457", 2000);

        account1.Deposit(500);
        account1.Withdraw(200);

        account2.Deposit(300);
        account2.Withdraw(100);
        
        account3.Deposit(100);
        account3.Withdraw(500);

        Console.WriteLine($"Баланс Семена Шишки: {account1.Balance} грн");
        Console.WriteLine($"Баланс Івана Вишні: {account2.Balance} грн");
        Console.WriteLine($"Баланс Гриця Вулика: {account3.Balance} грн");  
    }

}