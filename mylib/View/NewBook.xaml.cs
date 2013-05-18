using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using mylib.Model;

namespace mylib.View
{
    public partial class PivotPage1 : PhoneApplicationPage
    {
        public PivotPage1()
        {
            InitializeComponent();
            Search search1 = new Search();
            
            search1.DownLoad("commend.php", commendCallBack, "", "", "", "filter_code_1", "filter_request_1", "filter_code_2", "filter_request_2", "filter_code_3", "filter_request_3", "filter_code_4", "filter_request_4", "filter_code_5", "filter_request_5");

        }
        Search.Result searchResult;
        Search.Result commendResult;
        void commendCallBack(Search.Result result)
        {
            commendResult = result;
            foreach (var i in result.book_list)
            {
                TextBlock bookName = new TextBlock();
                bookName.Margin = new Thickness(10, 0, 10, 0);
                bookName.Text = i.tittle;
                bookName.FontSize = 25;
                bookName.TextWrapping = TextWrapping.Wrap;
                bookName.Foreground = new SolidColorBrush(Colors.Black);
                TextBlock publish = new TextBlock();
                publish.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                publish.Text = i.publish;
                publish.Padding = new Thickness(10, 3, 10, 8);
                publish.Foreground = new SolidColorBrush(Colors.Black);
                StackPanel sp = new StackPanel();
                sp.Children.Add(bookName);
                sp.Children.Add(publish);
                sp.Width = 427;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Brown);
                border.BorderThickness = new Thickness(0, 0, 0, 1);
                border.Child = sp;
                commendList.Items.Add(border);
            }
            Search search = new Search();
            search.DownLoad("newbook.php", CallBack, "", "", "", "filter_code_1", "filter_request_1", "filter_code_2", "filter_request_2", "filter_code_3", "filter_request_3", "filter_code_4", "filter_request_4", "filter_code_5", "filter_request_5");
        }

        void CallBack(Search.Result result)
        {
            searchResult = result;
            foreach (var i in result.book_list)
            {
                TextBlock bookName = new TextBlock();
                bookName.Margin = new Thickness(10, 0, 10, 0);
                bookName.Text = i.tittle;
                bookName.FontSize = 25;
                bookName.TextWrapping = TextWrapping.Wrap;
                bookName.Foreground = new SolidColorBrush(Colors.Black);
                TextBlock publish = new TextBlock();
                publish.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                publish.Text = i.publish;
                publish.Padding = new Thickness(10, 3, 10, 8);
                publish.Foreground = new SolidColorBrush(Colors.Black);
                StackPanel sp = new StackPanel();
                sp.Children.Add(bookName);
                sp.Children.Add(publish);
                sp.Width = 427;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Brown);
                border.BorderThickness = new Thickness(0, 0, 0, 1);
                border.Child = sp;
                newList.Items.Add(border);
            }
        }

        private void newList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (newList.SelectedIndex < 0)
            {
                return;
            }
            int index = newList.SelectedIndex;
            string bookName = searchResult.book_list[index].tittle;
            string year = searchResult.book_list[index].year;
            string isbn = searchResult.book_list[index].ISBN;
            string pub = searchResult.book_list[index].publish;
            string searchNumber = searchResult.book_list[index].search_number;
            string author = searchResult.book_list[index].author;
            string sign = searchResult.sign;
            string docNumber = searchResult.book_list[index].doc_number;
            newList.SelectedIndex = -1;

            NavigationService.Navigate(new Uri("/View/BookMessage.xaml?book=" + HttpUtility.UrlEncode(bookName)
                    + "&year=" + year + "&isbn=" + isbn + "&pub=" + pub
                    + "&search=" + searchNumber + "&author=" + author
                    + "&sign=" + sign + "&doc=" + docNumber, UriKind.Relative));
        }

        private void commendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (commendList.SelectedIndex < 0)
            {
                return;
            }
            int index = commendList.SelectedIndex;
            string bookName = commendResult.book_list[index].tittle;
            string year = commendResult.book_list[index].year;
            string isbn = commendResult.book_list[index].ISBN;
            string pub = commendResult.book_list[index].publish;
            string searchNumber = commendResult.book_list[index].search_number;
            string author = commendResult.book_list[index].author;
            string sign = commendResult.sign;
            string docNumber = commendResult.book_list[index].doc_number;
            commendList.SelectedIndex = -1;

            NavigationService.Navigate(new Uri("/View/BookMessage.xaml?book=" + HttpUtility.UrlEncode(bookName)
                    + "&year=" + year + "&isbn=" + isbn + "&pub=" + pub
                    + "&search=" + searchNumber + "&author=" + author
                    + "&sign=" + sign + "&doc=" + docNumber, UriKind.Relative));
        }
    }
}