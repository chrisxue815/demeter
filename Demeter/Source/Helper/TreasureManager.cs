using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Demeter
{
    public class TreasureManager
    {
        string currentLevel;

        int stillToGet;
        public int StillToGet
        {
            get { return stillToGet; }
        }

        public TreasureManager(string levelFileName, int stillToGet)
        {
            this.currentLevel = levelFileName;
            this.stillToGet = stillToGet;
        }

        public void ResetLevel()
        {
            if (File.Exists(@"Uerprofile\profile.xml"))
            {
                File.Delete(@"Uerprofile\profile.xml");
            }
        }

        public void GetTreasure(string id)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"Uerprofile\profile.xml");

            XmlNode node = xmlDoc.SelectSingleNode("levels");

            XmlElement xe = xmlDoc.CreateElement("treasure");
            xe.SetAttribute("id", id);

            node.AppendChild(xe);

            xmlDoc.Save(@"Uerprofile\profile.xml");
        }

        public List<String> AreGotten()
        {
            List<String> ids = new List<string>() ;
            if (!File.Exists(@"Uerprofile\profile.xml"))
            {
                CreateXmlFile();
            }

            XmlTextReader reader = new XmlTextReader(@"Uerprofile\profile.xml");
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "treasure")
                    {
                        string id = reader.GetAttribute("id");
                        ids.Add(id);
                    }
                }
            }
            reader.Close();

            return ids;
        }
        public void CreateXmlFile()
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
