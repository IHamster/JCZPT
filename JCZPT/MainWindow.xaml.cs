using JCZPT.BoundObjects;
using JCZPT.Const;
using JCZPT.Models;
using JCZPT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using JCZPT.Service;

namespace JCZPT
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑 贾梦
    /// </summary>
    public partial class MainWindow : Window
    {
        private double screenWidth = SystemParameters.PrimaryScreenWidth; //得到屏幕整体宽度;
        private double screenHeight = SystemParameters.PrimaryScreenHeight; //得到屏幕整体高度;
        private Browser leftBrowser = null;
        private Browser rightBrowser = null;
        public MainWindow()
        {            
            this.Width = this.screenWidth ;
            this.Height = this.screenHeight;
            InitializeComponent();

            //InitIndexPage();

            string strmac = PCInfoService.GetMacAddress();
            Console.WriteLine(strmac);
            //ShowMainPage();
        }
        public void InitIndexPage()
        {
            ConfigManageUtil config=ConfigManageUtil.GetInstance();
            config.ReadConfig();
            String lefturl = config.ConfigDic[ConfigConst.LEFTURL];
            if (String.IsNullOrEmpty(lefturl))
            {
                lefturl=ConfigConst.DEFAULTLEFTURL;
                config.ConfigDic[ConfigConst.LEFTURL] = lefturl;
                config.WriteConfig(ConfigConst.LEFTURL, ConfigConst.DEFAULTLEFTURL);
            }
            leftBrowser = new Browser(lefturl);
            UserBound userBound = new UserBound();
            userBound.OnLogOnSuccess += this.OnLogOnSuccess;
            userBound.OnLogOut += this.OnLogOut;
            userBound.OnExit += this.OnExit;
            leftBrowser.RegistBoundObject("UserBound", userBound);
            this.LeftPanel.Children.Add(leftBrowser.MyBrowser);
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void ShowMainPage()
        {
            String screenSize = ConfigManageUtil.GetInstance().ConfigDic[ConfigConst.SCREENSIZE];
            if ("1".Equals(screenSize))
            {
                //做html页面跳转
            }
            if("2".Equals(screenSize))
            {

                this.MinWidth = this.screenWidth * 2;
                this.Width = this.screenWidth * 2;
                this.MaxWidth = this.screenWidth * 2;
                this.rightColumn.Width = new GridLength(1, GridUnitType.Star);
                ////添加一列
                //ColumnDefinition col2=new ColumnDefinition();
                //col2.Width=new GridLength(1,GridUnitType.Star);
                //this.MainGrid.ColumnDefinitions.Add(col2);
                ////添加右屏
                Grid rightGrid = new Grid();
                rightGrid.Name = "RightPanel";
                

                String rightUrl = ConfigManageUtil.GetInstance().ConfigDic[ConfigConst.RIGHTURL];
                if (String.IsNullOrEmpty(rightUrl))
                {
                    rightUrl = ConfigConst.DEFAULTRIGHTURL;
                    ConfigManageUtil.GetInstance().ConfigDic[ConfigConst.RIGHTURL] = rightUrl;
                    ConfigManageUtil.GetInstance().WriteConfig(ConfigConst.RIGHTURL, ConfigConst.DEFAULTRIGHTURL);
                }
                rightBrowser= new Browser(rightUrl);
                UserBound userBound = new UserBound();
                userBound.OnLogOnSuccess += this.OnLogOnSuccess;
                userBound.OnLogOut += this.OnLogOut;
                userBound.OnExit += this.OnExit;
                rightBrowser.RegistBoundObject("UserBound", userBound);
                this.RightPanel.Children.Add(rightBrowser.MyBrowser);
               
            }
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void ReturnLogin()
        {
            String screenSize = ConfigManageUtil.GetInstance().ConfigDic[ConfigConst.SCREENSIZE];
            if ("1".Equals(screenSize))
            {
                //做html页面跳转
            }
            if ("2".Equals(screenSize))
            {
                this.MinWidth = this.screenWidth ;
                this.Width = this.screenWidth ;
                this.MaxWidth = this.screenWidth ;
                this.rightColumn.Width = new GridLength(0);
                this.RightPanel.Children.Clear();
                if (this.rightBrowser!=null)
                {
                    this.rightBrowser.MyBrowser.Dispose();
                    this.rightBrowser = null; 
                }
            }
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void OnLogOnSuccess()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ShowMainPage));

                return;
            }
        }
        private void OnLogOut()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(ReturnLogin));

                return;
            }
        }
        private void OnExit()
        {
            if (this.leftBrowser != null)
            {
                
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(
                    delegate { this.leftBrowser.MyBrowser.Dispose(); 
                        this.leftBrowser = null; }
                    ));
                
            }
            if (this.rightBrowser != null)
            {
                
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(
    delegate { this.rightBrowser.MyBrowser.Dispose(); this.rightBrowser = null; }
    ));
                
            }
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
