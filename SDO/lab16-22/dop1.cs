using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop1
{
    public class Book
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Author { get; init; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public List<Review> Reviews { get; } = new List<Review>();
    }

    public class Reader
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public List<Loan> Loans { get; } = new List<Loan>();
        public List<Reservation> Reservations { get; } = new List<Reservation>();
    }

    public class Loan
    {
        public Book Book { get; init; } = null!;
        public Reader Reader { get; init; } = null!;
        public DateTime LoanDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned => ReturnDate.HasValue;
        public bool IsOverdue => !IsReturned && DueDate < DateTime.Today;

        public decimal CalculateFine(decimal dailyRate)
        {
            if (!IsOverdue) return 0;
            return (DateTime.Today - DueDate).Days * dailyRate;
        }
    }

    public class Fine
    {
        public Reader Reader { get; init; } = null!;
        public Loan Loan { get; init; } = null!;
        public decimal Amount { get; init; }
        public bool IsPaid { get; set; }
    }

    public class Reservation
    {
        public Book Book { get; init; } = null!;
        public Reader Reader { get; init; } = null!;
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class Review
    {
        public Reader Reader { get; init; } = null!;
        public Book Book { get; init; } = null!;
        public int Rating { get; init; }
        public string Text { get; init; } = string.Empty;
    }

    public class Library
    {
        private const decimal DailyFine = 10m;
        private readonly List<Book> _books = new List<Book>();
        private readonly List<Reader> _readers = new List<Reader>();
        private readonly List<Loan> _loans = new List<Loan>();
        private readonly List<Fine> _fines = new List<Fine>();
        private readonly List<Reservation> _reservations = new List<Reservation>();
        private readonly List<Review> _reviews = new List<Review>();

        public void AddBook(Book book) => _books.Add(book);
        public void AddReader(Reader reader) => _readers.Add(reader);

        private Book FindBook(string title) =>
            _books.FirstOrDefault(b => b.Title == title)
            ?? throw new ArgumentException($"Книга \"{title}\" не найдена.");

        private Reader FindReader(string name) =>
            _readers.FirstOrDefault(r => r.Name == name)
            ?? throw new ArgumentException($"Читатель \"{name}\" не найден.");

        public Loan BorrowBook(string title, string readerName, DateTime dueDate)
        {
            Book book = FindBook(title);
            Reader reader = FindReader(readerName);

            if (!book.IsAvailable)
                throw new InvalidOperationException($"Книга \"{title}\" уже выдана. Оформите резервирование.");

            Loan loan = new Loan
            {
                Book = book,
                Reader = reader,
                LoanDate = DateTime.Today,
                DueDate = dueDate
            };

            book.IsAvailable = false;
            reader.Loans.Add(loan);
            _loans.Add(loan);
            Console.WriteLine($"  Книга \"{title}\" выдана читателю {readerName} до {dueDate:dd.MM.yyyy}");
            return loan;
        }

        public Loan BorrowBook(string title, string readerName, int loanDays = 14) =>
            BorrowBook(title, readerName, DateTime.Today.AddDays(loanDays));

        public decimal ReturnBook(string title)
        {
            Book book = FindBook(title);
            Loan loan = _loans.FirstOrDefault(l => l.Book == book && !l.IsReturned)
                        ?? throw new InvalidOperationException($"Книга \"{title}\" не выдана.");

            decimal fine = loan.CalculateFine(DailyFine);
            bool wasOverdue = loan.IsOverdue;
            loan.ReturnDate = DateTime.Today;
            book.IsAvailable = true;

            if (fine > 0)
            {
                _fines.Add(new Fine { Reader = loan.Reader, Loan = loan, Amount = fine });
            }

            Console.WriteLine($"  Книга \"{title}\" возвращена читателем {loan.Reader.Name}");
            if (wasOverdue)
            {
                Console.WriteLine($"  Просрочка, начислен штраф: {fine:F2} руб.");
            }

            Reservation? reservation = _reservations.FirstOrDefault(r => r.Book == book && r.IsActive);
            if (reservation != null)
            {
                reservation.IsActive = false;
                Console.WriteLine($"  Уведомление: читатель {reservation.Reader.Name} может получить зарезервированную книгу \"{title}\".");
            }

            return fine;
        }

        public Reservation ReserveBook(string title, string readerName)
        {
            Book book = FindBook(title);
            Reader reader = FindReader(readerName);

            if (book.IsAvailable)
                throw new InvalidOperationException("Книга доступна, резервирование не требуется.");

            if (_reservations.Any(r => r.Book == book && r.Reader == reader && r.IsActive))
                throw new InvalidOperationException("Читатель уже зарезервировал эту книгу.");

            Reservation reservation = new Reservation { Book = book, Reader = reader };
            _reservations.Add(reservation);
            reader.Reservations.Add(reservation);
            Console.WriteLine($"  Читатель {readerName} зарезервировал книгу \"{title}\"");
            return reservation;
        }

        public Review AddReview(string title, string readerName, int rating, string text)
        {
            Book book = FindBook(title);
            Reader reader = FindReader(readerName);
            Review review = new Review { Book = book, Reader = reader, Rating = rating, Text = text };
            book.Reviews.Add(review);
            _reviews.Add(review);
            Console.WriteLine($"  Добавлен отзыв на \"{title}\": {rating}/5, \"{text}\"");
            return review;
        }

        public void NotifyDueSoon(int days)
        {
            Console.WriteLine($"Уведомления о возврате в ближайшие {days} дн.:");
            List<Loan> dueSoon = _loans
                .Where(l => !l.IsReturned && l.DueDate <= DateTime.Today.AddDays(days))
                .ToList();

            if (dueSoon.Count == 0)
            {
                Console.WriteLine("  Нет ближайших возвратов.");
                return;
            }

            foreach (Loan loan in dueSoon)
            {
                string status = loan.IsOverdue ? "просрочена" : "скоро срок возврата";
                Console.WriteLine($"  {loan.Reader.Name}: \"{loan.Book.Title}\" — {status}, срок {loan.DueDate:dd.MM.yyyy}");
            }
        }

        public void ShowFines()
        {
            Console.WriteLine("Штрафы:");
            if (_fines.Count == 0)
            {
                Console.WriteLine("  Штрафов нет.");
                return;
            }

            foreach (Fine fine in _fines)
            {
                Console.WriteLine($"  {fine.Reader.Name}: {fine.Amount:F2} руб. за \"{fine.Loan.Book.Title}\"");
            }
        }

        public void ShowReservations()
        {
            Console.WriteLine("Активные резервирования:");
            List<Reservation> active = _reservations.Where(r => r.IsActive).ToList();
            if (active.Count == 0)
            {
                Console.WriteLine("  Нет активных резервирований.");
                return;
            }

            foreach (Reservation reservation in active)
            {
                Console.WriteLine($"  {reservation.Reader.Name}: \"{reservation.Book.Title}\"");
            }
        }
    }

    public static void Run()
    {
        Library library = new Library();
        library.AddBook(new Book { Id = 1, Title = "CLR via C#", Author = "Джеффри Рихтер" });
        library.AddBook(new Book { Id = 2, Title = "Алгоритмы", Author = "Роберт Седжвик" });
        library.AddBook(new Book { Id = 3, Title = "Чистый код", Author = "Роберт Мартин" });

        library.AddReader(new Reader { Id = 1, Name = "Иван Петров" });
        library.AddReader(new Reader { Id = 2, Name = "Анна Смирнова" });

        Console.WriteLine("Выдача с просрочкой:");
        library.BorrowBook("CLR via C#", "Иван Петров", DateTime.Today.AddDays(-5));
        library.ReturnBook("CLR via C#");
        library.ShowFines();

        Console.WriteLine("\nВыдача и уведомление о сроке:");
        library.BorrowBook("Алгоритмы", "Иван Петров", DateTime.Today.AddDays(2));
        library.NotifyDueSoon(3);

        Console.WriteLine("\nРезервирование выданной книги:");
        library.BorrowBook("Чистый код", "Иван Петров", 10);
        library.ReserveBook("Чистый код", "Анна Смирнова");
        library.ShowReservations();
        library.ReturnBook("Чистый код");
        library.ShowReservations();

        Console.WriteLine("\nОтзыв:");
        library.AddReview("Чистый код", "Анна Смирнова", 5, "Полезная книга о качестве кода.");
    }
}
