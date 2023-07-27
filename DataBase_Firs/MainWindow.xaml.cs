using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataBase_Firs
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Book> Books { get; } = new ObservableCollection<Book>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void cmb1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (cmb1.SelectedItem is not ComboBoxItem selectedItem) return;

            cmb2.Items.Clear();

            using (LibraryContext database = new LibraryContext())
            {
                if (selectedItem.Content is "Authors")
                {
                    var authors = database.Authors;
                    foreach (var author in authors)
                    {
                        cmb2.Items.Add($"{author.FirstName} {author.LastName}");
                    }
                }
                else if (selectedItem.Content is "Themes")
                {
                    var themes = database.Themes;
                    foreach (var theme in themes)
                    {
                        cmb2.Items.Add(theme.Name);
                    }
                }
                else if (selectedItem.Content is "Categories")
                {
                    var categories = database.Categories;
                    foreach (var category in categories)
                    {
                        cmb2.Items.Add(category.Name);
                    }
                }
            }
        }

        private void cmb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb1.SelectedItem is not ComboBoxItem selectedItem) return;
            if (cmb2.SelectedItem is not string selectedValue) return;

            Books.Clear();

            using (LibraryContext database = new LibraryContext())
            {
                if (selectedItem.Content is "Authors")
                {
                    var authorBooks = database.Books
                        .Join(database.Authors,
                            book => book.IdAuthor,
                            author => author.Id,
                            (book, author) => new { Book = book, Author = author })
                        .Where(x => x.Author.FirstName + " " + x.Author.LastName == selectedValue)
                        .Select(x => x.Book)
                        .ToList();

                    foreach (var book in authorBooks)
                    {
                        Books.Add(book);
                    }
                }
                else if (selectedItem.Content is "Themes")
                {
                    var themeBooks = database.Books
                        .Join(database.Themes,
                            book => book.IdThemes,
                            theme => theme.Id,
                            (book, theme) => new { Book = book, Theme = theme })
                        .Where(x => x.Theme.Name == selectedValue)
                        .Select(x => x.Book)
                        .ToList();

                    foreach (var book in themeBooks)
                    {
                        Books.Add(book);
                    }
                }
                else if (selectedItem.Content is "Categories")
                {
                    var categoryBooks = database.Books
                        .Join(database.Categories,
                            book => book.IdCategory,
                            category => category.Id,
                            (book, category) => new { Book = book, Category = category })
                        .Where(x => x.Category.Name == selectedValue)
                        .Select(x => x.Book)
                        .ToList();

                    foreach (var book in categoryBooks)
                    {
                        Books.Add(book);
                    }
                }
            }
        }


    }
}
