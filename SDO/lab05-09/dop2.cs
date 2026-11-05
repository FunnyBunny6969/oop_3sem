using System;
using System.Collections.Generic;

public static class Dop2
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message) { }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message) { }
    }

    public class InvalidAmountException : Exception
    {
        public InvalidAmountException(string message) : base(message) { }
    }

    public class BankAccount
    {
        public string Number { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string number, decimal balance)
        {
            Number = number;
            Balance = balance;
        }

        public void Deposit(decimal amount) => Balance += amount;
        public void Withdraw(decimal amount) => Balance -= amount;
    }

    public class Bank
    {
        private readonly Dictionary<string, BankAccount> _accounts = new Dictionary<string, BankAccount>();

        public void AddAccount(BankAccount account) => _accounts[account.Number] = account;

        public void Transfer(string fromNumber, string toNumber, decimal amount)
        {
            if (amount <= 0) throw new InvalidAmountException("Сумма перевода должна быть положительной.");

            if (!_accounts.TryGetValue(fromNumber, out BankAccount? from))
                throw new AccountNotFoundException($"Счёт {fromNumber} не найден.");
            if (!_accounts.TryGetValue(toNumber, out BankAccount? to))
                throw new AccountNotFoundException($"Счёт {toNumber} не найден.");

            if (from.Balance < amount)
                throw new InsufficientFundsException($"На счёте {fromNumber} недостаточно средств.");

            from.Withdraw(amount);
            try
            {
                to.Deposit(amount);
            }
            catch
            {
                from.Deposit(amount);
                throw;
            }

            Console.WriteLine($"Перевод {amount:F2} руб. со счёта {fromNumber} на {toNumber} выполнен.");
        }
    }

    public static void Run()
    {
        Bank bank = new Bank();
        bank.AddAccount(new BankAccount("A-001", 10000m));
        bank.AddAccount(new BankAccount("A-002", 5000m));

        try
        {
            bank.Transfer("A-001", "A-002", 3000m);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            bank.Transfer("A-001", "A-999", 1000m);
        }
        catch (AccountNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            bank.Transfer("A-001", "A-002", 50000m);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        try
        {
            bank.Transfer("A-001", "A-002", -100m);
        }
        catch (InvalidAmountException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("Итоговые балансы:");
        Console.WriteLine($"  A-001: 10000 - 3000 = 7000 руб.");
        Console.WriteLine($"  A-002: 5000 + 3000 = 8000 руб.");
    }
}
