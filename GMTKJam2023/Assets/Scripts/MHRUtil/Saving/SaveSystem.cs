using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MHRUtil.Saving
{
    public static class SaveSystem
    {
        private static Dictionary<string, string> _data = new();
        private static SaveInfo _saveInfo;
        private static string _path;
        private static string _name;
        private static string _pathAndName;
        
        private const string DEFAULT_FILE_PATH = "MHRUtil/Test/";
        private const string DEFAULT_FILE_NAME = "TestFile";
        private const string DEFAULT_FILE_EXTENSION = "save";
        private const bool DEFAULT_PRETTY_PRINT = true;
        
        public static void SetData<T>(string name, T data)
        {
            string jsonData = JsonUtility.ToJson(data, _saveInfo == null ? DEFAULT_PRETTY_PRINT : _saveInfo.PrettyPrint);
            
            if (_data.ContainsKey(name))
            {
                _data[name] = jsonData;
            }
            else
            {
                _data.Add(name, jsonData);
            }
        }

        public static bool TryGetData<T>(string name, ref T data)
        {
            if (!_data.ContainsKey(name))
            {
                return false;
            }

            data = (T)JsonUtility.FromJson(_data[name], typeof(T));
            return true;
        }
        
        public static void Save()
        {
            WriteToFile();
        }

        public static void Load(SaveInfo saveInfo = null)
        {
            _saveInfo = saveInfo;
            
            TrySetFileAndDirectory();
            TryCreateFileAndDirectory(_pathAndName, _path);

            ReadFromFile();
        }

        private static void WriteToFile()
        {
            StreamWriter streamWriter = new StreamWriter(_pathAndName);
            foreach (KeyValuePair<string, string> entry in _data)
            {
                streamWriter.WriteLine("[" + entry.Key + "]" + entry.Value);
            }
            
            streamWriter.Close();
        }

        private static void ReadFromFile()
        {
            string text = "";
            
            StreamReader streamReader = new StreamReader(_pathAndName);
            while (streamReader.Peek() >= 0)
            {
                text += streamReader.Read();
            }

            text.Split('[');

            streamReader.Close();
        }

        private static void TrySetFileAndDirectory()
        {
            if (!string.IsNullOrEmpty(_path) && !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_pathAndName))
                return;
            
            _path = GetFilePath();
            _name = GetFileName();
            _pathAndName = _path + _name;
        }
        
        private static string GetFilePath()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), _saveInfo == null ? DEFAULT_FILE_PATH : _saveInfo.FilePath);
#elif UNITY_WEBGL
            return string.Empty;
#endif
        }

        private static string GetFileName()
        {
            if (_saveInfo == null)
            {
                return DEFAULT_FILE_NAME + "." + DEFAULT_FILE_EXTENSION;
            }
            else
            {
                return _saveInfo.FileName + "." + _saveInfo.FileExtension;
            }
        }
        
        private static void TryCreateFileAndDirectory(string pathAndName, string path)
        {
            if (File.Exists(pathAndName))
                return;
            
            Directory.CreateDirectory(path);
            FileStream tmp = File.Create(pathAndName);
            tmp.Close();
        }
    }
}