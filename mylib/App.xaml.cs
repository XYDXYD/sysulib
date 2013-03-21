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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Net.NetworkInformation;

namespace mylib
{
    public partial class App : Application
    {
        static string[] _servers = new string[1];
        static double[] _time = new double[1];
        public static bool[] _died = new bool[1];
        public static int _selectedIndex;
        /// <summary>
        ///提供对电话应用程序的根框架的轻松访问。
        /// </summary>
        /// <returns>电话应用程序的根框架。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application 对象的构造函数。
        /// </summary>
        public App()
        {
            // 未捕获的异常的全局处理程序。 
            UnhandledException += Application_UnhandledException;

            // 标准 Silverlight 初始化
            InitializeComponent();

            // 特定于电话的初始化
            InitializePhoneApplication();

            // 调试时显示图形分析信息。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 显示当前帧速率计数器。
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 显示在每个帧中重绘的应用程序区域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true；

                // 启用非生产分析可视化模式， 
                // 该模式显示递交给 GPU 的包含彩色重叠区的页面区域。
                //Application.Current.Host.Settings.EnableCacheVisualization = true；

                // 通过将应用程序的 PhoneApplicationService 对象的 UserIdleDetectionMode 属性
                // 设置为 Disabled 来禁用应用程序空闲检测。
                //  注意: 仅在调试模式下使用此设置。禁用用户空闲检测的应用程序在用户不使用电话时将继续运行
                // 并且消耗电池电量。
                //PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }
        public static bool NetworkAvailable()
        {
            int dieNumber = 0;
            for (int i = 0; i < _died.Length; i++)
            {
                if (_died[i] == true)
                    dieNumber++;
            }
            return NetworkInterface.GetIsNetworkAvailable()
                && NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None && dieNumber != _died.Length;
        }

        public static string ServerNow()
        {
            /*double min = double.MaxValue;
            
            for (int i = 0; i < _time.Length; i++)
            {
                if (_time[i] < min && _servers[i] != "" && _died[i] == false)
                {
                    min = _time[i];
                    _selectedIndex = i;
                }
            }
            return _servers[_selectedIndex];*/
            return _servers[0];
        }

        public static void SetTime(double newTime)
        {
            if (_time[_selectedIndex] == 0)
                _time[_selectedIndex] = newTime;
            else
                _time[_selectedIndex] = _time[_selectedIndex] / 2 + newTime / 2;
        }

        // 应用程序启动(例如，从“开始”菜单启动)时执行的代码
        // 此代码在重新激活应用程序时不执行
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            _selectedIndex = 0;
            _servers[0] = "http://appdev.sysu.edu.cn/~mad/10389233/lib/";//http://appdev.sysu.edu.cn/~mad/10389226/
           // _servers[1] = "";//http://1.winphonexiang.sinaapp.com/mylib/
           // _servers[2] = "";//http://ftpcxy.xindeke.com/
           // _servers[3] = "";//http://www.czxyh.org/cache/

            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("time0"))
            {
                _time[0] = (double)settings["time0"];
               // _time[1] = (double)settings["time1"];
               // _time[2] = (double)settings["time2"];
               // _time[3] = (double)settings["time3"];
            }
            else
            {
                settings.Add("time0", 0);
               // settings.Add("time1", 0);
               // settings.Add("time2", 0);
               // settings.Add("time3", 0);
                for (int i = 0; i < _time.Length; i++)
                    _time[i] = 0;
            }
            for (int i = 0; i < _died.Length; i++)
                _died[i] = false;
        }

        // 激活应用程序(置于前台)时执行的代码
        // 此代码在首次启动应用程序时不执行
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // 停用应用程序(发送到后台)时执行的代码
        // 此代码在应用程序关闭时不执行
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 应用程序关闭(例如，用户点击“后退”)时执行的代码
        // 此代码在停用应用程序时不执行
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("time0"))
            {
                settings["time0"] = _time[0];
              //  settings["time1"] = _time[1];
              //  settings["time2"] = _time[2];
               // settings["time3"] = _time[3];
            }
        }

        // 导航失败时执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 导航已失败；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }

        // 出现未处理的异常时执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 出现未处理的异常；强行进入调试器
                System.Diagnostics.Debugger.Break();
            }
        }
        public static Dictionary<string, string> uriContainer = new Dictionary<string, string>();
        #region 电话应用程序初始化

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 请勿向此方法中添加任何其他代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建框架但先不将它设置为 RootVisual；这允许初始
            // 屏幕保持活动状态，直到准备呈现应用程序时。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理导航故障
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 确保我们未再次初始化
            phoneApplicationInitialized = true;
        }

        // 请勿向此方法中添加任何其他代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 设置根视觉效果以允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要它
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}