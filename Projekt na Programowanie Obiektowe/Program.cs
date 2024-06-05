class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        User user = new User { Name = "John", Email = "john@example.com" };
        BookFactory bookFactory = new BookFactory();
        Book book = bookFactory.CreateBook("Title", "Author", "Category");

        library.AddBook(book);
        library.BorrowBook(book, user);
        library.ReturnBook(book, user);
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
    private List<User> users = new List<User>();

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
            Console.WriteLine($"{user.Name} borrowed {book.Title}");
        }
        else
        {
            Console.WriteLine("Book is not available");
        }
    }

    public void ReturnBook(Book book, User user)
    {
        books.Add(book);
        Console.WriteLine($"{user.Name} returned {book.Title}");
    }

    public List<Book> SearchBook(string criteria)
    {
        return books.Where(b => b.Title.Contains(criteria) || b.Author.Contains(criteria) || b.Category.Contains(criteria)).ToList();
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