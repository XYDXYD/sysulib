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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace mylib.View
{
    public partial class BookMessage : PhoneApplicationPage
    {
        public BookMessage()
        {
            InitializeComponent();
        }
        string summary = "";
        private double beginTime, endTime;
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            IDictionary<string, string> queryString = this.NavigationContext.QueryString;
            bookTxt.Text = queryString["book"] + " (" + queryString["year"] + ")";
            isbnTxt.Text = queryString["isbn"];
            authorTxt.Text = queryString["author"];
            publishTxt.Text = queryString["pub"];
            searchNumTxt.Text = queryString["search"];

            string doc = queryString["doc"];
            string sign = queryString["sign"];
            string url = App.ServerNow() + "status.php?sign=" + sign + "&doc_number=" + doc + "&ISBN=" + isbnTxt.Text.Trim();
            WebClient BMessageClient = new WebClient();
            BMessageClient.Encoding = Encoding.UTF8;
            BMessageClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(BMessageClient_DownloadStringCompleted);
            beginTime = DateTime.Now.Ticks;
            BMessageClient.DownloadStringAsync(new Uri(url));

            Uri uri = new Uri("http://202.112.150.126/index.php?client=libcode&isbn=" + isbnTxt.Text + "/cover", UriKind.Absolute);

            ImageSource imgSource = new BitmapImage(uri);

            bookCover.Source = imgSource;

        }

        void BMessageClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // throw new NotImplementedException();
            if (e.Error != null)
            {
                if (App.NetworkAvailable())
                {
                    App._died[App._selectedIndex] = true;
                    LoadData();
                }
                else
                    MessageBox.Show("网络有问题，请稍候重试");
            }
            else
            {
                endTime = DateTime.Now.Ticks;
                App.SetTime(endTime - beginTime);
                string result = e.Result;
                if (result == "")
                {
                    if (App.NetworkAvailable())
                    {
                        App._died[App._selectedIndex] = true;
                        LoadData();
                    }
                    else
                        MessageBox.Show("网络有问题，请稍候重试");
                }
                else
                {
                    result = result.Replace('\\', ' ');
                    JObject book_message = JObject.Parse(result);
                    summary = book_message["summary"].ToString();

                    if (summary == "")
                        textblock1.Text = "暂无";
                    else
                        textblock1.Text = summary;
                    textblock1.FontSize = 23;

                    if (book_message["status_list"].ToString() == "[]")
                    {
                        /*if (App.NetworkAvailable())
                        {
                            App._died[App._selectedIndex] = true;
                            LoadData();
                        }
                        else
                            MessageBox.Show("网络有问题，请稍候重试");*/
                        TextBlock tb = new TextBlock();
                        tb.Text = "暂无馆藏数据";
                        tb.Foreground = new SolidColorBrush(Colors.Black);
                        BookStatus.Items.Add(tb);
                    }
                    else
                    {
                        int Record = 1;
                        foreach (var i in book_message["status_list"])
                        {
                            TextBlock txtRecord = new TextBlock();
                            txtRecord.Padding = new Thickness(10, 0, 10, 0);
                            txtRecord.Text = "(第" + (Record) + "条记录)";
                            txtRecord.Foreground = new SolidColorBrush(Colors.Black);

                            string tmp1 = i["borrow_state"].ToString();
                            TextBlock txtBorrow = new TextBlock();
                            txtBorrow.Padding = new Thickness(10, 0, 10, 0);
                            txtBorrow.Text = "借阅状态：" + tmp1;
                            txtBorrow.Foreground = txtRecord.Foreground;

                            string tmp2 = i["return_time"].ToString();
                            TextBlock txtTime = new TextBlock();
                            txtTime.Padding = new Thickness(10, 0, 10, 0);
                            txtTime.Text = "归还日期：" + tmp2;
                            txtTime.Foreground = txtRecord.Foreground;

                            string tmp3 = i["sub_library"].ToString();
                            TextBlock txtLib = new TextBlock();
                            txtLib.Padding = new Thickness(10, 0, 10, 0);
                            txtLib.Text = "分馆：" + tmp3;
                            txtLib.Foreground = txtRecord.Foreground;

                            string tmp4 = i["sub_library_position"].ToString();
                            TextBlock txtLibPos = new TextBlock();
                            txtLibPos.Padding = new Thickness(10, 0, 10, 0);
                            txtLibPos.Text = "图书位置：" + tmp4;
                            txtLibPos.Foreground = txtRecord.Foreground;

                            string tmp5 = i["line_number"].ToString();
                            TextBlock txtLineNum = new TextBlock();
                            txtLineNum.Padding = new Thickness(10, 0, 10, 0);
                            txtLineNum.Text = "条形码：" + tmp5;
                            txtLineNum.Foreground = txtRecord.Foreground;

                            Button button = new Button();
                            button.Content = "预约";
                            button.Background = new SolidColorBrush(Color.FromArgb(255, 51, 169, 221));


                            StackPanel sp = new StackPanel();
                            sp.Width = 437;
                            sp.Children.Add(txtRecord);
                            sp.Children.Add(txtBorrow);
                            sp.Children.Add(txtTime);
                            sp.Children.Add(txtLib);
                            sp.Children.Add(txtLibPos);
                            sp.Children.Add(txtLineNum);

                            Border border = new Border();
                            border.BorderBrush = new SolidColorBrush(Colors.Brown);
                            border.BorderThickness = new Thickness(0, 0, 0, 1);
                            border.Child = sp;

                            BookStatus.Items.Add(border);
                            Record++;
                        }
                    }
                }
            }
        }
    }
}