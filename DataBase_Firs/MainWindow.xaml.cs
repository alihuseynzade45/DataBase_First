using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataBase_Firs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public ObservableCollection<Book> book { get; } = new();



        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }




        private void cmb1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (LibraryContext database = new())
            {
                ComboBoxItem? selectedItem = cmb1.SelectedItem as ComboBoxItem;

                book.Clear();

                if (selectedItem!.Content.ToString() == "Authors")
                {
                    cmb2.Items.Clear();

                    var authors = database.Authors;

                    authors.ToList().ForEach(a => cmb2.Items.Add($@"{a.FirstName} {a.LastName}"));
                }
                else if (selectedItem!.Content.ToString() == "Themes")
                {
                    cmb2.Items.Clear();

                    var themes = database.Themes;

                    themes.ToList().ForEach(t => cmb2.Items.Add($@"{t.Name}"));
                }
                else if (selectedItem!.Content.ToString() == "Categories")
                {
                    cmb2.Items.Clear();

                    var categories = database.Categories;

                    categories.ToList().ForEach(c => cmb2.Items.Add($"{c.Name}"));
                }
            }
        }

        private void cmb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (LibraryContext database = new())
            {
                ComboBoxItem? selectedItem = cmb1.SelectedItem as ComboBoxItem;


                if (selectedItem!.Content.ToString() == "Authors")
                {
                    book.Clear();

                    var selectedAuthor = cmb2.SelectedItem as string;

                    if (selectedAuthor != null)
                    {
                        var authorBooks = database.Books
                            .Join(
                                database.Authors,
                                b => b.IdAuthor,
                                a => a.Id,
                                (b, a) => new { Book = b, Author = a }
                            )
                            .ToList()
                            .Where(x => $"{x.Author.FirstName} {x.Author.LastName}" == selectedAuthor)
                            .Select(x => x.Book)
                            .ToList();

                        authorBooks.ForEach(b => book.Add(b));
                    }
                }
                else if (selectedItem.Content.ToString() == "Themes")
                {
                    var selectedTheme = cmb2.SelectedItem as string;

                    if (selectedTheme != null)
                    {
                        book.Clear();

                        var themeBooks = database.Books
                            .Join(
                                database.Themes,
                                b => b.IdThemes,
                                t => t.Id,
                                (b, t) => new { Book = b, Theme = t }
                            )
                            .ToList()
                            .Where(x => x.Theme.Name == selectedTheme)
                            .Select(x => x.Book)
                            .ToList();

                        themeBooks.ForEach(b => book.Add(b));
                    }
                }
                else if (selectedItem.Content.ToString() == "Categories")
                {
                    var selectedCategory = cmb2.SelectedItem as string;

                    if (selectedCategory != null)
                    {
                        book.Clear();

                        var categoryBooks = database.Books
                            .Join(
                                database.Categories,
                                b => b.IdCategory,
                                c => c.Id,
                                (b, c) => new { Book = b, Category = c }
                            )
                            .ToList()
                            .Where(x => x.Category.Name == selectedCategory)
                            .Select(x => x.Book)
                            .ToList();

                        categoryBooks.ForEach(b => book.Add(b));
                    }
                }

            }
        }
    }
}