using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public interface IBookActions
{
    void Borrow(string borrower);
    void Return();
}
public abstract class LibraryItem
{
    public string Title { get; set; }
    public string Category { get; set; }
}
class BooksData
{
    public List<Book> Books { get; set; }
}

public sealed class Book : LibraryItem, IBookActions
{
    public string Author { get; set; }
    public string isRented { get; set; }

    public void Borrow(string borrower)
    {
        isRented = borrower;
    }

    public void Return()
    {
        isRented = "";
    }

    public override string ToString()
    {
        return $"{Title} ({Category}) by {Author}";
    }
}

class Program
{
    static void Main(string[] args)
    {
       

        while (true)
        {
            Console.WriteLine("\nLibrary Management System");
            Console.WriteLine("1. Add a book");
            Console.WriteLine("2. Remove a book");
            Console.WriteLine("3. Borrow a book");
            Console.WriteLine("4. Return a book");
            Console.WriteLine("5. Search for a book");
            Console.WriteLine("6. Display all books");
            Console.WriteLine("7. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    BorrowBook();
                    break;
                case "4":
                    ReturnBook();
                    break;
                case "5":
                    SearchBook();
                    break;
                case "6":
                    DisplayBooks();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddBook()
    {
        Console.Write("Enter the title of the book: ");
        string title = Console.ReadLine();

        Console.Write("Enter the author of the book: ");
        string author = Console.ReadLine();

        Console.Write("Enter the category of the book: ");
        string category = Console.ReadLine();

        Console.Write("Enter the rental status of the book (leave empty if available): ");
        string isRented = Console.ReadLine();

        Book newBook = new Book
        {
            Title = title,
            Author = author,
            Category = category,
            isRented = isRented
        };

        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        data.Books.Add(newBook);

        string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("../../../books.json", updatedJson);

        Console.WriteLine("Book added successfully.");
    }
    static void RemoveBook()
    {
        Console.Write("Enter the title of the book to remove: ");
        string title = Console.ReadLine();

        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        Book bookToRemove = data.Books.FirstOrDefault(book => book.Title == title);

        if (bookToRemove != null)
        {
            Console.Write("Are you sure you want to remove this book? (yes/no): ");
            string confirmation = Console.ReadLine();

            if (confirmation.ToLower() == "yes")
            {
                data.Books.Remove(bookToRemove);

                string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText("../../../books.json", updatedJson);

                Console.WriteLine("Book removed successfully.");
            }
            else
            {
                Console.WriteLine("Book removal cancelled.");
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }
    static void BorrowBook()
    {
        Console.Write("Enter the title of the book to borrow: ");
        string title = Console.ReadLine();

        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        Book bookToBorrow = data.Books.FirstOrDefault(book => book.Title == title);

        if (bookToBorrow != null)
        {
            if (string.IsNullOrEmpty(bookToBorrow.isRented))
            {
                Console.Write("Enter your name: ");
                string renter = Console.ReadLine();

                bookToBorrow.isRented = renter;

                string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText("../../../books.json", updatedJson);

                Console.WriteLine("Book borrowed successfully.");
            }
            else
            {
                Console.WriteLine("This book is already rented.");
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }
    static void ReturnBook()
    {
        Console.Write("Enter the title of the book to return: ");
        string title = Console.ReadLine();

        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        Book bookToReturn = data.Books.FirstOrDefault(book => book.Title == title);

        if (bookToReturn != null)
        {
            if (!string.IsNullOrEmpty(bookToReturn.isRented))
            {
                bookToReturn.isRented = "";

                string updatedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText("../../../books.json", updatedJson);

                Console.WriteLine("Book returned successfully.");
            }
            else
            {
                Console.WriteLine("This book is not rented.");
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }
    static void SearchBook()
    {
        Console.Write("Enter the title of the book to search: ");
        string title = Console.ReadLine();

        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        Book bookToSearch = data.Books.FirstOrDefault(book => book.Title == title);

        if (bookToSearch != null)
        {
            Console.WriteLine($"Found book: {bookToSearch}");
            if (string.IsNullOrEmpty(bookToSearch.isRented))
            {
                Console.WriteLine("The book is available.");
            }
            else
            {
                Console.WriteLine($"The book is rented by {bookToSearch.isRented}.");
            }
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }
    static void DisplayBooks()
    {
        Console.WriteLine("=========================================");
        string json = File.ReadAllText("../../../books.json");
        BooksData data = JsonConvert.DeserializeObject<BooksData>(json);

        var availableBooks = data.Books.Where(book => string.IsNullOrEmpty(book.isRented)).ToList();
        var rentedBooks = data.Books.Where(book => !string.IsNullOrEmpty(book.isRented)).ToList();

        Console.WriteLine("\nAvailable Books:");
        foreach (Book book in availableBooks)
        {
            Console.WriteLine(book);
        }

        Console.WriteLine("\nRented Books:");
        foreach (Book book in rentedBooks)
        {
            Console.WriteLine($"{book.Title} ({book.Category}) by {book.Author} is Rented by {book.isRented}");
        }
    }
}