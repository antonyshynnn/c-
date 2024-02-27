using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int numberOfBooks = 100000;
        List<Book> books = GenerateRandomBooks(numberOfBooks);

        // Запуск сортування в основному потоці
        Stopwatch stopwatch = Stopwatch.StartNew();
        var sortedBooks = SortBooks(books, "Author");
        stopwatch.Stop();
        Console.WriteLine($"Сортування в основному потоці: {stopwatch.ElapsedMilliseconds} ms");

        // Запуск сортування в новому потоці
        stopwatch.Restart();
        Thread sortThread = new Thread(() => SortBooks(books, "Author"));
        sortThread.Start();
        sortThread.Join();
        stopwatch.Stop();
        Console.WriteLine($"Сортування в додатковому потоці: {stopwatch.ElapsedMilliseconds} ms");
    }

    static List<Book> SortBooks(List<Book> books, string sortBy)
    {
        switch (sortBy)
        {
            case "Author":
                return books.OrderBy(b => b.Author).ToList();
            case "Title":
                return books.OrderBy(b => b.Title).ToList();
            case "Genre":
                return books.OrderBy(b => b.Genre).ToList();
            case "PublicationDate":
                return books.OrderBy(b => b.PublicationDate).ToList();
            case "Price":
                return books.OrderBy(b => b.Price).ToList();
            default:
                throw new ArgumentException("Невідомий критерій сортування", nameof(sortBy));
        }
    }

    static List<Book> GenerateRandomBooks(int count) 
    {
        List<Book> books = new List<Book>();
        Random random = new Random();

        for (int i = 0; i < count; i++)
        {
            string author = $"Author{random.Next(100000, 1000000)}";
            string title = $"Title{random.Next(100000, 1000000)}";
            string genre = $"Genre{random.Next(100000, 1000000)}";
            string language = $"Language{random.Next(100000, 1000000)}";
            books.Add(new Book(
                author,
                title,
                genre,
                new DateTime(random.Next(1900, 2021), random.Next(1, 13), random.Next(1, 29)),
                $"ISBN {random.Next(100000, 1000000)}-{random.Next(100000, 1000000)}",
                random.Next(100, 1001),
                language,
                (decimal)(random.NextDouble() * (100 - 5) + 5)
            ));
        }

        return books;
    }

}
