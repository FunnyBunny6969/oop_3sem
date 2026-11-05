using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop2
{
    public abstract class Discount
    {
        public abstract string Name { get; }
        public abstract decimal Apply(decimal subtotal, int quantity, Customer customer);
    }

    public class PercentageDiscount : Discount
    {
        private readonly decimal _percent;
        public PercentageDiscount(decimal percent) => _percent = percent;
        public override string Name => $"Процентная скидка {_percent}%";
        public override decimal Apply(decimal subtotal, int quantity, Customer customer) =>
            subtotal * (1 - _percent / 100m);
    }

    public class FixedDiscount : Discount
    {
        private readonly decimal _amount;
        public FixedDiscount(decimal amount) => _amount = amount;
        public override string Name => $"Фиксированная скидка {_amount:F2} руб.";
        public override decimal Apply(decimal subtotal, int quantity, Customer customer) =>
            Math.Max(0, subtotal - _amount);
    }

    public class QuantityDiscount : Discount
    {
        private readonly int _minQuantity;
        private readonly decimal _percent;

        public QuantityDiscount(int minQuantity, decimal percent)
        {
            _minQuantity = minQuantity;
            _percent = percent;
        }

        public override string Name => $"Скидка {_percent}% за количество от {_minQuantity} шт.";
        public override decimal Apply(decimal subtotal, int quantity, Customer customer) =>
            quantity >= _minQuantity ? subtotal * (1 - _percent / 100m) : subtotal;
    }

    public class LoyaltyDiscount : Discount
    {
        private readonly int _minOrders;
        private readonly decimal _percent;

        public LoyaltyDiscount(int minOrders, decimal percent)
        {
            _minOrders = minOrders;
            _percent = percent;
        }

        public override string Name => $"Скидка постоянному клиенту {_percent}%";
        public override decimal Apply(decimal subtotal, int quantity, Customer customer) =>
            customer.OrderCount >= _minOrders ? subtotal * (1 - _percent / 100m) : subtotal;
    }

    public class Product
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }

    public class CartItem
    {
        public Product Product { get; init; } = null!;
        public int Quantity { get; set; }
        public decimal Total => Product.Price * Quantity;
    }

    public class Customer
    {
        public string Name { get; init; } = string.Empty;
        public int OrderCount { get; set; }
    }

    public class Cart
    {
        private readonly List<CartItem> _items = new List<CartItem>();
        public IReadOnlyList<CartItem> Items => _items;

        public void Add(Product product, int quantity = 1)
        {
            CartItem? existing = _items.FirstOrDefault(i => i.Product == product);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }

        public bool Remove(Product product, int quantity = 1)
        {
            CartItem? existing = _items.FirstOrDefault(i => i.Product == product);
            if (existing == null) return false;
            existing.Quantity -= quantity;
            if (existing.Quantity <= 0) _items.Remove(existing);
            return true;
        }

        public decimal Subtotal => _items.Sum(i => i.Total);
        public int TotalQuantity => _items.Sum(i => i.Quantity);
    }

    public class Payment
    {
        public decimal Amount { get; init; }
        public string Method { get; init; } = "Карта";

        public bool Pay()
        {
            Console.WriteLine($"  Оплата {Amount:F2} руб. способом \"{Method}\" выполнена.");
            return true;
        }
    }

    public class Order
    {
        public Customer Customer { get; init; } = null!;
        public List<CartItem> Items { get; init; } = new List<CartItem>();
        public List<Discount> Discounts { get; } = new List<Discount>();

        public decimal Subtotal => Items.Sum(i => i.Total);
        public int TotalQuantity => Items.Sum(i => i.Quantity);

        public decimal CalculateTotal()
        {
            decimal total = Subtotal;
            Console.WriteLine($"  Подытог: {total:F2} руб.");
            foreach (Discount discount in Discounts)
            {
                decimal next = discount.Apply(total, TotalQuantity, Customer);
                Console.WriteLine($"  {discount.Name}: {total:F2} -> {next:F2} руб.");
                total = next;
            }
            return total;
        }

        public void Checkout()
        {
            decimal total = CalculateTotal();
            Payment payment = new Payment { Amount = total, Method = "Банковская карта" };
            payment.Pay();
            Customer.OrderCount++;
            Console.WriteLine($"  Заказ оформлен. Итоговая стоимость: {total:F2} руб.");
        }
    }

    public static void Run()
    {
        Product laptop = new Product { Id = 1, Name = "Ноутбук", Price = 1200m };
        Product mouse = new Product { Id = 2, Name = "Мышь", Price = 800m };

        Customer customer = new Customer { Name = "Иван Петров", OrderCount = 3 };
        Cart cart = new Cart();
        cart.Add(laptop, 2);
        cart.Add(mouse, 1);

        Console.WriteLine($"Корзина: {cart.TotalQuantity} товаров на {cart.Subtotal:F2} руб.");

        Order order = new Order
        {
            Customer = customer,
            Items = cart.Items.Select(i => new CartItem { Product = i.Product, Quantity = i.Quantity }).ToList()
        };

        order.Discounts.Add(new PercentageDiscount(10));
        order.Discounts.Add(new QuantityDiscount(2, 5));
        order.Discounts.Add(new LoyaltyDiscount(3, 5));
        order.Discounts.Add(new FixedDiscount(200));

        Console.WriteLine("Расчёт заказа:");
        order.Checkout();

        cart.Remove(laptop, 1);
        Console.WriteLine($"\nПосле удаления одного ноутбука: {cart.TotalQuantity} товаров на {cart.Subtotal:F2} руб.");
    }
}
