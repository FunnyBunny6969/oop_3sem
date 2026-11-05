using System;
using System.Collections.Generic;

public static class Dop3
{
    public abstract class Ticket
    {
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public decimal BasePrice { get; set; }
        public string SeatType { get; set; }

        protected Ticket(string eventName, DateTime eventDate, decimal basePrice, string seatType)
        {
            EventName = eventName;
            EventDate = eventDate;
            BasePrice = basePrice;
            SeatType = seatType;
        }

        public abstract decimal CalculatePrice();

        public override string ToString() =>
            $"{EventName} ({EventDate:dd.MM.yyyy}), место: {SeatType}, цена: {CalculatePrice():F2} руб.";
    }

    public class ConcertTicket : Ticket
    {
        public bool BackstagePass { get; set; }

        public ConcertTicket(string eventName, DateTime date, decimal price, string seatType, bool backstage)
            : base(eventName, date, price, seatType)
        {
            BackstagePass = backstage;
        }

        public override decimal CalculatePrice()
        {
            decimal price = BasePrice;
            if (SeatType == "VIP") price *= 1.5m;
            if (BackstagePass) price += 3000m;
            return price;
        }
    }

    public class CinemaTicket : Ticket
    {
        public bool IsMatinee { get; set; }

        public CinemaTicket(string eventName, DateTime date, decimal price, string seatType, bool matinee)
            : base(eventName, date, price, seatType)
        {
            IsMatinee = matinee;
        }

        public override decimal CalculatePrice()
        {
            decimal price = BasePrice;
            if (IsMatinee) price *= 0.8m;
            return price;
        }
    }

    public class TransportTicket : Ticket
    {
        public int Zone { get; set; }

        public TransportTicket(string eventName, DateTime date, decimal price, string seatType, int zone)
            : base(eventName, date, price, seatType)
        {
            Zone = zone;
        }

        public override decimal CalculatePrice() => BasePrice + Zone * 50m;
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message) { }
    }

    public static class TicketOffice
    {
        private static readonly Queue<string> SalesLog = new Queue<string>();

        public static decimal SellTickets(List<Ticket> tickets, decimal payment)
        {
            decimal total = 0;
            foreach (Ticket ticket in tickets)
            {
                total += ticket.CalculatePrice();
            }

            if (tickets.Count >= 3)
            {
                total *= 0.9m;
                Console.WriteLine($"Применена групповая скидка 10% для {tickets.Count} билетов.");
            }

            if (payment < total)
            {
                throw new InsufficientFundsException(
                    $"Недостаточно средств: требуется {total:F2} руб., внесено {payment:F2} руб.");
            }

            foreach (Ticket ticket in tickets)
            {
                SalesLog.Enqueue(ticket.ToString());
            }

            return payment - total;
        }

        public static void PrintLog()
        {
            Console.WriteLine("Журнал продаж:");
            while (SalesLog.Count > 0)
            {
                Console.WriteLine($"  {SalesLog.Dequeue()}");
            }
        }
    }

    public static void Run()
    {
        List<Ticket> tickets = new List<Ticket>
        {
            new ConcertTicket("Рок-концерт", new DateTime(2026, 12, 12), 2500m, "VIP", true),
            new CinemaTicket("Премьера фильма", new DateTime(2026, 11, 20), 800m, "Обычное", true),
            new TransportTicket("Экскурсия", new DateTime(2026, 11, 25), 300m, "Сидячее", 2)
        };

        foreach (Ticket ticket in tickets)
        {
            Console.WriteLine(ticket);
        }

        Console.WriteLine();

        try
        {
            decimal change = TicketOffice.SellTickets(tickets, 6000m);
            Console.WriteLine($"Билеты проданы. Сдача: {change:F2} руб.");
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Ошибка оплаты: {ex.Message}");
        }

        try
        {
            TicketOffice.SellTickets(tickets, 1000m);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Ошибка оплаты: {ex.Message}");
        }

        Console.WriteLine();
        TicketOffice.PrintLog();
    }
}
