using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCZPT.Models
{
    public class Browser
    {
        private ChromiumWebBrowser myBrowser = null;

        public ChromiumWebBrowser MyBrowser
        {
            get { return myBrowser; }
            set { myBrowser = value; }
        }
        public Browser()
        {
        }
        public Browser(String startHtmlUrl)
        {

            try
            {

                if (!Cef.IsInitialized)
                {
                    var setting = new CefSettings();
                    setting.Locale = "zh-Cn";
                    setting.CefCommandLineArgs.Add("disable-gpu", "1");
                    setting.LogSeverity = LogSeverity.Disable;
                    setting.PersistSessionCookies = true;
                    setting.CachePath = AppDomain.CurrentDomain.BaseDirectory + @"\Cache";
                    Cef.Initialize(setting, true, false);
                }

                myBrowser = new ChromiumWebBrowser();
                myBrowser.BrowserSettings = new BrowserSettings();
                myBrowser.BrowserSettings.WebSecurity = CefState.Disabled;
                myBrowser.Address = new StringBuilder(AppDomain.CurrentDomain.BaseDirectory).Append(startHtmlUrl).ToString();
                myBrowser.PreviewTextInput += (o, e) =>
                {
                    foreach (var character in e.Text)
                    {
                        // 把每个字符向浏览器组件发送一遍
                        myBrowser.GetBrowser().GetHost().SendKeyEvent((int)WM.CHAR, (int)character, 0);
                    }
                    // 不让cef自己处理
                    e.Handled = true;
                };
                myBrowser.MenuHandler = new MenuHandler();
                myBrowser.DownloadHandler = new DownloadHandler();
            }
            catch (Exception ex)
            {

                //LOG.Error(ex);

            }
        }
        /// <summary>
        /// 注册bound方法
        /// </summary>
        /// <param name="boundName">bound类字符串</param>
        /// <param name="boundObject">bound方法实例</param>
        public void RegistBoundObject(String boundName, Object boundObject)
        {
            if (this.MyBrowser != null)
            {
                this.MyBrowser.RegisterAsyncJsObject(boundName, boundObject, false);
            }
        }
        /// <summary>
        /// cef菜单事件
        /// </summary>
        public class MenuHandler : CefSharp.IContextMenuHandler
        {

            void CefSharp.IContextMenuHandler.OnBeforeContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model)
            {
                model.Clear();
            }

            bool CefSharp.IContextMenuHandler.OnContextMenuCommand(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.CefMenuCommand commandId, CefSharp.CefEventFlags eventFlags)
            {
                //throw new NotImplementedException();
                return false;
            }

            void CefSharp.IContextMenuHandler.OnContextMenuDismissed(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame)
            {
                //throw new NotImplementedException();
            }

            bool CefSharp.IContextMenuHandler.RunContextMenu(CefSharp.IWebBrowser browserControl, CefSharp.IBrowser browser, CefSharp.IFrame frame, CefSharp.IContextMenuParams parameters, CefSharp.IMenuModel model, CefSharp.IRunContextMenuCallback callback)
            {
                return false;
            }
        }
        internal class DownloadHandler : IDownloadHandler
        {


            public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
            {
                throw new NotImplementedException();
            }

            public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
            {
                throw new NotImplementedException();
            }
            //bool OnBeforeDownload(CefSharp.DownloadItem downloadItem, out string downloadPath, out bool showDialog)
            //{
            //    downloadPath = "";
            //    showDialog = true;
            //    return true;
            //}
            //bool OnDownloadUpdated(CefSharp.DownloadItem downloadItem)
            //{
            //    return false;
            //}
        }
    }
}
