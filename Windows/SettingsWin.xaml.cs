using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public SettingsWin(MainWindow parent)
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
            cssText.Text = css.ContainsKey(Properties.Settings.Default.ComboCss) ? css[Properties.Settings.Default.ComboCss] : "";
            jsText.Text = js.ContainsKey(Properties.Settings.Default.ComboJs) ? js[Properties.Settings.Default.ComboJs] : "";
            backgroundColorString.Text = Properties.Settings.Default.ContentBackground;
        }

        private void ChatFade_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            String text = (String)e.Text;
            if (!(new Regex("^[0-9]+$")).IsMatch(text))
            {
                e.Handled = true;
            }
        }

        private void apply_Click(object sender, RoutedEventArgs e)
        {
            string cssItemName;
            ComboBoxItem cssItem = (ComboBoxItem)comboCss.SelectedItem;
            if (cssItem.Content.ToString() != "Add New")
            {
                cssItemName = cssItem.Content.ToString().Trim();
                css[cssItemName] = cssText.Text;
                Properties.Settings.Default.currentCss = css[cssItemName];
            }
            else {
                Properties.Settings.Default.currentCss = "";
                cssItemName = "None";
            }

            string jsItemName;
            ComboBoxItem jsItem = (ComboBoxItem)comboJs.SelectedItem;
            if (cssItem.Content.ToString() != "Add New")
            {
                jsItemName = comboJs.Text.Trim();  
                js[jsItemName] = jsText.Text;
                Properties.Settings.Default.currentJs = js[jsItemName];
            }
            else
            {
                Properties.Settings.Default.currentJs = "";
                jsItemName = "None";
            }

            Properties.Settings.Default.ChatType = ChatType.Text;
            Properties.Settings.Default.ChatFade = Int32.Parse(ChatFade.Text);
            Properties.Settings.Default.ShowBots = (bool)sBots.IsChecked;
            Properties.Settings.Default.KapchatTheme = KapChatTheme.Text;
            Properties.Settings.Default.Username = username.Text;
            Properties.Settings.Default.CustomUrl = CustomUrl.Text;
            Properties.Settings.Default.ComboCss = cssItemName;
            Properties.Settings.Default.ComboJs = jsItemName;
            Properties.Settings.Default.ContentBackground = backgroundColorString.Text;
            Properties.Settings.Default.Save();

            this.parent.ApplySettings();
            StoreData.Store(css, js);
            this.Close();
        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Add_Css_Item(object sender, RoutedEventArgs e)
        {
            if (css.ContainsKey(newCssName.Text) || newCssName.Text.Trim() == "") return;

            css.Add(newCssName.Text, cssText.Text);

            ComboBoxItem cssItem = new ComboBoxItem();
            cssItem.Content = newCssName.Text.Trim();
            cssItem.IsSelected = true;
            comboCss.Items.Add(cssItem);

            newCssName.Text = "";
        }
        private void Add_Js_Item(object sender, RoutedEventArgs e)
        {
            if (js.ContainsKey(newJsName.Text) || newJsName.Text.Trim() == "") return;

            js.Add(newJsName.Text, jsText.Text);

            ComboBoxItem jsItem = new ComboBoxItem();
            jsItem.Content = newJsName.Text.Trim();
            jsItem.IsSelected = true;
            comboJs.Items.Add(jsItem);

            newJsName.Text = "";
        }
        private void Delete_Css_Item(object sender, RoutedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboCss.SelectedItem;
            comboCss.Items.Remove(comboCss.SelectedItem);
            comboCss.SelectedIndex = 1;

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
                cssText.Text = css[value.Content.ToString()];
        }

        private void comboJs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem value = (ComboBoxItem)comboJs.SelectedItem;
            if (value == null)
            {
                comboJs.SelectedIndex = 1;
                return;
            }
            if (value.Content.ToString() == "Add New")
            {
                jsText.Text = "";
            }
            else
                jsText.Text = js[value.Content.ToString()];
        }

        private void cssText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void jsText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
