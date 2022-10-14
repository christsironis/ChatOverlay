using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatOverlay
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SettingsWin : Window
    {
        public MainWindow parent;
        public Dictionary<string, string> storedCss;
        public Dictionary<string, string> storedJs;
        public Dictionary<string, string> css = new Dictionary<string, string>();
        public Dictionary<string, string> js = new Dictionary<string, string>();

        public SettingsWin( MainWindow parent )
        {
            InitializeComponent();
            this.parent = parent;

            // retrieve custom css & js
            Dictionary<string, Dictionary<string, string>> data = StoreData.Retrieve();
            css = data["css"];
            js = data["js"];

            InitializeValues();
        }
        private void ChatType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void InitializeValues()
        {

            ComboBoxItem cssItem = new ComboBoxItem();
            cssItem.Content = "Add New";
            cssItem.IsSelected = true;
            comboCss.Items.Add(cssItem);

            ComboBoxItem jsItem = new ComboBoxItem();
            jsItem.Content = "Add New";
            jsItem.IsSelected = true;
            comboJs.Items.Add(jsItem);

            foreach (KeyValuePair<string, string> item in css)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Key;
                if (item.Key == Properties.Settings.Default.ComboCss)
                {
                    comboBoxItem.IsSelected = true;
                }
                comboCss.Items.Add(comboBoxItem);

            }

            foreach (KeyValuePair<string, string> item in js)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Key;
                if (item.Key == Properties.Settings.Default.ComboJs)
                {
                    comboBoxItem.IsSelected = true;
                }
                comboJs.Items.Add(comboBoxItem);
   
            }

            ChatType.Text = Properties.Settings.Default.ChatType;
            ChatFade.Text = Properties.Settings.Default.ChatFade.ToString();
            CustomUrl.Text = Properties.Settings.Default.CustomUrl;
            sBots.IsChecked = Properties.Settings.Default.ShowBots;
            KapChatTheme.Text = Properties.Settings.Default.KapchatTheme;
            username.Text = Properties.Settings.Default.Username;
            comboCss.Text = Properties.Settings.Default.ComboCss;
            comboJs.Text = Properties.Settings.Default.ComboJs;
            cssText.Text = css[Properties.Settings.Default.ComboCss];
            jsText.Text = js[Properties.Settings.Default.ComboJs];
        }

        private void ChatFade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            String text = (String)e.Text;
            if ( !(new Regex("^[0-9]+$")).IsMatch(text) )
            {
                e.Handled = true;
            }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ChatType = ChatType.Text;
            Properties.Settings.Default.ChatFade = Int32.Parse(ChatFade.Text);
            Properties.Settings.Default.ShowBots = (bool)sBots.IsChecked;
            Properties.Settings.Default.KapchatTheme = KapChatTheme.Text;
            Properties.Settings.Default.Username = username.Text;
            Properties.Settings.Default.CustomUrl = CustomUrl.Text;
            Properties.Settings.Default.ComboCss = comboCss.Text;
            Properties.Settings.Default.ComboJs = comboJs.Text;
            Properties.Settings.Default.currentCss = css[comboCss.Text];
            Properties.Settings.Default.currentJs = js[comboJs.Text];
            Properties.Settings.Default.Save();

            ComboBoxItem cssItem = (ComboBoxItem)comboCss.SelectedItem;
            css[cssItem.Content.ToString()] = cssText.Text;

            ComboBoxItem jsItem = (ComboBoxItem)comboJs.SelectedItem;
            js[jsItem.Content.ToString()] = jsText.Text;

            this.parent.ChangeUrlOnChatType();
            StoreData.Store(css,js);
            this.Close();
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add_Css_Item(object sender, RoutedEventArgs e)
        {
            if (css.ContainsKey(newCssName.Text)) return;

            css.Add(newCssName.Text, cssText.Text);

            ComboBoxItem cssItem = new ComboBoxItem();
            cssItem.Content = newCssName.Text;
            cssItem.IsSelected = true;
            comboCss.Items.Add(cssItem);

            newCssName.Text = "";
        }
        private void Add_Js_Item(object sender, RoutedEventArgs e)
        {
            if (js.ContainsKey(newJsName.Text)) return;

            js.Add(newJsName.Text, jsText.Text);

            ComboBoxItem jsItem = new ComboBoxItem();
            jsItem.Content = newJsName.Text;
            jsItem.IsSelected = true;
            comboJs.Items.Add(jsItem);

            newJsName.Text = "";
        }
        private void Delete_Css_Item(object sender, RoutedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboCss.SelectedItem;
            comboCss.Items.Remove(comboCss.SelectedItem);
            comboJs.SelectedIndex = 1;

            css.Remove(value.Content.ToString());
        }
        private void Delete_Js_Item(object sender, RoutedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboJs.SelectedItem;
            comboJs.Items.Remove(comboJs.SelectedItem);
            comboJs.SelectedIndex = 1;

            js.Remove(value.Content.ToString());
        }

        private void comboCss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboCss.SelectedItem;
            if (value == null)
            {
                comboCss.SelectedIndex = 1;
                return;
            }
            if (value.Content.ToString() == "Add New")
            {
                cssText.Text = "";
            }
            else
                cssText.Text = css[ value.Content.ToString() ];
        }

        private void comboJs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboJs.SelectedItem;
            if (value == null)
            {
                comboJs.SelectedIndex = 1;
                return;
            }
            if(value.Content.ToString() == "Add New")
            {
                jsText.Text = "";
            }
            else
                jsText.Text = js[ value.Content.ToString() ];
        }

        private void cssText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void jsText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
