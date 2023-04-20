// Создайте приложение для учёта книг. Необходимо хранить следующую информацию:

// Название книги
// ФИО автора
// Жанр книги
// Год выпуска

//Для хранения сотрудников используйте класс LinkedList<T>.

//Приложение должно предоставлять такую функциональность:

// Добавление книг (по одной, массивом, списком)
// Вывод всего списка книг
// Вывод книги по номеру в списке
// Удаление книг
// Изменение информации о книгах (через индекс)
// Поиск книг по параметрам
// Вставка книги в начало списка
// Вставка книги в конец списка
// Вставка книги в определенную позицию
// Удаление книги из начала списка
// Удаление книги из конца списка
// Удаление книги из определенной позиции
// Вывод всего списка книг
// Вывод книги по номеру в списке

// Создать меню/подменю пользователя
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace _1_Linked_List
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int PublishingYear { get; set; }

        public Book(string title, string author, string genre, int publishingYear)
        {
            Title = title;
            Author = author;
            Genre = genre;
            PublishingYear = publishingYear;
        }
        public Book() { }

        public override string ToString()
        {
            return $"\tTitle: {Title,-20} | " +
                    $"Author: {Author,-15} | " +
                    $"Gere: {Genre,-15} | " +
                    $"Year: {PublishingYear} | ";
        }
    }

    public class Library
    {
        private LinkedList<Book> books = new();

        public LinkedList<Book> Books
        {
            get { return books; }
            set { books = value; }
        }

        public Library(LinkedList<Book> books)
        {
            this.books = books;
        }
        public Library(List<Book> books)
        {
            foreach (var item in books)
            {
                this.books.AddLast(item);
            }
        }
        public Library() { }
        public void Add(Book book)
        {
            books.AddLast(book);
        }
        public void Add(List<Book> listOfbooks)
        {
            foreach (var item in listOfbooks)
            {
                books.AddLast(item);
            }
        }
        public  Library(Book[] massiveOfbooks)
        {
            foreach (var item in massiveOfbooks)
            {
                books.AddLast(item);
            }
        }

        public void SowAll()
        {
            int n = 1;
            foreach (var item in books)
            {
                Console.WriteLine("Book #" + n + "  " + item.ToString());
                n++;
            }
        }

        private LinkedListNode<Book> FindBook(int number)
        {
            LinkedListNode<Book> node = books.First;
            for (int i = 1; i < number; i++)
            {
                node = node.Next;
            }
            return node;
        }

        public void ShowBook(int number)
        {
            LinkedListNode<Book> node = FindBook(number);
            if (books.Count < number || number <= 0 || node == null)
                Console.WriteLine("The book with such a number does not exist!");
            else
                Console.WriteLine(node.Value.ToString());

        }

        public Book this[int index]
        {
            get
            {
                return FindBook(index).Value;
            }
            set
            {
                FindBook(index).Value = value;
            }
        }


        public void AddAfter(int n, Book book)
        {
            books.AddAfter(FindBook(n), book);
        }


        public List<Book> FilterBook((string? title, string? author, string? genre, int? year) f)
        {
            var query = books.Where(x =>
            {

                return x.Title == (f.title ?? x.Title) &&
                       x.Author == (f.author ?? x.Author) &&
                       x.Genre == (f.genre ?? x.Genre) &&
                       x.PublishingYear == (f.year ?? x.PublishingYear);
            });
            return query.ToList();
        }
    }

    internal class Program
    {
        static (string? title, string? author, string? genre, int? year) FilterSettings()
        {
            string? title = null;
            string? author = null;
            string? genre = null;
            int? year = null;
            string? temp;
            Console.WriteLine("--------- Filter params ----------");
            Console.WriteLine("To ignore param just press Enter");
            Console.WriteLine("-----------------------------------");

            Console.Write("Enter Title of the book to search: ");
            temp = Console.ReadLine();
            if (temp != "") title = temp;


            Console.Write("Enter Author of the book to search: ");
            temp = Console.ReadLine();
            if (temp != "") author = temp;

            Console.Write("Enter Genre of the book to search: ");
            temp = Console.ReadLine();
            if (temp != "") genre = temp;

            Console.Write("Enter Publishing year of the book to search: ");
            int y;
            int.TryParse(Console.ReadLine(), out y);
            if (y != 0) year = y;

            return (title, author, genre, year);
        }

        static void Main(string[] args)
        {
            //List<Book> books = new List<Book>();
            //books.Add(new Book("Programming C#", "Bill Gates", "Techno", 1999));
            //books.Add(new Book("Learning C++", "Jack Norton", "Programming", 2000));
            //books.Add(new Book("Programming C#", "Bill Gates", "Techno", 2001));
            //books.Add(new Book("Learning C++", "Jack Norton", "Programming", 2002));
            //books.Add(new Book("Programming C#", "Bill Gates", "Techno", 2003));
            //books.Add(new Book("Learning C++", "Jack Norton", "Programming", 2004));
            //books.Add(new Book("Programming C#", "Bill Gates", "Techno", 2003));
            //books.Add(new Book("Learning C++", "Jack Norton", "Programming", 2006));

            //Library library = new Library(books);

            LinkedList<Book> allBooks;

            FileStream stream = new FileStream("library.json", FileMode.Open);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(LinkedList<Book>));
            allBooks = (LinkedList<Book>)serializer.ReadObject(stream);
            stream.Close();
            Library library = new Library(allBooks);

            //LinkedList<Book> allBooks = library.Books;


            library.SowAll();
            Console.WriteLine();
            Console.Write("Enter the number of the book after to insert new book: ");
            library.AddAfter(int.Parse(Console.ReadLine()), new Book("Learning C", "Gorre James", "Horror", 1990));
            Console.WriteLine();
            library.SowAll();


            allBooks = library.Books;
            stream = new FileStream("library.json", FileMode.Create);
            serializer = new DataContractJsonSerializer(typeof(LinkedList<Book>));
            serializer.WriteObject(stream, allBooks);
            stream.Close();

            //Console.Write("Enter the number of the book: ");
            //int n = int.Parse(Console.ReadLine());
            //library.ShowBook(n);

            //Console.Write("Enter the index of the book: ");
            //n = int.Parse(Console.ReadLine());
            //Console.WriteLine(library[n].ToString());

            //Console.WriteLine("Change book info by index.");
            //Console.Write("Enter the index of the book: ");
            //n = int.Parse(Console.ReadLine());
            //library[n].Author = "Jim Karter";
            //library[n].PublishingYear = 1966;
            //Console.WriteLine(library[n].ToString());

            //Console.WriteLine();
            //List<Book> lst = library.FilterBook(FilterSettings());
            //Console.WriteLine();
            //foreach (var item in lst)
            //{
            //    Console.WriteLine(item.ToString());
            //}
        }
    }


}