using System;

public class Book
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ISBN { get; set; }
    public int Pages { get; set; }
    public string Language { get; set; }
    public decimal Price { get; set; }

    public Book(string author, string title, string genre, DateTime publicationDate, string isbn, int pages, string language, decimal price)
    {
        Author = author;
        Title = title;
        Genre = genre;
        PublicationDate = publicationDate;
        ISBN = isbn;
        Pages = pages;
        Language = language;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Author}, '{Title}', {Genre}, {PublicationDate.ToShortDateString()}, {ISBN}, {Pages}p., {Language}, {Price}$";
    }
}
