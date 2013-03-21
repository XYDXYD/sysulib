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
using WindowsPhonePostClient;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Threading;

namespace mylib.Model
{
    public class LoginClass
    {
        public delegate void Callback(string result);
        private string user;
        private string password;
        private Callback returnResult;

        public LoginClass(string userName, string passwd, Callback back)
        {
            App.uriContainer.Clear();
            user = userName;
            password = passwd;
            check(CheckCallback);
            returnResult = back;
        }

        public void CheckCallback(string result)
        {
            //action="http://202.116.64.108:8991/F/MPNAQJNPS2HG3SFQ69BAGL7K7Y8U2FE225P14ACQTMK8IKH86X-13702" > 
            Regex hash = new Regex("action=\"http://202.116.64.108:8991/F/(.*)\">");
            Match hashValue = hash.Match(result);
            if (hashValue.Success)
            {
                string res =  hashValue.Value;
                res = res.Replace("action=\"", "");
                res = res.Replace("\">", "");
                //resultBlock.Text = res;

                //现在已经获得了哈希值，然后开始登录
                Login(res, user, password, LoginCallback);
            }
        }

        public void LoginCallback(string result)
        {
            if (result == "用户名或密码错误")
            {
                returnResult(result);
                return;
            }

            //MessageBox.Show(result);

            Regex borrowed = new Regex("<td class=td1><a href=\"(.*)func=bor-loan(.*);\">");
            Match borrow = borrowed.Match(result);
            if (borrow.Success)
            {
                string res = borrow.Value;
                res = res.Replace("<td class=td1><a href=\"javascript:replacePage('", "");
                res = res.Replace("');\">", "");
                App.uriContainer.Add("borrowed", res);
            }

            Regex history = new Regex("<td class=td1><a href=\"(.*)func=bor-history-loan(.*);\">");
            Match history1 = history.Match(result);
            if (history1.Success)
            {
                string res = history1.Value;
                res = res.Replace("<td class=td1><a href=\"javascript:replacePage('", "");
                res = res.Replace("');\">", "");
                App.uriContainer.Add("history", res);
            }

            returnResult("success$" + user + "$" + password);
        }

        public void check(Callback back)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("func", "login-session");
            parameters.Add("login_source", "bor-info");
            parameters.Add("bor_library", "ZSU50");
            parameters.Add("bor_id", "");
            parameters.Add("bor_verification", "");
            PostClient proxy = new PostClient(parameters);

            proxy.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null && e.Result != "")
                {
                    back(e.Result);
                }
                else
                {
                    MessageBox.Show("网络有问题，请稍候重试");
                }
            };
            proxy.DownloadStringAsync(new Uri("http://202.116.64.108:8991/F/-", UriKind.Absolute));

        }



        private void Login(string uri, string userName, string password, Callback back)
        {
            //构造post的参数
            string postString = "func=login-session&bor_library=ZSU50&login_source=bor-info&bor_id=" + userName + "&bor_verification=" + password;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uri);//创建对url的请求
            req.Accept = "text/html, application/xhtml+xml, */*";//接受任意文件
            //模拟网页登录模式
            req.UserAgent = " Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727)"; // 模拟使用IE在浏览
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.8.1.16) Gecko/20080702 Firefox/2.0.0.16";
            //req.CookieContainer = App.cookieContainer; // 取得相应cookie
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";//窗体数据被编码为名称/值对。这是标准的编码格式

            if (postString != null & postString.Length > 0)
            {
                //处理post信息，转化为二进制状态
                byte[] postdata = Encoding.UTF8.GetBytes(postString);
                req.BeginGetRequestStream(async1 =>
                {
                    using (Stream stream = req.EndGetRequestStream(async1))
                        stream.Write(postdata, 0, postdata.Length);
                    req.BeginGetResponse(async2 =>
                    {
                        //处理回流信息
                        WebResponse rep = req.EndGetResponse(async2);
                        string [] a = rep.Headers.AllKeys;
                        using (Stream stream = rep.GetResponseStream())
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string result = sr.ReadToEnd();
                            if (result.IndexOf("证号或密码错误。") != -1)
                            {
                                back("用户名或密码错误");
                                return;
                            }
                            else if (result.IndexOf("，您好！") == -1) //直接进入了下一页，不用进一步
                            {
                                back(result);
                                return;
                            }
                            string postUri = "";
                            Dictionary<string, string> para = new Dictionary<string,string>();
                            //获取下一步的网址
                            Regex hash = new Regex("action=\"http://202.116.64.108:8991/F/(.*)\" style=\"margin:0 25%\">");
                            Match hashValue = hash.Match(result);
                            if (hashValue.Success)
                            {
                                postUri = hashValue.Value;
                                postUri = postUri.Replace("action=\"", "");
                                postUri = postUri.Replace("\" style=\"margin:0 25%\">", "");
                            }
                            //获取参数
                            Regex input = new Regex("<input type=\"hidden\" name=\"(.*)\">");
                            MatchCollection inputs = input.Matches(result);
                            foreach (Match i in inputs)
                            {
                                string tmp = i.Value;
                                string name = "";
                                string value = "";
                                tmp = tmp.Replace("<input type=\"hidden\" name=\"", "");
                                for (int j = 0; tmp[j] != '"'; j++)
                                {
                                    name += tmp[j];
                                }
                                int jValue = tmp.IndexOf("value=\"");
                                jValue += 7;
                                for (int j = jValue; tmp[j] != '"'; j++)
                                {
                                    value += tmp[j];
                                }
                                if (name != "" || value != "")
                                    para.Add(name, value);
                            }

                            //现在完成了登录，点击确定进入下一页，获取信息
                            Request(postUri, para, back);
                        }
                    }, null);
                }, null);
            }
        }

        private void Request(string postUri, Dictionary<string, string> para, Callback back)
        {
            //构造post的参数
            string postString = "";
            foreach (var i in para)
            {
                postString += i.Key;
                postString += "=";
                postString += i.Value;
                postString += "&";
            }
            postString = postString.Remove(postString.Length - 1);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(postUri);//创建对url的请求
            req.Accept = "text/html, application/xhtml+xml, */*";//接受任意文件
            //模拟网页登录模式
            req.UserAgent = " Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727)"; // 模拟使用IE在浏览
            req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.8.1.16) Gecko/20080702 Firefox/2.0.0.16";
            //req.CookieContainer = App.cookieContainer; // 取得相应cookie
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";//窗体数据被编码为名称/值对。这是标准的编码格式

            if (postString != null & postString.Length > 0)
            {
                //处理post信息，转化为二进制状态
                byte[] postdata = Encoding.UTF8.GetBytes(postString);
                req.BeginGetRequestStream(async1 =>
                {
                    using (Stream stream = req.EndGetRequestStream(async1))
                        stream.Write(postdata, 0, postdata.Length);
                    req.BeginGetResponse(async2 =>
                    {
                        //处理回流信息
                        WebResponse rep = req.EndGetResponse(async2);
                        string[] a = rep.Headers.AllKeys;
                        using (Stream stream = rep.GetResponseStream())
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string result = sr.ReadToEnd();
                            back(result);
                        }
                    }, null);
                }, null);
            }
        }
    }
}
