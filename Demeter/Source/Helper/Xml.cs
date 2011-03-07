using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Demeter
{
    static class Xml
    {
        public static string LoadConfig(string tagName, string propertyName)
        {
            string value = null; ;

            if (!File.Exists(@"Uerprofile\profile.xml"))
            {
                CreateXmlFile();
            }
            else
            {
                XmlTextReader reader = new XmlTextReader(@"Uerprofile\profile.xml");
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == tagName)
                        {
                            value = reader.GetAttribute(propertyName);
                            break;
                        }
                    }
                }
                reader.Close();
            }

            return null;
        }

        public static void CreateXmlFile()
        {
            DirectoryInfo dir = new DirectoryInfo("Uerprofile");
            if (!dir.Exists)
            {
                dir.Create();
            }

            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement xmlelement;
            xmlelement = doc.CreateElement("levels");
            doc.AppendChild(xmlelement);

            doc.Save(@"Uerprofile\profile.xml");
            doc = null;
        }
    }
}
