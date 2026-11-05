using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop3
{
    public enum WorkType { Book, Audiobook, Magazine, Article }
    public enum AuthorContribution { Author, CoAuthor, Editor, Translator }
    public enum SearchCriterion { Title, Author, Year, Rating }
    public enum SortOrder { Ascending, Descending }
    public enum AccessLevel { Guest, Reader, Librarian }

    public struct Work
    {
        public string Title { get; set; }
        public WorkType Type { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public double Price { get; set; }

        public Work(string title, WorkType type, string author, int year, double rating, double price)
        {
            Title = title;
            Type = type;
            Author = author;
            Year = year;
            Rating = rating;
            Price = price;
        }

        public override string ToString() =>
            $"{Title} ({Type}), {Author}, {Year}, рейтинг {Rating:F1}, цена {Price:F2} руб.";
    }

    public struct LibraryCard
    {
        public string ReaderName { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool HasDebt { get; set; }

        public LibraryCard(string readerName, AccessLevel accessLevel, bool hasDebt)
        {
            ReaderName = readerName;
            AccessLevel = accessLevel;
            HasDebt = hasDebt;
        }
    }

    public class DigitalLibrary
    {
        private readonly List<Work> _works = new List<Work>();

        public void Add(Work work) => _works.Add(work);

        public IEnumerable<Work> Search(string query, SearchCriterion criterion)
        {
            return criterion switch
            {
                SearchCriterion.Title => _works.Where(w => w.Title.Contains(query, StringComparison.OrdinalIgnoreCase)),
                SearchCriterion.Author => _works.Where(w => w.Author.Contains(query, StringComparison.OrdinalIgnoreCase)),
                SearchCriterion.Year => _works.Where(w => w.Year.ToString() == query),
                SearchCriterion.Rating => _works.Where(w => w.Rating >= double.Parse(query)),
                _ => Enumerable.Empty<Work>()
            };
        }

        public IEnumerable<Work> Sort(SearchCriterion criterion, SortOrder order)
        {
            Func<Work, object> key = criterion switch
            {
                SearchCriterion.Title => w => w.Title,
                SearchCriterion.Author => w => w.Author,
                SearchCriterion.Year => w => w.Year,
                SearchCriterion.Rating => w => w.Rating,
                _ => w => w.Title
            };

            return order == SortOrder.Ascending
                ? _works.OrderBy(key)
                : _works.OrderByDescending(key);
        }

        public bool CanAccess(LibraryCard card)
        {
            if (card.HasDebt) return false;
            return card.AccessLevel != AccessLevel.Guest;
        }

        public void PrintStatistics()
        {
            Console.WriteLine($"Всего произведений: {_works.Count}");
            foreach (IGrouping<WorkType, Work> group in _works.GroupBy(w => w.Type))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()}, средний рейтинг {group.Average(w => w.Rating):F1}");
            }
            Console.WriteLine($"Средняя цена: {_works.Average(w => w.Price):F2} руб.");
        }
    }

    public static void Run()
    {
        DigitalLibrary library = new DigitalLibrary();
        library.Add(new Work("Война и мир", WorkType.Book, "Лев Толстой", 1869, 4.8, 1200));
        library.Add(new Work("Мастер и Маргарита", WorkType.Book, "Михаил Булгаков", 1967, 4.9, 900));
        library.Add(new Work("Наука и жизнь", WorkType.Magazine, "Редакция", 2024, 4.2, 300));
        library.Add(new Work("C# в действии", WorkType.Audiobook, "Джон Скит", 2020, 4.7, 1500));
        library.Add(new Work("LINQ для начинающих", WorkType.Article, "Анна Смирнова", 2023, 4.5, 0));

        Console.Write("Поиск по автору \"Толстой\": ");
        foreach (Work work in library.Search("Толстой", SearchCriterion.Author))
            Console.WriteLine(work);

        Console.WriteLine("\nСортировка по рейтингу (по убыванию):");
        foreach (Work work in library.Sort(SearchCriterion.Rating, SortOrder.Descending))
            Console.WriteLine($"  {work.Title}: {work.Rating:F1}");

        LibraryCard reader = new LibraryCard("Иван Петров", AccessLevel.Reader, false);
        LibraryCard debtor = new LibraryCard("Петр Сидоров", AccessLevel.Reader, true);

        Console.WriteLine($"\nДоступ для {reader.ReaderName}: {library.CanAccess(reader)}");
        Console.WriteLine($"Доступ для {debtor.ReaderName}: {library.CanAccess(debtor)}");

        Console.WriteLine();
        library.PrintStatistics();
    }
}
