using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using CefSharp;
using WpfAnimatedGif;

namespace TwitchChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string HomeAddr { get; set; } = @"https://www.twitch.tv/popout/" + UserSettings.Default.ChatUsername + "/chat?popout=";
        public string JS { get; set; } = "/* alert('All Resources Have Loaded');*/";
        public string CSS { get; set; } = UserSettings.Default.ComboThemeValue;
        //public RenderProcessMessageHandler script =new RenderProcessMessageHandler { script = script2 };
        public MainWindow()
        {

            InitializeComponent();
            browser.Address = HomeAddr; // browser address initialization
            //browser.RenderProcessMessageHandler = script;
            DataContext = new WindowViewModel(this);


        }
        private void Window_Initialized(object sender, EventArgs e)
        {

        }

        #region Custom Context Menu appear on right click
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowhandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        //The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window.
        //This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
        private const uint WM_NCRBUTTONDOWN = 0xa4;

        //window message parameter for the hit test in the title bar
        private const uint HTCAPTION = 0x02;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Message for the System Menu
            if ((msg == WM_NCRBUTTONDOWN) && (wParam.ToInt32() == HTCAPTION))
            {
                //Show our context menu
                ShowContextMenu();

                //prevent default context menu from appearing
                handled = true;
            }

            return IntPtr.Zero;
        }

        private void ShowContextMenu()
        {
            var contextMenu = Resources["contextMenu"] as ContextMenu;
            contextMenu.IsOpen = true;
        }
        #endregion

        private void ChromiumWebBrowser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Console.WriteLine("started");
            this.Dispatcher.Invoke(() =>
            {
                loading.Visibility = Visibility.Visible;
                ImageBehavior.GetAnimationController(loading).Play();
            });
        }

        private void ChromiumWebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                Console.WriteLine("frameloadend executed");
                //e.Frame.ExecuteJavaScriptAsync(script2);
                browser.ExecuteScriptAsync("(function () { let JS= document.createElement('script'); JS.innerHTML = window.atob(\"" + Convert.ToBase64String(Encoding.UTF8.GetBytes(JS)) + "\"); document.querySelector('head').appendChild(JS);  const CSS = document.createElement('style');   CSS.innerHTML = window.atob(\"" + Convert.ToBase64String(Encoding.UTF8.GetBytes(CSS)) + "\");   document.querySelector('head').appendChild(CSS); })();");
            }
            this.Dispatcher.Invoke(() =>
            {
                loading.Visibility = Visibility.Collapsed;
                ImageBehavior.GetAnimationController(loading).Pause();
            });
        }
        private void browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            //Wait for the Page to finish loading
            if (e.IsLoading == false)
            {
                Console.WriteLine("loadingchanged executed");
            }

        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            this.browser.Address = AddressText.Text;
        }

        private void AddressText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.browser.Address = AddressText.Text;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (browser.CanGoBack)
            {
                browser.Back();

            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (browser.CanGoForward)
            {
                browser.Forward();
            }
        }

        private void browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AddressText.Text = browser.Address;
        }

        private void AppWindow_Initialized(object sender, EventArgs e)
        {

        }
    }
}
