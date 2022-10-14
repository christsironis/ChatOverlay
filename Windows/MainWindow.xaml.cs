using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public MainWindow()
        {
            InitializeComponent();
            ChangeUrlOnChatType();

        }
        private string InsertCustomCSS(string CSS)
        {
            string uriEncodedCSS = Uri.EscapeDataString(CSS);
            string script = "const ttcCSS = document.createElement('style');";
            script += "ttcCSS.innerHTML = decodeURIComponent(\"" + uriEncodedCSS + "\");";
            script += "document.querySelector('head').appendChild(ttcCSS);";
            return script;
        }

        private void InsertCustomJavaScript(string JS)
        {
            try
            {
                JS = "document.addEventListener(\"DOMContentLoaded\", function() { " + JS + "});";
                this.Browser.ExecuteScriptAsync(JS);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void ChangeUrlOnChatType()
        {
            if (Properties.Settings.Default.ChatType == "KapChat - Twitch")
            {
                Browser.Address = $"https://nightdev.com/hosted/obschat?theme={Properties.Settings.Default.KapchatTheme}&channel={Properties.Settings.Default.Username}&fade={Properties.Settings.Default.ChatFade}&bot_activity={Properties.Settings.Default.ShowBots}&prevent_clipping=false";
                url.Text = Properties.Settings.Default.Username;
            }
            else if (Properties.Settings.Default.ChatType == "Twitch")
            {
                Browser.Address = $"https://www.twitch.tv/popout/{Properties.Settings.Default.Username}/chat?popout=";
                url.Text = Properties.Settings.Default.Username;
            }
            else if (Properties.Settings.Default.ChatType == "Twitch")
            {
                Browser.Address = $"https://www.twitch.tv/popout/{Properties.Settings.Default.Username}/chat?popout=";
                url.Text = Properties.Settings.Default.Username;
            }
            else
            {
                Browser.Address = Properties.Settings.Default.CustomUrl; 
                url.Text = Properties.Settings.Default.CustomUrl;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
            {
                Browser.Back();
                url.Text = Browser.Address;
            }
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoForward)
            {
                Browser.Forward();
                url.Text = Browser.Address;
            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWin settingsWin = new SettingsWin( this );
            settingsWin.Show();
        }

        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Properties.Settings.Default.ChatType == "KapChat - Twitch")
                {
                    Properties.Settings.Default.Username = url.Text;
                    Browser.Address = $"https://nightdev.com/hosted/obschat?theme={Properties.Settings.Default.KapchatTheme}&channel={Properties.Settings.Default.Username}&fade={Properties.Settings.Default.ChatFade}&bot_activity={Properties.Settings.Default.ShowBots}&prevent_clipping=false";
                }
                else if (Properties.Settings.Default.ChatType == "Twitch")
                {
                    Properties.Settings.Default.Username = url.Text;
                    Browser.Address = $"https://www.twitch.tv/popout/{Properties.Settings.Default.Username}/chat?popout=";
                }
                else if (Properties.Settings.Default.ChatType == "Twitch")
                {
                    Properties.Settings.Default.Username = url.Text;
                    Browser.Address = $"https://www.twitch.tv/popout/{Properties.Settings.Default.Username}/chat?popout=";
                }
                else
                {
                    Properties.Settings.Default.CustomUrl = url.Text;
                    Browser.Address = Properties.Settings.Default.CustomUrl;
                }
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void restore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            taskbarIcon.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private void toggleBorders_Click(object sender, RoutedEventArgs e)
        {

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            // Change the extended window style to include WS_EX_TRANSPARENT
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);

            if (border.Visibility == Visibility.Visible)
            {
                border.Visibility = Visibility.Hidden;
                topBorder.BorderThickness = new Thickness(0);
                mainContent.BorderThickness = new Thickness(0);
                MainWindow.SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            }
            else
            {
                border.Visibility = Visibility.Visible;
                topBorder.BorderThickness = new Thickness(0,10,0,0);
                mainContent.BorderThickness = new Thickness(2);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomInCommand.Execute(null);
        }
        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomOutCommand.Execute(null);
        }
        private void ZoomReset_Click(object sender, RoutedEventArgs e)
        {
            Browser.ZoomLevel = 0;
        }
        private void OpacityMore_Click(object sender, RoutedEventArgs e)
        {
            Browser.Opacity += 0.05;
        }
        private void OpacityLess_Click(object sender, RoutedEventArgs e)
        {
            Browser.Opacity -= 0.05;
        }
        private void OpacityReset_Click(object sender, RoutedEventArgs e)
        {
            Browser.Opacity = 1;
        }
        private void Window_f9_KeyDown(object sender, KeyEventArgs e)
        {
            if( e.Key == Key.F9)
                this.toggleBorders_Click(sender, e);
        }
        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            win2.Show();
        }
        private void Show_Window(object sender, RoutedEventArgs e)
        {
            if( this.WindowState == WindowState.Minimized ) 
                this.WindowState = WindowState.Normal;
            this.Topmost = true;
        }
        private void ResetWindow_Click(object sender, RoutedEventArgs e)
        {
            // reset visibility & transparense
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            // Change the extended window style to include WS_EX_TRANSPARENT
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            border.Visibility = Visibility.Visible;
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle & ~WS_EX_TRANSPARENT);
            // reset opacity
            Browser.Opacity = 1;
            // reset zoom
            Browser.ZoomLevel = 0;
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {

            InsertCustomJavaScript(InsertCustomCSS(Properties.Settings.Default.currentCss));
            InsertCustomJavaScript(Properties.Settings.Default.currentJs);
        }
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //InsertCustomJavaScript(InsertCustomCSS(Properties.Settings.Default.currentCss));
            //InsertCustomJavaScript(Properties.Settings.Default.currentJs);
        }
    }
}
