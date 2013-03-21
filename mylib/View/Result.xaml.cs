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
using mylib.Model;
using Microsoft.Phone.Controls;

namespace mylib.View
{
    public partial class Result : PhoneApplicationPage
    {
        public Result()
        {
            InitializeComponent();
            pageNumber = 1;
        }

        private int pageNumber;
        IDictionary<string, string> queryString;
        Search.Result searchResult;
        string tmpRequest; //保存这个变量的目的：为了防止重新加载页面，在导航到BookMessage的时候取消了queryString["request"]，翻页之前需要加上

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            queryString = this.NavigationContext.QueryString;
            if (queryString.Keys.Contains("request"))
            {
                Search search = new Search();
                if (queryString.Keys.Contains("local_base"))
                    search.DownLoad("book_list.php", CallBack, queryString["request"], queryString["find_code"], pageNumber.ToString(), queryString["filter_code_1"], queryString["filter_request_1"], queryString["filter_code_2"], queryString["filter_request_2"], queryString["filter_code_3"], queryString["filter_request_3"], queryString["filter_code_4"], queryString["filter_request_4"], queryString["filter_code_5"], queryString["filter_request_5"], queryString["local_base"]);
                else
                    search.DownLoad("book_list.php", CallBack, queryString["request"], queryString["find_code"], pageNumber.ToString(), queryString["filter_code_1"], queryString["filter_request_1"], queryString["filter_code_2"], queryString["filter_request_2"], queryString["filter_code_3"], queryString["filter_request_3"], queryString["filter_code_4"], queryString["filter_request_4"], queryString["filter_code_5"], queryString["filter_request_5"]);
            }
            else
            {
                queryString.Add("request", tmpRequest);
            }
            base.OnNavigatedTo(e);
        }



        private void CallBack(Search.Result result)
        {
            if (searchResult == null)
                searchResult = result;
            else
            {
                searchResult.book_list.AddRange(result.book_list);
                listResult.Items.RemoveAt(listResult.Items.Count - 1);
            }

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
                listResult.Items.Add(border);
            }
            if (result.items_page == result.items_total)
            {
            }
            else
            {
                Button nextPage = new Button { HorizontalAlignment = HorizontalAlignment.Stretch, Content = "more..." };
                nextPage.Click += new RoutedEventHandler(buttonNext_Click);
                Border border = new Border();
                nextPage.Width = 437;
                nextPage.Height = 80;
                nextPage.Foreground = new SolidColorBrush(Colors.Gray);
                nextPage.BorderThickness = new Thickness(1, 1, 1, 1);
                nextPage.BorderBrush = new SolidColorBrush(Colors.Gray);
                nextPage.Margin = new Thickness(0, 0, 0, 50);
                nextPage.FontWeight = FontWeights.ExtraBold;
                listResult.Items.Add(nextPage);
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            pageNumber++;
            Search search = new Search();
            if (queryString.Keys.Contains("local_base"))
                search.DownLoad("book_list.php", CallBack, queryString["request"], queryString["find_code"], pageNumber.ToString(), queryString["filter_code_1"], queryString["filter_request_1"], queryString["filter_code_2"], queryString["filter_request_2"], queryString["filter_code_3"], queryString["filter_request_3"], queryString["filter_code_4"], queryString["filter_request_4"], queryString["filter_code_5"], queryString["filter_request_5"], queryString["local_base"]);
            else
                search.DownLoad("book_list.php", CallBack, queryString["request"], queryString["find_code"], pageNumber.ToString(), queryString["filter_code_1"], queryString["filter_request_1"], queryString["filter_code_2"], queryString["filter_request_2"], queryString["filter_code_3"], queryString["filter_request_3"], queryString["filter_code_4"], queryString["filter_request_4"], queryString["filter_code_5"], queryString["filter_request_5"]);
            
            (listResult.Items.ElementAt(listResult.Items.Count - 1) as Button).Content = "loading...";
        }








        //上面是拿数据显示数据
        //下面是操作





        private void listResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listResult.SelectedIndex < 0)
            {
                return;
            }
            int index = listResult.SelectedIndex;
            string bookName = searchResult.book_list[index].tittle;
            string year = searchResult.book_list[index].year;
            string isbn = searchResult.book_list[index].ISBN;
            string pub = searchResult.book_list[index].publish;
            string searchNumber = searchResult.book_list[index].search_number;
            string author = searchResult.book_list[index].author;
            string sign = searchResult.sign;
            string docNumber = searchResult.book_list[index].doc_number;
            listResult.SelectedIndex = -1;

            tmpRequest = NavigationContext.QueryString["request"];//保存在tmpRequest里面，翻页时要用
            this.NavigationContext.QueryString.Remove("request");//去掉request，返回本页时判断如果没这个字段则不重新加载
            NavigationService.Navigate(new Uri("/View/BookMessage.xaml?book=" + HttpUtility.UrlEncode(bookName)
                    + "&year=" + year + "&isbn=" + isbn + "&pub=" + pub 
                    + "&search=" + searchNumber + "&author=" + author
                    + "&sign=" + sign + "&doc=" + docNumber, UriKind.Relative));
        }
















    }
}