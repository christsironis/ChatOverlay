using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace TwitchChat
{
    /// <summary>
    /// Interaction logic for set.xaml
    /// </summary>
    public partial class set : Window
    {
        public static bool opened = false;
        public static List<CustomCss> CSSList = new List<CustomCss>();
        public static List<CustomJs> JSList = new List<CustomJs>();

        public set()
        {
            InitializeComponent();
            opened = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            #region css.xml initialization--Reader
            // Get sml serializer
            XmlSerializer CssSerial = new XmlSerializer(CSSList.GetType());  
            // checkes if file exists
            if (!File.Exists("css.xml"))
            {
                CSSList.Add(new CustomCss() { Title = "Add Custom CSS", css = "Write your CSS here..." });
                CSSList.Add(new CustomCss() { Title = "default", css = CustomCss.defaultCSS });
                StreamWriter writer = new StreamWriter("css.xml");
                CssSerial.Serialize(writer, CSSList);
                writer.Close();
                Console.WriteLine("it was empty and i added the default-chat");
            }
            // READ CSS FILE
            StreamReader reader = new StreamReader("css.xml");              // file location
            CSSList = (List<CustomCss>)CssSerial.Deserialize(reader);   // Css List
            reader.Close();

            //  CSS FILE INITIALIZATION
            if (CSSList.Count == 0)
            {
                CSSList.Add(new CustomCss() { Title = "Add Custom CSS", css = "Write your CSS here..." });
                CSSList.Add(new CustomCss() { Title = "default", css = CustomCss.defaultCSS });
                StreamWriter writer = new StreamWriter("css.xml");
                CssSerial.Serialize(writer, CSSList);
                writer.Close();
                Console.WriteLine("it was empty and i added the default-chat");
            }
            #endregion

            #region Js.xml initialization--Reader
            // Get sml serializer
            XmlSerializer JsSerial = new XmlSerializer(JSList.GetType());
            // checkes if file exists
            if (!File.Exists("js.xml"))
            {
                JSList.Add(new CustomJs() { Title = "Add Custom JS", js = "Write your JS here..." });
                JSList.Add(new CustomJs() { Title = "default", js = CustomJs.defaultJS });
                StreamWriter writer = new StreamWriter("js.xml");
                JsSerial.Serialize(writer, JSList);
                writer.Close();
                Console.WriteLine("it was empty and i added the default-chat");
            }
            // READ JS FILE
            StreamReader JsReader = new StreamReader("js.xml");              // file location
            JSList = (List<CustomJs>)JsSerial.Deserialize(JsReader);   // Css List
            JsReader.Close();

            //  JS FILE INITIALIZATION
            if (JSList.Count == 0)
            {
                JSList.Add(new CustomJs() { Title = "Add Custom CSS", js = "Write your JS here..." });
                JSList.Add(new CustomJs() { Title = "default", js = CustomJs.defaultJS });
                StreamWriter writer = new StreamWriter("js.xml");
                JsSerial.Serialize(writer, JSList);
                writer.Close();
                Console.WriteLine("it was empty and i added the default-chat-JS");
            }
            #endregion

            #region  Chat
            // chat  +  Twitch Popout
            this.Function.SelectedIndex = UserSettings.Default.ChatType;
            this.tbUsername.Text = UserSettings.Default.ChatUsername;

            // Chat
            this.tbFadeTime.Text = UserSettings.Default.ChatTime;
            this.cbFade.IsChecked = UserSettings.Default.ChatFade;
            this.cbBotActivity.IsChecked = UserSettings.Default.ChatBots;

            // comboTheme initialization
            comboTheme.ItemsSource = CSSList;
            comboTheme.DisplayMemberPath = "Title";
            comboTheme.SelectedValuePath = "css";
            comboTheme.SelectedIndex = UserSettings.Default.ComboThemeIndex;

            // comboTheme initialization
            combojs.ItemsSource = JSList;
            combojs.DisplayMemberPath = "Title";
            combojs.SelectedValuePath = "js";
            combojs.SelectedIndex = UserSettings.Default.ComboJsIndex;

            // Sounds
            //this.comboChatSound.SelectedIndex = Settings.ChatSound;
            #endregion

            #region General
            this.cbEnableTrayIcon.IsChecked = UserSettings.Default.TrayIcon;
            this.cbConfirmClose.IsChecked = UserSettings.Default.ConfirmClose;
            this.cbTaskbar.IsChecked = UserSettings.Default.TaskbarIcon;
            // this.cbInteraction.IsChecked = Settings.AllowInteraction;
            #endregion
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded || !this.list.IsLoaded)
                return;
            switch (this.list.SelectedIndex)
            {
                case 0: // Chat
                    this.chatGrid.Visibility = Visibility.Visible;
                    this.generalGrid.Visibility = Visibility.Collapsed;
                    this.widgetGrid.Visibility = Visibility.Collapsed;
                    this.aboutGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1: // Widgets
                    this.chatGrid.Visibility = Visibility.Collapsed;
                    this.generalGrid.Visibility = Visibility.Collapsed;
                    this.widgetGrid.Visibility = Visibility.Visible;
                    this.aboutGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2: // General
                    this.chatGrid.Visibility = Visibility.Collapsed;
                    this.generalGrid.Visibility = Visibility.Visible;
                    this.widgetGrid.Visibility = Visibility.Collapsed;
                    this.aboutGrid.Visibility = Visibility.Collapsed;
                    break;
                case 3: // About
                    this.chatGrid.Visibility = Visibility.Collapsed;
                    this.generalGrid.Visibility = Visibility.Collapsed;
                    this.widgetGrid.Visibility = Visibility.Collapsed;
                    this.aboutGrid.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Function_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (this.Function.SelectedIndex)
            {
                case 0: // Chat
                        // fade check
                    this.fadecheck.Visibility = Visibility.Visible;
                    // fade time
                    this.fadetime.Visibility = Visibility.Visible;
                    // bots
                    this.bots.Visibility = Visibility.Visible;
                    // sounds
                    this.sounds.Visibility = Visibility.Visible;
                    // Theme
                    this.css.SetValue(Grid.RowProperty, 3);
                    // CSS text
                    this.tbCSS.SetValue(Grid.RowProperty, 4);
                    this.tbCSS.SetValue(Grid.RowSpanProperty, 2);
                    // Javascript
                    this.javascript.SetValue(Grid.RowProperty, 3);
                    // JS text
                    this.tbJS.SetValue(Grid.RowProperty, 4);
                    this.tbJS.SetValue(Grid.RowSpanProperty, 2);
                    break;
                case 1: // Twitch Popout
                        // fade check
                    this.fadecheck.Visibility = Visibility.Collapsed;
                    // fade time
                    this.fadetime.Visibility = Visibility.Collapsed;
                    // bots
                    this.bots.Visibility = Visibility.Collapsed;
                    // sounds
                    this.sounds.Visibility = Visibility.Collapsed;
                    // Theme
                    this.css.SetValue(Grid.RowProperty, 1);
                    // CSS text
                    this.tbCSS.SetValue(Grid.RowProperty, 2);
                    this.tbCSS.SetValue(Grid.RowSpanProperty, 4);
                    // Javascript
                    this.javascript.SetValue(Grid.RowProperty, 1);
                    // JS text
                    this.tbJS.SetValue(Grid.RowProperty, 2);
                    this.tbJS.SetValue(Grid.RowSpanProperty, 4);
                    break;

            }
        }

        private void cbFade_Unchecked(object sender, RoutedEventArgs e)
        {
            tbFadeTime.IsEnabled = false;
        }
        private void comboChatSound_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void comboChatSound_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void comboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(comboTheme.SelectedValue);
            this.tbCSS.Text = (string)comboTheme.SelectedValue;

            switch (Convert.ToInt32(comboTheme.SelectedIndex))
            {
                case 0:
                    ComboStackCss.Visibility = Visibility.Collapsed;
                    NewCss.Visibility = Visibility.Visible;
                    this.tbCSS.IsReadOnly = false;
                    this.DeleteCss.IsEnabled = false;
                    break;
                case 1:
                    ComboStackCss.Visibility = Visibility.Visible;
                    NewCss.Visibility = Visibility.Collapsed;
                    this.tbCSS.IsReadOnly = true;
                    this.DeleteCss.IsEnabled = false;
                    break;
                default:
                    ComboStackCss.Visibility = Visibility.Visible;
                    NewCss.Visibility = Visibility.Collapsed;
                    this.tbCSS.IsReadOnly = false;
                    this.DeleteCss.IsEnabled = true;
                    break;
            }          
        }

        private void AddCss_Click(object sender, RoutedEventArgs e)
        {
            // WRITES THE CSS LIST IN THE FILE
            XmlSerializer CssSerial = new XmlSerializer(CSSList.GetType());  // get sml serializer
            CSSList.Add(new CustomCss() { Title = CssName.Text, css = tbCSS.Text });
            StreamWriter writer = new StreamWriter("css.xml");
            CssSerial.Serialize(writer, CSSList);
            writer.Close();
            // READS THE CSS LIST FROM THE FILE
            StreamReader reader = new StreamReader("css.xml");              // file location
            CSSList = (List<CustomCss>)CssSerial.Deserialize(reader);   // Css List
            comboTheme.ItemsSource = CSSList;
            comboTheme.DisplayMemberPath = "Title";
            comboTheme.SelectedValuePath = "css";
            reader.Close();

            ComboStackCss.Visibility = Visibility.Visible;
            NewCss.Visibility = Visibility.Collapsed;
            comboTheme.SelectedIndex = CSSList.Count - 1;
        }

        private void DeleteCss_Click(object sender, RoutedEventArgs e)
        {
            int item = comboTheme.SelectedIndex;
            CSSList.RemoveAt(item);
            // Console.WriteLine(CSSList[item]);
            comboTheme.ItemsSource = null;
            comboTheme.ItemsSource = CSSList;
            comboTheme.DisplayMemberPath = "Title";
            comboTheme.SelectedValuePath = "css";
            comboTheme.SelectedIndex = item - 1;
            // WRITES THE CSS LIST IN THE FILE
            XmlSerializer CssSerial = new XmlSerializer(CSSList.GetType());  // get sml serializer
            StreamWriter writer = new StreamWriter("css.xml");
            CssSerial.Serialize(writer, CSSList);
            writer.Close();
        }

        private void CancelCssAdd_Click(object sender, RoutedEventArgs e)
        {
            NewCss.Visibility = Visibility.Collapsed;
            ComboStackCss.Visibility = Visibility.Visible;
        }

        private void combojs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(combojs.SelectedValue);
            this.tbJS.Text = (string)combojs.SelectedValue;

            switch (Convert.ToInt32(combojs.SelectedIndex))
            {
                case 0:
                    ComboStackJs.Visibility = Visibility.Collapsed;
                    NewJs.Visibility = Visibility.Visible;
                    this.tbJS.IsReadOnly = false;
                    this.DeleteJs.IsEnabled = false;
                    break;
                case 1:
                    ComboStackJs.Visibility = Visibility.Visible;
                    NewCss.Visibility = Visibility.Collapsed;
                    this.tbJS.IsReadOnly = true;
                    this.DeleteJs.IsEnabled = false;
                    break;
                default:
                    ComboStackJs.Visibility = Visibility.Visible;
                    NewJs.Visibility = Visibility.Collapsed;
                    this.tbJS.IsReadOnly = false;
                    this.DeleteJs.IsEnabled = true;
                    break;
            }
        }

        private void AddJs_Click(object sender, RoutedEventArgs e)
        {
            // WRITES THE JS LIST IN THE FILE
            XmlSerializer JsSerial = new XmlSerializer(JSList.GetType());  // get sml serializer
            JSList.Add(new CustomJs() { Title = JsName.Text, js = tbJS.Text });
            StreamWriter writer = new StreamWriter("js.xml");
            JsSerial.Serialize(writer, JSList);
            writer.Close();
            // READS THE CSS LIST FROM THE FILE
            StreamReader JsReader = new StreamReader("js.xml");              // file location
            JSList = (List<CustomJs>)JsSerial.Deserialize(JsReader);   // Css List
            combojs.ItemsSource = JSList;
            combojs.DisplayMemberPath = "Title";
            combojs.SelectedValuePath = "js";
            JsReader.Close();

            ComboStackJs.Visibility = Visibility.Visible;
            NewJs.Visibility = Visibility.Collapsed;
            combojs.SelectedIndex = JSList.Count - 1;
        }

        private void CancelJsAdd_Click(object sender, RoutedEventArgs e)
        {
            NewJs.Visibility = Visibility.Collapsed;
            ComboStackJs.Visibility = Visibility.Visible;
        }

        private void DeleteJs_Click(object sender, RoutedEventArgs e)
        {
            int item = combojs.SelectedIndex;
            JSList.RemoveAt(item);
            // Console.WriteLine(JSList[item]);
            combojs.ItemsSource = null;
            combojs.ItemsSource = JSList;
            combojs.DisplayMemberPath = "Title";
            combojs.SelectedValuePath = "js";
            combojs.SelectedIndex = item - 1;
            // WRITES THE JS LIST IN THE FILE
            XmlSerializer JsSerial = new XmlSerializer(JSList.GetType());  // get sml serializer
            StreamWriter writer = new StreamWriter("js.xml");
            JsSerial.Serialize(writer, JSList);
            writer.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate_1(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate_2(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void Hyperlink_RequestNavigate_3(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }

        private void cbFade_Checked(object sender, RoutedEventArgs e)
        {
            tbFadeTime.IsEnabled = true;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Chat 
            // chat  +  Twitch Popout
            UserSettings.Default.ChatType = Function.SelectedIndex;
            UserSettings.Default.ChatUsername = tbUsername.Text;

            // Chat
            UserSettings.Default.ChatTime = tbFadeTime.Text;
            UserSettings.Default.ChatFade = (bool)cbFade.IsChecked;
            UserSettings.Default.ChatBots = (bool)cbBotActivity.IsChecked;
            UserSettings.Default.ComboJsIndex =combojs.SelectedIndex;
            UserSettings.Default.ComboThemeIndex = comboTheme.SelectedIndex;
            UserSettings.Default.comboJsValue = tbJS.Text;
            UserSettings.Default.ComboThemeValue = tbCSS.Text; ;
            //Settings.ChatSound = comboChatSound.SelectedIndex;

            // General
            UserSettings.Default.TrayIcon = (bool)cbEnableTrayIcon.IsChecked;
            UserSettings.Default.ConfirmClose = (bool)cbConfirmClose.IsChecked;
            UserSettings.Default.TaskbarIcon = (bool)cbTaskbar.IsChecked;
            //  cbInteraction.IsChecked = Settings.AllowInteraction;
            UserSettings.Default.Save();
            Application.Current.MainWindow.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void onClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            opened = false;
        }


    }
}
