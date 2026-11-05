using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop3
{
    public enum RoomType
    {
        Standard,
        SemiSuite,
        Suite
    }

    public class Room
    {
        public int Number { get; init; }
        public RoomType Type { get; init; }
        public decimal BasePrice { get; init; }
    }

    public class Service
    {
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }

    public class Guest
    {
        public string Name { get; init; } = string.Empty;
    }

    public class Payment
    {
        public decimal Amount { get; init; }
        public string Method { get; init; } = "Карта";

        public void Pay() => Console.WriteLine($"  [Оплата] {Amount:F2} руб. оплачено способом \"{Method}\"");
    }

    public class Booking
    {
        public int Id { get; init; }
        public Guest Guest { get; init; } = null!;
        public Room Room { get; init; } = null!;
        public DateTime CheckIn { get; init; }
        public DateTime CheckOut { get; init; }
        public List<Service> Services { get; init; } = new List<Service>();
        public decimal TotalPrice { get; init; }
        public string Status { get; set; } = "Активно";
        public int Nights => (CheckOut - CheckIn).Days;
    }

    public class Hotel
    {
        private int _nextBookingId = 1;
        private readonly List<Room> _rooms = new List<Room>();
        private readonly List<Booking> _bookings = new List<Booking>();
        private readonly List<Service> _services = new List<Service>();

        public void AddRoom(Room room) => _rooms.Add(room);
        public void AddService(Service service) => _services.Add(service);

        private bool IsAvailable(Room room, DateTime checkIn, DateTime checkOut) =>
            !_bookings.Any(b => b.Room == room
                                && b.Status != "Отменено"
                                && b.CheckIn < checkOut
                                && checkIn < b.CheckOut);

        public List<Room> SearchAvailable(DateTime checkIn, DateTime checkOut, RoomType? type = null)
        {
            if (checkOut <= checkIn) throw new ArgumentException("Дата выезда должна быть позже даты заезда.");

            return _rooms
                .Where(r => type == null || r.Type == type)
                .Where(r => IsAvailable(r, checkIn, checkOut))
                .OrderBy(r => r.BasePrice)
                .ToList();
        }

        public Booking Book(Guest guest, Room room, DateTime checkIn, DateTime checkOut, IEnumerable<Service> services)
        {
            Console.WriteLine($"  [Бронирование] Гость {guest.Name} выбирает номер {room.Number} ({room.Type})");
            Console.WriteLine($"  [Бронирование] Проверка доступности с {checkIn:dd.MM.yyyy} по {checkOut:dd.MM.yyyy}");

            if (!IsAvailable(room, checkIn, checkOut))
                throw new InvalidOperationException($"Номер {room.Number} недоступен на выбранные даты.");

            List<Service> serviceList = services.ToList();
            int nights = (checkOut - checkIn).Days;
            decimal total = nights * room.BasePrice + serviceList.Sum(s => s.Price);

            Booking booking = new Booking
            {
                Id = _nextBookingId++,
                Guest = guest,
                Room = room,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Services = serviceList,
                TotalPrice = total
            };

            _bookings.Add(booking);
            Console.WriteLine($"  [Бронирование] Номер доступен, создана бронь №{booking.Id} на {nights} ноч.");
            foreach (Service service in serviceList)
            {
                Console.WriteLine($"  [Услуга] {service.Name}: {service.Price:F2} руб.");
            }
            new Payment { Amount = total, Method = "Банковская карта" }.Pay();
            return booking;
        }

        public void Cancel(int bookingId)
        {
            Booking booking = _bookings.FirstOrDefault(b => b.Id == bookingId)
                              ?? throw new ArgumentException($"Бронь №{bookingId} не найдена.");

            if (booking.Status == "Отменено")
            {
                Console.WriteLine($"  Бронь №{bookingId} уже отменена.");
                return;
            }

            booking.Status = "Отменено";
            Console.WriteLine($"  [Бронирование] Бронь №{bookingId} для {booking.Guest.Name} отменена, номер {booking.Room.Number} освобождён.");
        }

        public void PrintBookings()
        {
            Console.WriteLine("Бронирования:");
            if (_bookings.Count == 0)
            {
                Console.WriteLine("  Броней нет.");
                return;
            }

            foreach (Booking booking in _bookings)
            {
                Console.WriteLine($"  №{booking.Id}: {booking.Guest.Name}, номер {booking.Room.Number} ({booking.Room.Type}), " +
                                  $"{booking.CheckIn:dd.MM.yyyy}-{booking.CheckOut:dd.MM.yyyy}, {booking.TotalPrice:F2} руб., {booking.Status}");
            }
        }
    }

    public static void Run()
    {
        Hotel hotel = new Hotel();
        hotel.AddRoom(new Room { Number = 101, Type = RoomType.Standard, BasePrice = 3000m });
        hotel.AddRoom(new Room { Number = 102, Type = RoomType.SemiSuite, BasePrice = 5000m });
        hotel.AddRoom(new Room { Number = 201, Type = RoomType.Suite, BasePrice = 9000m });

        Service breakfast = new Service { Name = "Завтрак", Price = 500m };
        Service transfer = new Service { Name = "Трансфер", Price = 1500m };
        Service spa = new Service { Name = "Спа", Price = 2000m };
        hotel.AddService(breakfast);
        hotel.AddService(transfer);
        hotel.AddService(spa);

        DateTime checkIn = DateTime.Today.AddDays(1);
        DateTime checkOut = DateTime.Today.AddDays(4);
        Guest guest = new Guest { Name = "Иван Петров" };

        Console.WriteLine("Поиск доступных номеров:");
        foreach (Room room in hotel.SearchAvailable(checkIn, checkOut))
        {
            Console.WriteLine($"  Номер {room.Number}: {room.Type}, {room.BasePrice:F2} руб./ночь");
        }

        Console.WriteLine("\nБронирование номера 101:");
        Room selected = hotel.SearchAvailable(checkIn, checkOut, RoomType.Standard).First();
        Booking booking = hotel.Book(guest, selected, checkIn, checkOut, new[] { breakfast, transfer });
        Console.WriteLine($"  Итоговая стоимость: {booking.TotalPrice:F2} руб. за {booking.Nights} ноч.");

        Console.WriteLine("\nПопытка повторного бронирования того же номера:");
        try
        {
            hotel.Book(new Guest { Name = "Анна Смирнова" }, selected, checkIn.AddDays(1), checkOut.AddDays(1), Array.Empty<Service>());
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Отказ: {ex.Message}");
        }

        hotel.PrintBookings();

        Console.WriteLine("\nОтмена брони и повторный поиск:");
        hotel.Cancel(booking.Id);
        foreach (Room room in hotel.SearchAvailable(checkIn, checkOut, RoomType.Standard))
        {
            Console.WriteLine($"  Снова доступен номер {room.Number}");
        }
    }
}
