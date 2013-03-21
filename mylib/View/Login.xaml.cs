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

namespace mylib.View
{
    public partial class Login : PhoneApplicationPage
    {
        bool isBack = true;
        public Login()
        {
            InitializeComponent();
            isBack = false;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (isBack == true)
                NavigationService.GoBack();
            base.OnNavigatedTo(e);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUser.Text.Trim() == "" || textBoxPasswd.Password.Trim() == "")
                return;
            Model.LoginClass login = new Model.LoginClass(textBoxUser.Text.Trim(), textBoxPasswd.Password.Trim(), CallBack);

            
        }

        private void CallBack(string result)
        {
            if (result.Contains("success"))
            {

                Dispatcher.BeginInvoke(() =>
                {
                    if (radioButtonRemember.IsChecked == true)
                    {
                        string name = result.Split('$')[1];
                        string password = result.Split('$')[2];
                        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                        if (settings.Contains("user"))
                        {
                            settings["user"] = name;
                            settings["password"] = password;
                        }
                        else
                        {
                            settings.Add("user", name);
                            settings.Add("password", password);
                        }
                    }
                    textBlockWrong.Text = "登录成功！";
                    isBack = true;
                    NavigationService.Navigate(new Uri("/View/MyLibrary.xaml", UriKind.Relative));
                });
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    textBlockWrong.Text = "用户名或密码错误！";
                });
            }
        }

        private void textBoxPasswd_GotFocus(object sender, RoutedEventArgs e)
        {
            textBlockWrong.Text = "";
        }

        private void textBoxUser_GotFocus(object sender, RoutedEventArgs e)
        {
            textBlockWrong.Text = "";
        }
    }
}