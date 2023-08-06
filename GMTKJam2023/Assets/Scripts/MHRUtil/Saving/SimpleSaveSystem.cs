using System.IO;
using System.Text;
using UnityEngine;

namespace MHRUtil.Saving
{
    public static class SimpleSaveSystem
    {
        private static string _data;
        private static SaveInfo _saveInfo;
        private static string _path;
        private static string _name;
        private static string _pathAndName;

        private const string DEFAULT_FILE_PATH = "MHRUtil/Test/";
        private const string DEFAULT_FILE_NAME = "TestFile";
        private const string DEFAULT_FILE_EXTENSION = "save";
        private const bool DEFAULT_PRETTY_PRINT = true;

        public static void SetData<T>(T data)
        {
            string jsonData = JsonUtility.ToJson(data, _saveInfo == null ? DEFAULT_PRETTY_PRINT : _saveInfo.PrettyPrint);

            _data = jsonData;
        }

        public static T TryGetData<T>()
        {
            T data = (T)JsonUtility.FromJson(_data, typeof(T));
            return data;
        }
        
        public static void Save()
        {
            WriteToFile();
        }

        public static void Load(SaveInfo saveInfo = null)
        {
            _saveInfo = saveInfo;
            
            TrySetFileAndDirectory();
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            TryCreateFileAndDirectory(_pathAndName, _path);
#endif

            ReadFromFile();
        }

        private static void WriteToFile()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            StreamWriter streamWriter = new StreamWriter(_pathAndName);
            streamWriter.Write(_data);

            streamWriter.Close();
#elif UNITY_WEBGL
            PlayerPrefs.SetString(_name, _data);
            PlayerPrefs.Save();
#endif
            TryDebugLogString("Wrote data as\n", _data);
        }

        private static void ReadFromFile()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            string text = "";
            
            StreamReader streamReader = new StreamReader(_pathAndName);
            text = streamReader.ReadToEnd();
            _data = text;

            streamReader.Close();
#elif UNITY_WEBGL
            _data = PlayerPrefs.GetString(_name);
#endif
            TryDebugLogString("Read data as\n", _data);
        }

        private static void TrySetFileAndDirectory()
        {
            if (!string.IsNullOrEmpty(_path) && !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_pathAndName))
                return;
            
            _path = _GetFilePath();
            _name = _GetFileName();
            _pathAndName = _path + _name;
            
            string _GetFilePath()
            {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), _saveInfo == null ? DEFAULT_FILE_PATH : _saveInfo.FilePath);
#elif UNITY_WEBGL
            return string.Empty;
#endif
            }
            
            string _GetFileName()
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
        }

        private static void TryCreateFileAndDirectory(string pathAndName, string path)
        {
            if (File.Exists(pathAndName))
                return;
            
            Directory.CreateDirectory(path);
            FileStream tmp = File.Create(pathAndName);
            tmp.Close();
        }

        private static void TryDebugLogString(params string[] toLog)
        {
            if (_saveInfo == null || !_saveInfo.DebugLogSaveData)
                return;

            string output = "<color=#FFBF00>SimpleSaveSystem log:</color> ";

            foreach (string str in toLog)
            {
                output += str;
            }
            Debug.Log(output);
        }
    }
}