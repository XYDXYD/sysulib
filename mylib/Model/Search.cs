using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace mylib.Model
{
    public class Search
    {
        public class Category
        {
            public string doc_number { get; set; }
            public string tittle { get; set; }
            public string ISBN { get; set; }
            public string author { get; set; }
            public string publish { get; set; }
            public string year { get; set; }
            public string search_number { get; set; }
        }
        public class Result
        {
            public string items_total { get; set; }
            public string items_page { get; set; }
            public string sign { get; set; }
            public List<Category> book_list { get; set; }
        }
        Result returnResult;

        public delegate void CallBack(Result result);
        CallBack Back;

        private double beginTime, endTime;
        private string partUrl = "";

        public void DownLoad(string fileName, CallBack back, string request, string find_code, string page_number, string filter_code_1, string filter_request_1, string filter_code_2,
                     string filter_request_2, string filter_code_3, string filter_request_3, string filter_code_4, string filter_request_4,
                     string filter_code_5, string filter_request_5, string local_base = "ZSU01")
        {
            Back = back;
            //获取的路径
            partUrl = fileName + "?" + "request=" + HttpUtility.UrlEncode(request) + "&find_code=" + find_code +
            "&page_number=" + page_number + "&filter_code_1=" + filter_code_1 + "&filter_request_1=" + filter_request_1 + "&filter_code_2=" +
            filter_code_2 + "&filter_request_2=" + filter_request_2 + "&filter_code_3=" + filter_code_3 + "&filter_request_3="
            + filter_request_3 + "&filter_code_4=" + filter_code_4 + "&filter_request_4=" + filter_request_4 + "&filter_code_5=" + filter_code_5
            + "&filter_request_5=" + filter_request_5 + "&local_base=" + local_base;
            LoadData();
        }
        private void LoadData()
        {
            string Urlhead = App.ServerNow() + partUrl;
            Uri url = new Uri(Urlhead);
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.Encoding = Encoding.UTF8;
            client.DownloadStringAsync(url);
            beginTime = DateTime.Now.Ticks;
        }
        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error != null)
            {
                if (App.NetworkAvailable())
                {
                    App._died[App._selectedIndex] = true;
                    LoadData();
                }
                else
                {
                    MessageBox.Show("网络有问题，请稍候重试");
                }
                return;
            }
            endTime = DateTime.Now.Ticks;
            App.SetTime(endTime - beginTime);
            String json = e.Result;

            returnResult = new Result();
            //categoryResults = new List<Category>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(returnResult.GetType());
            returnResult = (Result)ser.ReadObject(ms);
            ms.Close();
            foreach (var i in returnResult.book_list)
            {
                i.author = i.author.Replace("&rsquo;", "'").Replace("&quot;", "\"").Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                i.tittle = i.tittle.Replace("&rsquo;", "'").Replace("&quot;", "\"").Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
                i.publish = i.publish.Replace("&rsquo;", "'").Replace("&quot;", "\"").Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">");
            }
            Back(returnResult);
        }
    }
}
