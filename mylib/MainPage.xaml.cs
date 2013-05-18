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
using System.IO.IsolatedStorage;

namespace mylib
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
        }

        //跳转图书馆
        private void button1_Click(object sender)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("user"))
            {
                progressBar.Visibility = System.Windows.Visibility.Visible;
                Model.LoginClass login = new Model.LoginClass(settings["user"].ToString(), settings["password"].ToString(), LoginCallBack);
            }
            else
            {
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
            }
        }
        private void LoginCallBack(string result)
        {
            Dispatcher.BeginInvoke(() =>
            {
                progressBar.Visibility = System.Windows.Visibility.Collapsed;
                NavigationService.Navigate(new Uri("/View/MyLibrary.xaml", UriKind.Relative));
            });
        }

        //跳转到新书通知
        private void button2_Click(object sender)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/View/NewBook.xaml", UriKind.Relative));
        }

        //跳转到馆员
        private void button3_Click(object sender)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/View/AskQuestion.xaml", UriKind.Relative));
        }

        //跳转到规章制度
        private void button4_Click(object sender)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/View/Rule.xaml", UriKind.Relative));
        }

        //跳转到搜索结果页面
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            string classSelect = "";
            if (listPicker.SelectedItem == keyWord)
                classSelect = "WTI";
            else if (listPicker.SelectedItem == author)
                classSelect = "WAU";
            if (searchBox.Text.Trim() != "")
                NavigationService.Navigate(new Uri("/View/Result.xaml?request=" + HttpUtility.UrlEncode(searchBox.Text.Trim()) + "&find_code=" + classSelect +
                "&page_number=1&filter_code_1=WLN&filter_request_1=&filter_code_2=WYR&filter_request_2=&filter_code_3=WYR&filter_request_3=&filter_code_4=WFM&filter_request_4=&filter_code_5=WSL&filter_request_5=", UriKind.Relative));
        }

        //跳转到高级搜索页面
        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/View/AdvanceSearch.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            progressBar.Visibility = System.Windows.Visibility.Collapsed;
            NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }

        private void listPicker_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            author.Background = new SolidColorBrush(Color.FromArgb(255, 51, 169, 221));
                keyWord.Background = new SolidColorBrush(Color.FromArgb(255, 51, 169, 221));
                keyWord.Foreground = new SolidColorBrush(Colors.White);
                author.Foreground = new SolidColorBrush(Colors.White);
        }

        private void button_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            (sender as Button).Margin = new Thickness((sender as Button).Margin.Left + 4, (sender as Button).Margin.Top + 4, (sender as Button).Margin.Right, (sender as Button).Margin.Bottom );
            e.Handled = true;
        }

        private void button1_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            (sender as Button).Margin = new Thickness((sender as Button).Margin.Left - 4, (sender as Button).Margin.Top - 4, (sender as Button).Margin.Right, (sender as Button).Margin.Bottom);
            button1_Click(sender);
        }


        private void button2_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            (sender as Button).Margin = new Thickness((sender as Button).Margin.Left - 4, (sender as Button).Margin.Top - 4, (sender as Button).Margin.Right, (sender as Button).Margin.Bottom);
            button2_Click(sender);
        }

        private void button4_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            (sender as Button).Margin = new Thickness((sender as Button).Margin.Left - 4, (sender as Button).Margin.Top - 4, (sender as Button).Margin.Right, (sender as Button).Margin.Bottom);
            button4_Click(sender);
        }

        private void button3_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            (sender as Button).Margin = new Thickness((sender as Button).Margin.Left - 4, (sender as Button).Margin.Top - 4, (sender as Button).Margin.Right, (sender as Button).Margin.Bottom);
            button3_Click(sender);
        }

        private void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //判断按键，执行代码 
            {
                this.Focus(); //隐藏虚拟键盘，回到程序界面 
            } 
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}