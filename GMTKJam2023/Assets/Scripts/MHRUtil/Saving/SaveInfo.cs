using UnityEngine;

namespace MHRUtil.Saving
{
    [CreateAssetMenu(fileName = "New Save Info", menuName = "MHR Util/Save Info", order = 1)]
    public class SaveInfo : ScriptableObject
    {
        public string FilePath => _filePath;
        public string FileName => _fileName;
        public string FileExtension => _fileExtension;
        public bool PrettyPrint => _prettyPrint;
        public bool DebugLogSaveData => _debugLogSaveData;
        
        [SerializeField] private string _filePath = "MattRoysOurBoy\\";
        [SerializeField] private string _fileName = "SaveData";
        [SerializeField] private string _fileExtension = "save";
        [SerializeField] private bool _prettyPrint;
        [SerializeField] private bool _debugLogSaveData;
    }
}