using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;

namespace BookFinder
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    Library library = new Library();  

    public MainWindow()
    {
      InitializeComponent();
    }

    private void FindBtn_Click(object sender, RoutedEventArgs e)
    {
      if(searchTextBox.Text.Trim().Length > 0)
      {
        library.LoadSearch(searchTextBox.Text.Trim());
        if (library.Books.Count > 0)
        {
          bookListBox.ItemsSource = null; // clear
          bookListBox.ItemsSource = library.Books;
        }
        else
        {
          MessageBox.Show("Can't find book with this name");
        }
      } else
      {
        MessageBox.Show("Please enter something");
      }
    }

    private void ShowPopularBtn_Click(object sender, RoutedEventArgs e)
    {
      library.LoadMostPopular();
      bookListBox.ItemsSource = null; // clear
      bookListBox.ItemsSource = library.Books;
    }

    private void bookListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if(bookListBox.Items.Count > 0)
      {
        bookListBox.Visibility = Visibility.Hidden;
        returnBtn.IsEnabled = true;
        findBtn.IsEnabled = false;
        showPopularBtn.IsEnabled = false;
        searchTextBox.IsEnabled = false;

        outputTextBlock.Text = ((Book)bookListBox.SelectedItem).Text;

        outputScrollViewer.Visibility = Visibility.Visible;
      }
    }

    private void returnBtn_Click(object sender, RoutedEventArgs e)
    {
      bookListBox.Visibility = Visibility.Visible;
      outputScrollViewer.Visibility = Visibility.Hidden;
      returnBtn.IsEnabled = false;
      findBtn.IsEnabled = true;
      showPopularBtn.IsEnabled = true;
      searchTextBox.IsEnabled = true;
      outputTextBlock.Text = "";
    }
  }
}
