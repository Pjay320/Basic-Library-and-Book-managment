using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        BookFactory bookFactory = new BookFactory();

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

            switch (choice)
            {
                case "1":
                    AddBook(library, bookFactory);
                    break;
                case "2":
                    RemoveBook(library);
                    break;
                case "3":
                    BorrowBook(library);
                    break;
                case "4":
                    ReturnBook(library);
                    break;
                case "5":
                    SearchBook(library);
                    break;
                case "6":
                    library.DisplayBooks();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddBook(Library library, BookFactory bookFactory)
    {
        Console.Write("Enter book title: ");
        string title = Console.ReadLine();
        Console.Write("Enter book author: ");
        string author = Console.ReadLine();
        Console.Write("Enter book category: ");
        string category = Console.ReadLine();
        Book book = bookFactory.CreateBook(title, author, category);
        library.AddBook(book);
        Console.WriteLine("Book added successfully.");
    }

    static void RemoveBook(Library library)
    {
        Console.Write("Enter book title to remove: ");
        string title = Console.ReadLine();
        Book book = library.SearchBook(title).FirstOrDefault();
        if (book != null)
        {
            library.RemoveBook(book);
            Console.WriteLine("Book removed successfully.");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    static void BorrowBook(Library library)
    {
        Console.Write("Enter user name: ");
        string userName = Console.ReadLine();
        Console.Write("Enter user email: ");
        string userEmail = Console.ReadLine();
        User user = new User { Name = userName, Email = userEmail };

        Console.Write("Enter book title to borrow: ");
        string title = Console.ReadLine();
        Book book = library.SearchBook(title).FirstOrDefault();
        if (book != null)
        {
            library.BorrowBook(book, user);
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    static void ReturnBook(Library library)
    {
        Console.Write("Enter user name: ");
        string userName = Console.ReadLine();
        Console.Write("Enter user email: ");
        string userEmail = Console.ReadLine();
        User user = new User { Name = userName, Email = userEmail };

        Console.Write("Enter book title to return: ");
        string title = Console.ReadLine();
        Book book = library.SearchBook(title).FirstOrDefault();
        if (book != null)
        {
            library.ReturnBook(book, user);
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }

    static void SearchBook(Library library)
    {
        Console.Write("Enter search criteria (title, author, category): ");
        string criteria = Console.ReadLine();
        List<Book> books = library.SearchBook(criteria);
        if (books.Any())
        {
            Console.WriteLine("Books found:");
            foreach (var book in books)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} (Category: {book.Category})");
            }
        }
        else
        {
            Console.WriteLine("No books found.");
        }
    }
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
}

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public interface IBookManagement
{
    void AddBook(Book book);
    void RemoveBook(Book book);
    void BorrowBook(Book book, User user);
    void ReturnBook(Book book, User user);
    List<Book> SearchBook(string criteria);
}

public class Library : IBookManagement
{
    private List<Book> books = new List<Book>();
    private Dictionary<Book, User> borrowedBooks = new Dictionary<Book, User>();

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void RemoveBook(Book book)
    {
        books.Remove(book);
    }

    public void BorrowBook(Book book, User user)
    {
        if (books.Contains(book))
        {
            books.Remove(book);
            borrowedBooks[book] = user;
            Console.WriteLine($"{user.Name} borrowed {book.Title}");
        }
        else
        {
            Console.WriteLine("Book is not available");
        }
    }

    public void ReturnBook(Book book, User user)
    {
        if (borrowedBooks.ContainsKey(book) && borrowedBooks[book] == user)
        {
            books.Add(book);
            borrowedBooks.Remove(book);
            Console.WriteLine($"{user.Name} returned {book.Title}");
        }
        else
        {
            Console.WriteLine($"{user.Name} did not borrow {book.Title}");
        }
    }

    public List<Book> SearchBook(string criteria)
    {
        return books.Where(b => b.Title.Contains(criteria) || b.Author.Contains(criteria) || b.Category.Contains(criteria)).ToList();
    }

    public void DisplayBooks()
    {
        Console.WriteLine("Available books:");
        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title} by {book.Author} (Category: {book.Category})");
        }

        Console.WriteLine("\nBorrowed books:");
        foreach (var entry in borrowedBooks)
        {
            var book = entry.Key;
            var user = entry.Value;
            Console.WriteLine($"- {book.Title} by {book.Author} (Category: {book.Category}) borrowed by {user.Name}");
        }
    }
}

public sealed class DatabaseConnection
{
    private static DatabaseConnection instance = null;

    private DatabaseConnection()
    {
    }

    public static DatabaseConnection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DatabaseConnection();
            }
            return instance;
        }
    }
}

public class BookFactory
{
    public Book CreateBook(string title, string author, string category)
    {
        return new Book { Title = title, Author = author, Category = category };
    }
}
