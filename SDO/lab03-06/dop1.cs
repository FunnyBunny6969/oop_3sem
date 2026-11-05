using System;

public static class Dop1
{
    public abstract class BankAccount
    {
        public string AccountNumber { get; }
        public string Owner { get; }
        public decimal Balance { get; protected set; }
        private readonly string _pin;

        protected BankAccount(string accountNumber, string owner, decimal balance, string pin)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            Balance = balance;
            _pin = pin;
        }

        public bool CheckPin(string pin) => pin == _pin;

        public abstract void Deposit(decimal amount, string pin);
        public abstract void Withdraw(decimal amount, string pin);
    }

    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; }

        public SavingsAccount(string number, string owner, decimal balance, string pin, decimal interestRate)
            : base(number, owner, balance, pin)
        {
            InterestRate = interestRate;
        }

        public override void Deposit(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            Balance += amount;
        }

        public override void Withdraw(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            if (Balance - amount < 1000m)
                throw new InvalidOperationException("На сберегательном счёте должен оставаться минимум 1000 руб.");
            Balance -= amount;
        }

        public void AddInterest() => Balance += Balance * InterestRate / 100m;
    }

    public class CheckingAccount : BankAccount
    {
        public decimal Commission { get; }

        public CheckingAccount(string number, string owner, decimal balance, string pin, decimal commission)
            : base(number, owner, balance, pin)
        {
            Commission = commission;
        }

        public override void Deposit(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            Balance += amount;
        }

        public override void Withdraw(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            decimal total = amount + Commission;
            if (Balance < total) throw new InvalidOperationException("Недостаточно средств с учётом комиссии.");
            Balance -= total;
        }
    }

    public class CreditAccount : BankAccount
    {
        public decimal CreditLimit { get; }
        public decimal Debt { get; private set; }

        public CreditAccount(string number, string owner, decimal balance, string pin, decimal creditLimit)
            : base(number, owner, balance, pin)
        {
            CreditLimit = creditLimit;
        }

        public override void Deposit(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            Balance += amount;
        }

        public override void Withdraw(decimal amount, string pin)
        {
            if (!CheckPin(pin)) throw new UnauthorizedAccessException("Неверный PIN.");
            if (amount <= 0) throw new ArgumentException("Сумма должна быть положительной.");
            if (Balance < amount)
            {
                decimal creditPart = amount - Balance;
                if (Debt + creditPart > CreditLimit)
                    throw new InvalidOperationException("Превышен кредитный лимит.");
                Debt += creditPart;
                Balance = 0;
            }
            else
            {
                Balance -= amount;
            }
        }
    }

    public static void Run()
    {
        BankAccount[] accounts =
        {
            new SavingsAccount("S-001", "Иван Петров", 10000m, "1111", 5m),
            new CheckingAccount("C-002", "Анна Смирнова", 8000m, "2222", 100m),
            new CreditAccount("K-003", "Петр Сидоров", 3000m, "3333", 20000m)
        };

        foreach (BankAccount account in accounts)
        {
            Console.WriteLine($"{account.GetType().Name}: {account.Owner}, баланс {account.Balance:F2} руб.");
            try
            {
                account.Deposit(5000m, "0000");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Ошибка: {ex.Message}");
            }

            try
            {
                account.Withdraw(4000m, "1111");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Ошибка: {ex.Message}");
            }

            Console.WriteLine($"  Новый баланс: {account.Balance:F2} руб.");
        }

        SavingsAccount savings = (SavingsAccount)accounts[0];
        savings.AddInterest();
        Console.WriteLine($"После начисления процентов: {savings.Balance:F2} руб.");
    }
}
