using AYAOPv1.Source.Interfaces;
using System.IO;
namespace AYAOPv1.Source.Abstract
{
    public abstract class JsonFile : IJsonFile
    {
        private string jsonFolderPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\AppData\");
        private string jsonFilePath = "ShortCutData.json";
        private string fullJsonPath = string.Empty;

        public JsonFile()
        {
            fullJsonPath = jsonFolderPath + jsonFilePath;
        }

        private void Create()
        {
            if (!Directory.Exists(jsonFolderPath)) Directory.CreateDirectory(jsonFolderPath);
            else
            {
                if (!File.Exists(fullJsonPath))
                {
                    using(var file = File.Create(fullJsonPath)) { }
                }
            }
        }

        public void Write(string json)
        {
            Create();
            File.WriteAllText(fullJsonPath, json);
        }
        public string Read() 
        {
            Create();
            using (var fileStream = new FileStream(fullJsonPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(var reader = new StreamReader(fileStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
