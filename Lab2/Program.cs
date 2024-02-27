using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int numberOfBooks = 1000000;
        int sortCount = 1;
        List<Book> books = GenerateRandomBooks(numberOfBooks);

        // Запуск сортування в основному потоці для порівняння
        Stopwatch stopwatch = Stopwatch.StartNew();
        var sortedBooksMain = SortBooks(books, "Author", sortCount++);
        stopwatch.Stop();
        Console.WriteLine($"Сортування в основному потоці: {stopwatch.ElapsedMilliseconds} ms");

        // Запуск з new Task(action) і t.Start()
        stopwatch.Restart();
        var task1 = new Task(() => SortBooks(books, "Author", sortCount++));
        task1.Start();
        task1.Wait();
        stopwatch.Stop();
        Console.WriteLine($"Сортування з Task(action) і Start: {stopwatch.ElapsedMilliseconds} ms");

        // Запуск з Task.Factory.StartNew(action)
        stopwatch.Restart();
        var task2 = Task.Factory.StartNew(() => SortBooks(books, "Author", sortCount++));
        task2.Wait();
        stopwatch.Stop();
        Console.WriteLine($"Сортування з Task.Factory.StartNew: {stopwatch.ElapsedMilliseconds} ms");

        // Синхронне отримання результату
        stopwatch.Restart();
        var taskWithResult = new Task<List<Book>>(() => SortBooks(books, "Author", sortCount++));
        taskWithResult.Start();
        sortedBooksMain = taskWithResult.Result; // Очікує завершення і повертає результат
        stopwatch.Stop();
        Console.WriteLine($"Синхронне отримання результату: {stopwatch.ElapsedMilliseconds} ms");

        // Очікування завершення задач
        Thread.Sleep(5000);
        Console.WriteLine("Затримка на 5 секунд для демонстрації закінчена.");

        // Очікування всіх задач
        var task3 = new Task(() => SortBooks(books, "Author", sortCount++));
        var task4 = new Task(() => SortBooks(books, "Author", sortCount++));
        task3.Start();
        task4.Start();
        Task.WaitAll(task3, task4);
        Console.WriteLine("Всі задачі завершено.");

        // Очікування будь-якої задачі (для демонстрації, всі вже завершені)
        var task5 = new Task(() => SortBooks(books, "Author", sortCount++));
        var slowTask = new Task(() => {
            SortBooks(books, "Author", sortCount++);
            Thread.Sleep(5000);
        });
        task5.Start();
        slowTask.Start();
        int index = Task.WaitAny(task5, slowTask);
        Console.WriteLine($"Задача з індексом {index} завершилася першою. WaitAny закінчено.");
        if (!slowTask.IsCompleted)
            {
                Console.WriteLine($"Повільна задача все ще виконується... Очікуємо її завершення.");
                slowTask.Wait(); // Очікуємо завершення повільної задачі
            }

        Console.WriteLine("Всі задачі завершено.");
    }

    static List<Book> SortBooks(List<Book> books, string sortBy, int count)
    {
        List<Book> sortedList;
        switch (sortBy)
        {
            case "Author":
                sortedList = books.OrderBy(b => b.Author).ToList();
                break;
            case "Title":
                sortedList = books.OrderBy(b => b.Title).ToList();
                break;
            case "Genre":
                sortedList = books.OrderBy(b => b.Genre).ToList();
                break;
            case "PublicationDate":
                sortedList = books.OrderBy(b => b.PublicationDate).ToList();
                break;
            case "Price":
                sortedList = books.OrderBy(b => b.Price).ToList();
                break;
            default:
                throw new ArgumentException("Невідомий критерій сортування", nameof(sortBy));
        }
        Console.WriteLine("Сортування " + count + " завершено.");
        return sortedList;
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
