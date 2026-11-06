using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

    public static class BookCatalog
    {
        private const string FileName = "books_catalog.xml";

        public static void CreateInitialXmlIfNotExist()
        {
            if (File.Exists(FileName)) return;

            XDocument doc = new XDocument(
                new XElement("Catalog",
                    new XElement("Book", new XAttribute("Id", "1"),
                        new XElement("Title", "Теория всего"),
                        new XElement("Author", "Стивен Хокинг"),
                        new XElement("Genre", "Наука"),
                        new XElement("Year", "2002"),
                        new XElement("Price", "450.50")),
                    new XElement("Book", new XAttribute("Id", "2"),
                        new XElement("Title", "Чистый код"),
                        new XElement("Author", "Роберт Мартин"),
                        new XElement("Genre", "IT"),
                        new XElement("Year", "2023"),
                        new XElement("Price", "1200.00")),
                    new XElement("Book", new XAttribute("Id", "3"),
                        new XElement("Title", "Грокаем алгоритмы"),
                        new XElement("Author", "Адитья Бхаргава"),
                        new XElement("Genre", "IT"),
                        new XElement("Year", "2021"),
                        new XElement("Price", "850.00")),
                    new XElement("Book", new XAttribute("Id", "4"),
                        new XElement("Title", "Дюна"),
                        new XElement("Author", "Фрэнк Герберт"),
                        new XElement("Genre", "Фантастика"),
                        new XElement("Year", "1965"),
                        new XElement("Price", "600.00")),
                    new XElement("Book", new XAttribute("Id", "5"),
                        new XElement("Title", "Астрофизика с космической скоростью"),
                        new XElement("Author", "Нил Деграсс Тайсон"),
                        new XElement("Genre", "Наука"),
                        new XElement("Year", "2022"),
                        new XElement("Price", "520.00"))
                )
            );

            doc.Save(FileName);
            Console.WriteLine($"[INFO] Создан начальный файл: {Path.GetFullPath(FileName)}\n");
        }

        public static void FindBooksByAuthor(string author)
        {
            Console.WriteLine($"=== Книги автора: {author} ===");
            XDocument doc = XDocument.Load(FileName);

            var books = doc.Root.Elements("Book")
                .Where(b => b.Element("Author")?.Value.Equals(author, StringComparison.OrdinalIgnoreCase) == true)
                .Select(b => b.Element("Title")?.Value);

            foreach (var title in books)
            {
                Console.WriteLine($"- {title}");
            }
            Console.WriteLine();
        }

        public static void FindRecentBooks()
        {
            int currentYear = 2026;
            int startYear = currentYear - 5;
            Console.WriteLine($"=== Книги, изданные за последние 5 лет ({startYear}-{currentYear}) ===");

            XDocument doc = XDocument.Load(FileName);

            var recentBooks = doc.Root.Elements("Book")
                .Where(b => {
                    int year = int.TryParse(b.Element("Year")?.Value, out int y) ? y : 0;
                    return year >= startYear && year <= currentYear;
                })
                .Select(b => new {
                    Title = b.Element("Title")?.Value,
                    Year = b.Element("Year")?.Value
                });

            foreach (var book in recentBooks)
            {
                Console.WriteLine($"- {book.Title} ({book.Year} г.)");
            }
            Console.WriteLine();
        }

        public static void CalculateAveragePriceByGenre()
        {
            Console.WriteLine("=== Средняя цена книг по жанрам ===");
            XDocument doc = XDocument.Load(FileName);

            var genres = doc.Root.Elements("Book")
                .GroupBy(b => b.Element("Genre")?.Value ?? "Без жанра")
                .Select(g => new {
                    Genre = g.Key,
                    AvgPrice = g.Average(b => double.TryParse(b.Element("Price")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double p) ? p : 0)
                });

            foreach (var item in genres)
            {
                Console.WriteLine($"- Жанр: {item.Genre} -> Средняя цена: {item.AvgPrice:F2} руб.");
            }
            Console.WriteLine();
        }

        public static void GroupBooksByFirstLetter()
        {
            Console.WriteLine("=== Группировка книг по первой букве названия ===");
            XDocument doc = XDocument.Load(FileName);

            var grouped = doc.Root.Elements("Book")
                .Select(b => b.Element("Title")?.Value ?? string.Empty)
                .Where(title => !string.IsNullOrEmpty(title))
                .GroupBy(title => char.ToUpper(title[0]))
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                Console.WriteLine($"Буква [{group.Key}]:");
                foreach (var title in group)
                {
                    Console.WriteLine($"  * {title}");
                }
            }
            Console.WriteLine();
        }

        public static void AddBook(string title, string author, string genre, int year, double price)
        {
            XDocument doc = XDocument.Load(FileName);

            int maxId = doc.Root.Elements("Book")
                .Select(b => int.TryParse(b.Attribute("Id")?.Value, out int id) ? id : 0)
                .DefaultIfEmpty(0)
                .Max();

            XElement newBook = new XElement("Book", new XAttribute("Id", (maxId + 1).ToString()),
                new XElement("Title", title),
                new XElement("Author", author),
                new XElement("Genre", genre),
                new XElement("Year", year.ToString()),
                new XElement("Price", price.ToString(CultureInfo.InvariantCulture))
            );

            doc.Root.Add(newBook);
            doc.Save(FileName);

            Console.WriteLine($"[УСПЕХ] Добавлена книга: \"{title}\" (ID: {maxId + 1})\n");
        }

        public static void Run()
        {
            CreateInitialXmlIfNotExist();

            FindBooksByAuthor("роберт Мартин");
            FindBooksByAuthor("Роберт Мартин");

            FindRecentBooks();

            CalculateAveragePriceByGenre();

            GroupBooksByFirstLetter();

            Console.WriteLine("--- Тестирование добавления новой книги ---");
            AddBook("Совершенный код", "Стив Макконнелл", "IT", 2024, 1500.00);

            Console.WriteLine("--- Проверка изменений после добавления книги ---");
            GroupBooksByFirstLetter();
        }
    }