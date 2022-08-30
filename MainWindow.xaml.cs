using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WebUI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppSDKBugs
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Environment.SetEnvironmentVariable("WEBVIEW2_BROWSER_EXECUTABLE_FOLDER", @"C:\Program Files (x86)\Microsoft\Edge Dev\Application\106.0.1356.0",
                EnvironmentVariableTarget.Process);

            var initTask = new Task(async () => {
                await WebView2.EnsureCoreWebView2Async();
                Debug.WriteLine("CoreWebView2 " + WebView2.CoreWebView2.Environment.BrowserVersionString);

                 await WebView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Page.enable", "{}");
                WebView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Page.fileChooserOpened").DevToolsProtocolEventReceived += OnPageFileChooserOpened;
                await WebView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Page.setInterceptFileChooserDialog", "{\"enabled\": true}");
            });
            initTask.RunSynchronously();
        }

        private void OnClickGo(object sender, RoutedEventArgs e)
        {
            WebView2.Source = new System.Uri(AddressTextBox.Text);
        }

        private void OnPageFileChooserOpened(CoreWebView2 sender, CoreWebView2DevToolsProtocolEventReceivedEventArgs args)
        {
            Debug.WriteLine("OnPageFileChooserOpened");
        }

    }
}
