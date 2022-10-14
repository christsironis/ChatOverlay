using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml.Linq;

namespace ChatOverlay
{
    internal class StoreData
    {
        public static string filename = "Custom_CSS_JS.xml";
        public static void Store(Dictionary<string, string> css, Dictionary<string, string> js)
        {
            var xdec = new XDeclaration("1.0", "utf-8", "yes");
            var root = new XElement("root",
                new XElement("css", css.Select(kv => new XElement(kv.Key, kv.Value))),
                new XElement("js", js.Select(kv => new XElement(kv.Key, kv.Value)))
            );

            var doc = new XDocument(xdec, new XElement(root));
            doc.Save(filename, SaveOptions.OmitDuplicateNamespaces);

        }
        public static Dictionary<string, Dictionary<string, string>> Retrieve()
        {
            //if (!File.Exists(filename)) StoreData.Store(new Dictionary<string, string>() { { "None", "" }}, new Dictionary<string, string> () { { "None", "" } });

            XDocument doc = XDocument.Load(filename);
            Dictionary<string, string> css = new Dictionary<string, string>();
            Dictionary<string, string> js = new Dictionary<string, string>();

            foreach (XElement element in doc.Descendants() )
            {
                if( element.Name == "css")
                {
                    foreach (XElement elem in element.Descendants())
                    {
                        css.Add(elem.Name.ToString(), elem.Value.ToString());
                    }
                }
                else if (element.Name == "js")
                {
                    foreach (XElement elem in element.Descendants())
                    {
                        js.Add(elem.Name.ToString(), elem.Value.ToString());
                    }
                }

            }
            Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
            data.Add("css",css);
            data.Add("js", js);
            return data;
        }
    }
}
