using System;
using System.Collections.Generic;
using System.Linq;

public static class AdvancedTask1
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class Sale
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime Date { get; set; }

        public decimal Total => Quantity * UnitPrice;
    }

    public static void Run()
    {
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Ноутбук", Price = 75000m },
            new Product { Id = 2, Name = "Смартфон", Price = 45000m },
            new Product { Id = 3, Name = "Наушники", Price = 5000m },
            new Product { Id = 4, Name = "Клавиатура", Price = 3500m },
            new Product { Id = 5, Name = "Монитор", Price = 18000m },
            new Product { Id = 6, Name = "Принтер", Price = 12000m }
        };

        List<Customer> customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Иван Петров" },
            new Customer { Id = 2, Name = "Анна Смирнова" },
            new Customer { Id = 3, Name = "Петр Сидоров" },
            new Customer { Id = 4, Name = "Мария Иванова" },
            new Customer { Id = 5, Name = "Ольга Кузнецова" }
        };

        List<Sale> sales = new List<Sale>
        {
            new Sale { Id = 1, ProductId = 1, CustomerId = 1, Quantity = 1, UnitPrice = 75000m, Date = new DateTime(2024, 1, 10) },
            new Sale { Id = 2, ProductId = 2, CustomerId = 2, Quantity = 2, UnitPrice = 45000m, Date = new DateTime(2024, 1, 15) },
            new Sale { Id = 3, ProductId = 3, CustomerId = 3, Quantity = 1, UnitPrice = 5000m, Date = new DateTime(2024, 1, 20) },
            new Sale { Id = 4, ProductId = 1, CustomerId = 2, Quantity = 1, UnitPrice = 75000m, Date = new DateTime(2024, 2, 5) },
            new Sale { Id = 5, ProductId = 4, CustomerId = 1, Quantity = 3, UnitPrice = 3500m, Date = new DateTime(2024, 2, 11) },
            new Sale { Id = 6, ProductId = 5, CustomerId = 4, Quantity = 2, UnitPrice = 18000m, Date = new DateTime(2024, 2, 18) },
            new Sale { Id = 7, ProductId = 2, CustomerId = 1, Quantity = 1, UnitPrice = 45000m, Date = new DateTime(2024, 3, 2) },
            new Sale { Id = 8, ProductId = 3, CustomerId = 5, Quantity = 4, UnitPrice = 5000m, Date = new DateTime(2024, 3, 9) },
            new Sale { Id = 9, ProductId = 1, CustomerId = 4, Quantity = 1, UnitPrice = 75000m, Date = new DateTime(2024, 3, 15) },
            new Sale { Id = 10, ProductId = 5, CustomerId = 2, Quantity = 1, UnitPrice = 18000m, Date = new DateTime(2024, 3, 21) },
            new Sale { Id = 11, ProductId = 4, CustomerId = 3, Quantity = 2, UnitPrice = 3500m, Date = new DateTime(2024, 3, 27) },
            new Sale { Id = 12, ProductId = 2, CustomerId = 5, Quantity = 1, UnitPrice = 45000m, Date = new DateTime(2024, 3, 30) },
            new Sale { Id = 13, ProductId = 3, CustomerId = 1, Quantity = 2, UnitPrice = 5000m, Date = new DateTime(2024, 3, 30) }
        };

        Console.WriteLine("1) Топ-5 самых продаваемых товаров по количеству продаж:");
        var topProducts = (from sale in sales
                           group sale by sale.ProductId into salesByProduct
                           orderby salesByProduct.Sum(s => s.Quantity) descending
                           select new
                           {
                               ProductId = salesByProduct.Key,
                               SoldQuantity = salesByProduct.Sum(s => s.Quantity)
                           })
                           .Take(5)
                           .Join(products,
                                 item => item.ProductId,
                                 product => product.Id,
                                 (item, product) => new { product.Name, item.SoldQuantity });

        foreach (var item in topProducts)
        {
            Console.WriteLine($"   {item.Name} — {item.SoldQuantity} шт.");
        }

        Console.WriteLine();
        Console.WriteLine("2) Самый прибыльный клиент:");
        var bestCustomer = (from sale in sales
                            group sale by sale.CustomerId into salesByCustomer
                            orderby salesByCustomer.Sum(s => s.Total) descending
                            select new
                            {
                                CustomerId = salesByCustomer.Key,
                                TotalAmount = salesByCustomer.Sum(s => s.Total)
                            })
                            .Join(customers,
                                  item => item.CustomerId,
                                  customer => customer.Id,
                                  (item, customer) => new { customer.Name, item.TotalAmount })
                            .FirstOrDefault();

        if (bestCustomer != null)
        {
            Console.WriteLine($"   {bestCustomer.Name} — {bestCustomer.TotalAmount:F2} руб.");
        }

        Console.WriteLine();
        Console.WriteLine("3) Статистика продаж по месяцам с нарастающим итогом:");
        var monthlyStats = sales
            .GroupBy(sale => new { sale.Date.Year, sale.Date.Month })
            .Select(groupByMonth => new
            {
                groupByMonth.Key.Year,
                groupByMonth.Key.Month,
                Total = groupByMonth.Sum(sale => sale.Total)
            })
            .OrderBy(month => month.Year)
            .ThenBy(month => month.Month)
            .ToList();

        decimal runningTotal = 0;
        foreach (var month in monthlyStats)
        {
            runningTotal += month.Total;
            Console.WriteLine($"   {month.Month:00}.{month.Year}: {month.Total,12:F2} руб. | нарастающий итог: {runningTotal,12:F2} руб.");
        }

        Console.WriteLine();
        Console.WriteLine("4) Товары, которые ни разу не продавались:");
        var soldProductIds = sales.Select(sale => sale.ProductId).Distinct();
        var unsoldProducts = from product in products
                             where !soldProductIds.Contains(product.Id)
                             select product;

        foreach (var product in unsoldProducts)
        {
            Console.WriteLine($"   {product.Name}");
        }

        Console.WriteLine();
        Console.WriteLine("5) Средний чек по каждому клиенту:");
        var averageChecks = from sale in sales
                            group sale by sale.CustomerId into salesByCustomer
                            select new
                            {
                                CustomerId = salesByCustomer.Key,
                                AverageCheck = salesByCustomer.Average(s => s.Total)
                            };

        var averageChecksWithNames = from check in averageChecks
                                     join customer in customers on check.CustomerId equals customer.Id
                                     orderby customer.Name
                                     select new { customer.Name, check.AverageCheck };

        foreach (var item in averageChecksWithNames)
        {
            Console.WriteLine($"   {item.Name} — {item.AverageCheck:F2} руб.");
        }
    }
}
