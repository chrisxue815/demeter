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
            if (File.Exists(@"Profile\profile.xml"))
            {
                File.Delete(@"Profile\profile.xml");
            }
        }

        public void GetTreasure(string id)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"Profile\profile.xml");

            XmlNode node = xmlDoc.SelectSingleNode("profile");

            XmlElement xe = xmlDoc.CreateElement("treasure");
            xe.SetAttribute("id", id);

            node.AppendChild(xe);

            xmlDoc.Save(@"Profile\profile.xml");
        }

        public List<String> AreGotten()
        {
            return Xml.GetValues("treasure", "id");
        }
    }
}
