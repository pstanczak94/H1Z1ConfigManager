using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace H1Z1_Config_Manager
{
    public class Settings
    {
        public const string ConfigFilePath = "H1Z1 Config Manager.xml";
        public const string UserOptionsFileName = "UserOptions.ini";
        public const string UserOptionsBackupFileName = "UserOptions_backup.ini";
        public static string UserOptionsPath;

        public static void Init()
        {
            UserOptionsPath = String.Empty;
        }
        
        public static void Read()
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(ConfigFilePath);
            }
            catch
            {
                Logger.WriteLine(LogType.WARNING, "Unable to read app config.");
                return;
            }

            XmlNodeList list = doc.GetElementsByTagName("Settings");

            if (list.Count == 0)
                return;

            XmlNode settingsNode = list[0];

            foreach (XmlNode node in settingsNode.ChildNodes)
            {
                if (node.Name.Equals("UserOptionsPath"))
                {
                    UserOptionsPath = node.InnerText;
                }
            }
        }

        public static void Save()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);

            XmlNode first = doc.CreateNode(XmlNodeType.Element, "Settings", String.Empty);

            doc.AppendChild(first);

            XmlElement element = doc.CreateElement("UserOptionsPath");
            element.InnerText = UserOptionsPath;

            first.AppendChild(element);

            TextWriter writer = new StreamWriter(ConfigFilePath, false, Encoding.UTF8);

            doc.Save(writer);

            writer.Close();
        }
    }
}
