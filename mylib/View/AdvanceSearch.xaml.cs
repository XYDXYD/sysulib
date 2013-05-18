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

namespace mylib.View
{
    public partial class AdvanceSearch : PhoneApplicationPage
    {
        public AdvanceSearch()
        {
            InitializeComponent();
            radioButton1.IsChecked = true;
        }

        string ChooseBas = "";
        string ChooseType = "";
        string ChooseLang = "";
        string ChooseLib = "";


        private void selectBasic(object sender, RoutedEventArgs e)
        {
            basic.Opacity = 1;
            if (basic.Visibility == Visibility.Collapsed)
                basic.Visibility = Visibility.Visible;
            else
                basic.Visibility = Visibility.Collapsed;
        }

        private void selectLanguge(object sender, RoutedEventArgs e)
        {
            language.Opacity = 1;
            if (language.Visibility == Visibility.Collapsed)
                language.Visibility = Visibility.Visible;
            else
                language.Visibility = Visibility.Collapsed;
        }

        private void selectType(object sender, RoutedEventArgs e)
        {
            type.Opacity = 1;
            if (type.Visibility == System.Windows.Visibility.Collapsed)
                type.Visibility = System.Windows.Visibility.Visible;
            else
                type.Visibility = Visibility.Collapsed;
        }

        private void selectLib(object sender, RoutedEventArgs e)
        {
            lib_.Opacity = 1;
            if (lib_.Visibility == Visibility.Collapsed)
                lib_.Visibility = Visibility.Visible;
            else
                lib_.Visibility = Visibility.Collapsed;
        }

        void PrintText0(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            Basic.Content = lbi.Content.ToString();
            ChooseBas = lbi.Content.ToString();
            basic.Opacity = 0;
            basic.Visibility = Visibility.Collapsed;
        }
       void PrintText1(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
            //Lange.Content = lbi.Content.ToString();
            ChooseLang = lbi.Content.ToString();
            language.Opacity = 0;
            language.Visibility = Visibility.Collapsed;
        }
       void PrintText2(object sender, SelectionChangedEventArgs args)
       {
           ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
           Type.Content = lbi.Content.ToString();
           ChooseType = lbi.Content.ToString();
           type.Opacity = 0;
           type.Visibility = Visibility.Collapsed;
       }
       void PrintText3(object sender, SelectionChangedEventArgs args)
       {
           ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
           Lib_.Content = lbi.Content.ToString();
           ChooseLib = lbi.Content.ToString();
           lib_.Opacity = 0;
           lib_.Visibility = Visibility.Collapsed;
           
       }




       void button5_Click(object sender, RoutedEventArgs e)
       {
          // progressBar.Visibility = System.Windows.Visibility.Collapsed;
          // string classSelect = "";
          // if (listPicker.SelectedItem == keyWord)
           //    classSelect = "WTI";
          // else if (listPicker.SelectedItem == author)
          //     classSelect = "WAU";

           string classSelect = "";
           if (ChooseBas == "关键字")
               classSelect = "WTI";
           if (ChooseBas == "作者")
               classSelect = "WAU";
           if (ChooseBas == "出版社")
               classSelect = "WPU";
           if (ChooseBas == "ISBN")
               classSelect = "ISP";
           if (ChooseBas == "索取号")
               classSelect = "CAL";
           if (ChooseBas == "系统号")
               classSelect = "SYS";
           if (ChooseBas == "全部")
               classSelect = "WRD";
         
           //if (ChooseBas == "书名") { searchBox.Text = ChooseBas; }

           string languageSelect = "";
           if (ChooseLang == "全部")
               languageSelect = "";
           if (ChooseLang == "中文")
               languageSelect = "CHI";
           if (ChooseLang == "日语")
               languageSelect = "JPN";
           if (ChooseLang == "英文")
               languageSelect = "ENG";
           if (ChooseLang == "法语")
               languageSelect = "FRE";
           if (ChooseLang == "德语")
               languageSelect = "GER";
           if (ChooseLang == "俄语")
               languageSelect = "RUS";

           string typeSelect = "";
           if (ChooseLang == "全部")
               typeSelect = "";
           if (ChooseType == "图书")
               typeSelect = "BK";
           if (ChooseType == "报刊")
               typeSelect = "SE";
           if (ChooseType == "古籍")
               typeSelect = "AB";
           if (ChooseType == "音像资料")
               typeSelect = "OT";
           if (ChooseType == "地图")
               typeSelect = "MU";
           if (ChooseType == "电子资源")
               typeSelect = "ER";


           string libSelect = "";
           if (ChooseLib == "全部")
               libSelect = "";
           if (ChooseLib == "南校中文")
               libSelect = "NXZLT";
           if (ChooseLib == "南校外文")
               libSelect = "NXWLT";
           if (ChooseLib == "南校报刊")
               libSelect = "NXBK";
           if (ChooseLib == "北校流通")
               libSelect = "BXLT";
           if (ChooseLib == "北校图书")
               libSelect = "BXYL";
           if (ChooseLib == "北校报刊")
               libSelect = "BXBK";
           if (ChooseLib == "东校流通")
               libSelect = "DXLT";
           if (ChooseLib == "东校阅览")
               libSelect = "DXYL";
           if (ChooseLib == "东校专藏")
               libSelect = "DXZC";
           if (ChooseLib == "东校法学")
               libSelect = "DXFX";
           if (ChooseLib == "东校地库")
               libSelect = "DXDK";
           if (ChooseLib == "珠海流通")
               libSelect = "ZXLT";
           if (ChooseLib == "珠海阅览")
               libSelect = "ZHYL";
           if (ChooseLib == "经管阅览")
               libSelect = "LNYL";
           if (ChooseLib == "经管流通")
               libSelect = "LNLT";

           if (radioButton1.IsChecked == true)
           {
               if (searchBox.Text.Trim() != "")
                   NavigationService.Navigate(new Uri("/View/Result.xaml?request=" + HttpUtility.UrlEncode(searchBox.Text.Trim()) + "&find_code=" + classSelect +
                 "&page_number=1&filter_code_1=WLN&filter_request_1=" + languageSelect + "&filter_code_2=WYR&filter_request_2=&filter_code_3=WYR&filter_request_3=&filter_code_4=WFM&filter_request_4=" + typeSelect + "&filter_code_5=WSL&filter_request_5=" + libSelect + "&local_base=" + "ZSU01", UriKind.Relative));
           }
           else if (radioButton2.IsChecked == true)
           {
               if (searchBox.Text.Trim() != "")
                   NavigationService.Navigate(new Uri("/View/Result.xaml?request=" + HttpUtility.UrlEncode(searchBox.Text.Trim()) + "&find_code=" + classSelect +
                 "&page_number=1&filter_code_1=WLN&filter_request_1=" + languageSelect + "&filter_code_2=WYR&filter_request_2=&filter_code_3=WYR&filter_request_3=&filter_code_4=WFM&filter_request_4=" + typeSelect + "&filter_code_5=WSL&filter_request_5=" + libSelect + "&local_base=" + "ZSU09", UriKind.Relative));
           }
        }

       private void searchBox_KeyUp(object sender, KeyEventArgs e)
       {
           if (e.Key == Key.Enter) //判断按键，执行代码 
           {
               this.Focus(); //隐藏虚拟键盘，回到程序界面 
           } 
       }
      
    }
}