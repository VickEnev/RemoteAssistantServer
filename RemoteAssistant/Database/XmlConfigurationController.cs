using System.IO;
using System.Xml.Serialization;

namespace RemoteAssistantStorage
{
    public class XmlConfiguration
    {
        public int ExpirationTimeInMinutes { get; set; } = 60;
    }

    public class XmlConfigurationController
    {
        /// <summary>
        /// Съдържа името на файла и пътя към него
        /// </summary>
        private string mFullFileName;
        public XmlConfiguration XmlConfiguration { get; private set; }

        public static bool CheckIfConfigFileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        public XmlConfigurationController(string fullFileName)
        {
            mFullFileName = fullFileName;
        }

        public void CreateFile(bool overwrite = false)
        {
            if (File.Exists(mFullFileName) && !overwrite)
                return;

            File.Create(mFullFileName).Dispose();
            WriteToXmlConfigurationFile(new XmlConfiguration());
        }
      
        public bool ReadXmlConfigurationFile()
        {
            if (!File.Exists(mFullFileName))
                return false;

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlConfiguration));
                FileStream fileStream = new FileStream(mFullFileName, FileMode.Open);

                XmlConfiguration = (XmlConfiguration)xmlSerializer.Deserialize(fileStream);
                fileStream.Close();
            }
            catch { }

            return true;
        }

        public bool WriteToXmlConfigurationFile(XmlConfiguration newXmlConfiguration)
        {
            if (!File.Exists(mFullFileName))
                return false;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XmlConfiguration));
                TextWriter writer = new StreamWriter(mFullFileName);

                serializer.Serialize(writer, newXmlConfiguration);
                writer.Close();
            }
            catch { }

            return true;
        }

    }
}
