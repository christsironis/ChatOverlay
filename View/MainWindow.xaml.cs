using CefSharp;
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

namespace ChatOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
            {
                Browser.Back();

            }
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoBack)
            {
                Browser.Forward();

            }
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void url_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Browser.Load( url.Text );
            }
        }

        private void Browser_AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Browser.CanGoForward)
            {
                forward.Opacity = 1;

            }
            else
            {
                forward.Opacity = 0.8;
            }
            if (Browser.CanGoBack)
            {
                back.Opacity = 1;

            }
            else
            {
                back.Opacity = 0.8;
            }
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

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
            Application.Current.Shutdown();
        }
    }
}
