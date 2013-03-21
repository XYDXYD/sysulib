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
using System.Text;
using System.Text.RegularExpressions;

namespace mylib.View
{
    public partial class MyLibrary : PhoneApplicationPage
    {
        public MyLibrary()
        {
            InitializeComponent();
        }

        string xujieUri = "";
        string xujieallUri = "";
        List<string> xujieKeys = new List<string>();
        List<string> authors = new List<string>();
        List<string> items = new List<string>();
        List<string> years = new List<string>();
        List<string> returnTime = new List<string>();

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            ListBook.Items.Clear();
            ListHistory.Items.Clear();
            string url = App.uriContainer["borrowed"];
            WebClient borrowedClient = new WebClient();
            borrowedClient.Encoding = Encoding.UTF8;
            borrowedClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(borrowedClient_DownloadStringCompleted);
            borrowedClient.DownloadStringAsync(new Uri(url));
        }

        void borrowedClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error != null)
            {
                MessageBox.Show("网络连接错误！");
            }
            else
            {
                string result = e.Result;
                //获取归还时间列表
                Regex time = new Regex("<td class=td1 valign=top width=\"10%\">(.*)</td>");
                MatchCollection time1 = time.Matches(result);
                returnTime.Clear();
                
                foreach (Match i in time1)
                {
                    if (!i.Value.Contains("<br>"))
                        returnTime.Add(i.Value.Replace("<td class=td1 valign=top width=\"10%\">", "").Replace("</td>", "").Replace("有效应还日期: ", "").Replace(" 。","").Insert(4, "-").Insert(7, "-"));
                }

                //先获取书名列表
                Regex book = new Regex("target=_blank>(.*)</a></td>");
                MatchCollection book1 = book.Matches(result);
                items.Clear();
                for (int i = 0; i < book1.Count; i++ )
                {
                    string tmp = book1[i].Value.Replace("target=_blank>", "").Replace("</a></td>", "");
                    items.Add(tmp);
                    TextBlock txt = new TextBlock();
                    txt.Padding = new Thickness(10, 0, 10, 0);
                    txt.Text = tmp;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.Foreground = pivot.Foreground;
                    TextBlock txtTime = new TextBlock();
                    txtTime.Padding = new Thickness(10, 3, 8, 10);
                    txtTime.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtTime.Text = "归还期限：" + returnTime[i];
                    txtTime.Foreground = pivot.Foreground;
                    StackPanel sp = new StackPanel();
                    sp.Width = 437;
                    sp.Children.Add(txt);
                    sp.Children.Add(txtTime);

                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(Colors.Brown);
                    border.BorderThickness = new Thickness(0, 0, 0, 1);
                    border.Child = sp;
                    
                    ListBook.Items.Add(border);
                }


                //处理已借书目信息之前开一个线程去下载借阅历史
                string url = App.uriContainer["history"];
                WebClient historyClient = new WebClient();
                historyClient.Encoding = Encoding.UTF8;
                historyClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(historyClient_DownloadStringCompleted);
                historyClient.DownloadStringAsync(new Uri(url));

                //如果书名列表为空，不执行下面步骤
                if (items.Count == 0)
                {
                    textBlockNote.Text = "当前没有未归还的书";
                    xujieButton.IsEnabled = false;
                    return;
                }

                //续借全部网址
                //http://202.116.64.108:8991/F/Y5KY28DS3LUTHLFYVMYALNSGMAISDA1524P3PBC6U21VD7Y143-33514?func=bor-renew-all&adm_library=ZSU50
                Regex xujieall = new Regex("http://202.116.64.108:8991/F/(.*)?func=bor-renew-all&adm_library=ZSU50");
                Match xujieall1 = xujieall.Match(result);
                if (xujieall1.Success)
                {
                    string res = xujieall1.Value;
                    xujieallUri = res;
                }

                //续借网址
                Regex xujie = new Regex("var strData = \"http://202.116.64.108:8991/F/(.*)?func=bor-renew-all&renew_selected=Y&adm_library=ZSU50");
                Match xujie1 = xujie.Match(result);
                if (xujie1.Success)
                {
                    string res = xujie1.Value;
                    res = res.Replace("var strData = \"", "");
                    xujieUri = res;
                }
                //获取部分续借参数
                Regex key = new Regex("<input type=\"checkbox\" name=\"(.*)\"></td>");
                MatchCollection key1 = key.Matches(result);
                xujieKeys.Clear();
                foreach (Match i in key1)
                {
                    xujieKeys.Add(i.Value.Replace("<input type=\"checkbox\" name=\"", "").Replace("\"></td>", ""));
                }
                //获取作者列表
                Regex author = new Regex("<td class=td1 valign=top width=\"8%\" >(.*)</td>");
                MatchCollection author1 = author.Matches(result);
                authors.Clear();
                foreach (Match i in author1)
                {
                    authors.Add(i.Value.Replace("<td class=td1 valign=top width=\"8%\" >", "").Replace("</td>", ""));
                }
                //获取出版年份列表
                Regex year = new Regex("<td class=td1 valign=top width=\"6%\" >([0-9]*)</td>");
                MatchCollection year1 = year.Matches(result);
                years.Clear();
                foreach (Match i in year1)
                {
                    years.Add(i.Value.Replace("<td class=td1 valign=top width=\"6%\" >", "").Replace("</td>", ""));
                }
                
            }
        }

        void historyClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error != null)
            {
                MessageBox.Show("网络连接错误，借阅历史获取失败。");
            }
            else
            {
                string result = e.Result;
                List<string> books = new List<string>();
                List<string> timeDaoqi = new List<string>();
                List<string> timeGuihuan = new List<string>();

                Regex times = new Regex("<td class=td1 valign=top>[0-9]{8}</td>");
                MatchCollection times1 = times.Matches(result);
                for (int i = 0; i < times1.Count; i += 2)
                {
                    timeDaoqi.Add(times1[i].Value.Replace("<td class=td1 valign=top>", "").Replace("</td>", "").Insert(4, "-").Insert(7, "-"));
                    timeGuihuan.Add(times1[i + 1].Value.Replace("<td class=td1 valign=top>", "").Replace("</td>", "").Insert(4, "-").Insert(7, "-"));
                }

                Regex items = new Regex("target=_blank>(.*)</a></td>");
                MatchCollection items1 = items.Matches(result);
                for (int i = 1; i < items1.Count; i += 2)
                {
                    books.Add(items1[i].Value.Remove(0, 14).Replace("</a></td>", ""));
                    TextBlock txt = new TextBlock();
                    txt.Padding = new Thickness(10, 0, 10, 0);
                    txt.Text = books[i/2];
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.Foreground = pivot.Foreground;
                    TextBlock txtTime = new TextBlock();
                    txtTime.Padding = new Thickness(10, 3, 8, 10);
                    txtTime.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtTime.Text = "归还期限：" + timeDaoqi[i/2] + "    " + "归还时间：" + timeGuihuan[i/2];
                    txtTime.Foreground = pivot.Foreground;
                    StackPanel sp = new StackPanel();
                    sp.Width = 437;
                    sp.Children.Add(txt);
                    sp.Children.Add(txtTime);

                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(Colors.Brown);
                    border.BorderThickness = new Thickness(0, 0, 0, 1);
                    border.Child = sp;

                    ListHistory.Items.Add(border);
                }
            }
        }


        private void ListBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBook.SelectedIndex == -1)
                return;
            int index = ListBook.SelectedIndex;
            gridDetail.Visibility = Visibility.Visible;
            textBlockBook1.Text = items[index];
            textBlockPubDate1.Text = years[index];
            textBlockReturn1.Text = returnTime[index];
            textBlockAuthor1.Text = authors[index];
        }


        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonClose_Click_1(object sender, RoutedEventArgs e)
        {

            ListBook.SelectedIndex = -1;
            gridDetail.Visibility = Visibility.Collapsed;
        }

        private void buttonXujie_Click(object sender, RoutedEventArgs e)
        {
            string uri = xujieUri + "&" + xujieKeys[items.IndexOf(textBlockBook1.Text)] + "=Y";
            WebClient xujieClient = new WebClient();
            xujieClient.Encoding = Encoding.UTF8;
            xujieClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(xujieClient_DownloadStringCompleted);
            xujieClient.DownloadStringAsync(new Uri(uri));
        }

        void xujieClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("网络连接错误，续借失败!");
            }
            else
            {
                string result = e.Result;
                if (result.IndexOf("不成功") == -1)
                {
                    MessageBox.Show("续借成功！");
                }
                else
                {
                    MessageBox.Show("续借失败！");
                }

                ListBook.SelectedIndex = -1;
                gridDetail.Visibility = Visibility.Collapsed;
                LoadData();
            }
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            pivot.SelectedItem = historyPivot;
        }

        private void xujieButton_Click(object sender, RoutedEventArgs e)
        {
            WebClient wcXujieAll = new WebClient();
            wcXujieAll.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wcXujieAll_DownloadStringCompleted);
            wcXujieAll.DownloadStringAsync(new Uri(xujieallUri));
        }

        void wcXujieAll_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("网络故障，续借失败");
            }
            else
            {
                if (e.Result.IndexOf("续借成功") == -1)
                {
                    MessageBox.Show("全部续借失败");
                }
                else if (e.Result.IndexOf("不成功") == -1)
                {
                    MessageBox.Show("续借成功");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("部分续借失败");
                    LoadData();
                }
            }
        }


    }
}