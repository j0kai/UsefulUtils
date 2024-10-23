using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UsefulUtils.Serialization
{
    public class FileDataService : IDataService
    {
        private ISerializer m_Serializer;
        private string m_DataPath;
        private string m_FileExtension;

        public FileDataService(ISerializer serializer)
        {
            m_DataPath = Application.persistentDataPath;
            m_FileExtension = "json";
            m_Serializer = serializer;
        }

        string GetPathToFile(string fileName)
        {          
            return Path.Combine(m_DataPath, string.Concat(fileName, ".", m_FileExtension));
        }

        public void Save(GameData data, bool overwrite)
        {
            string fileLocation = GetPathToFile(data.Name);

            if (!overwrite && File.Exists(fileLocation))
                throw new IOException($"File '{data.Name}' already exists and cannot be overwritten!");

            File.WriteAllText(fileLocation, m_Serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation))
                throw new System.ArgumentException($"No persisted game data with name '{name}'");

            return m_Serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);

            if(File.Exists(fileLocation))
                File.Delete(fileLocation);
        }

        public void DeleteAll()
        {
            foreach(string filePath in Directory.GetFiles(m_DataPath))
                File.Delete(filePath);
        }

        public IEnumerable<string> ListSaves()
        {
            foreach(string path in Directory.EnumerateFiles(m_DataPath))
            {
                if(Path.GetExtension(path) == m_FileExtension)
                    yield return Path.GetFileNameWithoutExtension(path);
            }
        }


    }
}
