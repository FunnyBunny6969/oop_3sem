using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop4
{
    public class PhoneBook
    {
        private Dictionary<string, List<string>> _directBook = new Dictionary<string, List<string>>();
        private Dictionary<string, string> _inverseBook = new Dictionary<string, string>();

        public void AddContact(string lastName, string phone)
        {
            if (_inverseBook.ContainsKey(phone))
            {
                return;
            }

            if (!_directBook.ContainsKey(lastName))
            {
                _directBook[lastName] = new List<string>();
            }

            _directBook[lastName].Add(phone);
            _inverseBook[phone] = lastName;
        }

        public void AddAdditionalPhone(string lastName, string phone)
        {
            if (_inverseBook.ContainsKey(phone))
            {
                return;
            }

            if (_directBook.ContainsKey(lastName))
            {
                _directBook[lastName].Add(phone);
                _inverseBook[phone] = lastName;
            }
        }

        public void RemoveContact(string lastName)
        {
            if (_directBook.ContainsKey(lastName))
            {
                foreach (var phone in _directBook[lastName])
                {
                    _inverseBook.Remove(phone);
                }
                _directBook.Remove(lastName);
            }
        }

        public void RemovePhone(string lastName, string phone)
        {
            if (_directBook.ContainsKey(lastName) && _directBook[lastName].Contains(phone))
            {
                _directBook[lastName].Remove(phone);
                _inverseBook.Remove(phone);

                if (_directBook[lastName].Count == 0)
                {
                    _directBook.Remove(lastName);
                }
            }
        }

        public List<string> FindByLastName(string lastName)
        {
            if (_directBook.ContainsKey(lastName))
            {
                return new List<string>(_directBook[lastName]);
            }
            return new List<string>();
        }

        public string FindByPhone(string phone)
        {
            if (_inverseBook.ContainsKey(phone))
            {
                return _inverseBook[phone];
            }
            return null;
        }
    }


    public static void Run()
    {
        PhoneBook book = new PhoneBook();

        book.AddContact("Иванов", "+375291111111");
        book.AddContact("Петров", "+375292222222");

        book.AddAdditionalPhone("Иванов", "+375293333333");

        Console.WriteLine("Поиск по фамилии Иванов:");
        var ivanovPhones = book.FindByLastName("Иванов");
        foreach (var p in ivanovPhones) Console.WriteLine(p);

        Console.WriteLine("\nПоиск по телефону +375292222222:");
        Console.WriteLine(book.FindByPhone("+375292222222"));

        book.RemovePhone("Иванов", "+375291111111");
        Console.WriteLine("\nИванов после удаления первого телефона:");
        foreach (var p in book.FindByLastName("Иванов")) Console.WriteLine(p);

        book.RemoveContact("Петров");
        Console.WriteLine("\nПоиск Петрова после удаления контакта:");
        Console.WriteLine(book.FindByPhone("+375292222222") ?? "Не найдено");
    }
}
