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

        public void Save<T>(T data, string fileName, bool overwrite)
        {
            string fileLocation = GetPathToFile(fileName);

            if (!overwrite && File.Exists(fileLocation))
                throw new IOException($"File '{fileName}' already exists and cannot be overwritten!");

            File.WriteAllText(fileLocation, m_Serializer.Serialize(data));
        }

        public T Load<T>(string fileName)
        {
            string fileLocation = GetPathToFile(fileName);

            if (!File.Exists(fileLocation))
                throw new System.ArgumentException($"No persisted game data with name '{fileName}'");

            return m_Serializer.Deserialize<T>(File.ReadAllText(fileLocation));
        }

        public void Delete(string fileName)
        {
            string fileLocation = GetPathToFile(fileName);

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
