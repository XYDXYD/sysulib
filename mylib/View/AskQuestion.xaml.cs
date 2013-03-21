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
using WindowsPhonePostClient;
using Microsoft.Phone.Controls;

namespace mylib.View
{
    public partial class Collection : PhoneApplicationPage
    {
        public Collection()
        {
            InitializeComponent();
        }

        public void check()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("source","3");
            parameters.Add("language","5");
            parameters.Add("library","10434");
            parameters.Add("qpspmref","");
            parameters.Add("question",HttpUtility.UrlEncode(question.Text));
            parameters.Add("email",HttpUtility.UrlEncode(email.Text));
           
            PostClient proxy = new PostClient(parameters);

            proxy.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null && e.Result != "")
                {
                    MessageBox.Show("提交成功！（我们将在1-3个工作日内回答您的问题）");
                    //MessageBox.Show(e.Result);
                }
                else
                {
                    MessageBox.Show("网络有问题，请稍候重试");
                    
                }
            };
            proxy.DownloadStringAsync(new Uri("http://www.questionpoint.org/crs/servlet/org.oclc.ask.AskPatronQuestion", UriKind.Absolute));
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (question.Text.Trim() != "" && email.Text.Trim() != "")
                check();
            else
                MessageBox.Show("输入不能为空");
        }
    }
}