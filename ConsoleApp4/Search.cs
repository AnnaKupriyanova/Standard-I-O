using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp4 { 
  [Serializable]
  public class Search { 
    public string DirectoryName { get; set; }
    private List<TextFile> Files { get; set; } = new List<TextFile>();
    public Search(string directory) {
      if (!Directory.Exists(directory)) {
        throw new Exception("Directory doesn`t exist.");
      }
      DirectoryName = directory;
    }

    public void GetFiles() { 
      string[] names = Directory.GetFiles(DirectoryName);  
      foreach (string fileName in names) {
        Console.WriteLine(fileName.Remove(0, DirectoryName.Length + 1));
      }
    }

    public void AddFile(string name) { 
      name = DirectoryName + "/" + name;
      if (!File.Exists(name)) { 
        throw new Exception ("File doesn`t exist.");      
      }
      foreach (var file in Files) {
        if (file.Name == name) {
          throw new Exception ("File already added.");
        }
      }
      string[] keyWords = new string[2];
      Console.WriteLine($"Enter key words for {name.Remove(0, DirectoryName.Length + 1)}:");
      keyWords[0] = Console.ReadLine();
      keyWords[1] = Console.ReadLine();
      Files.Add(new TextFile(name, keyWords));
      Console.WriteLine("File added.");
    }

    public void SearchFile(string[] keyWords) { 
      Console.WriteLine("Files found:");
      var filesFound = false;
      foreach (var file in Files) {
        if (file.KeyWords[0] == keyWords[0] || file.KeyWords[0] == keyWords[1] || file.KeyWords[1] == keyWords[0] || file.KeyWords[1] == keyWords[1]) {
          Console.WriteLine($"{file.Name.Remove(0, DirectoryName.Length + 1)}"); //            
          filesFound = true;
        }
      }
      if (!filesFound) {
        throw new Exception("No files found.")   ; 
      }
    }

    public void Serialize(FileStream fileStream) {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize(fileStream, this);
      fileStream.Close();
    }

    public void Deserialize(FileStream fileStream) {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      Search deserialized = (Search)binaryFormatter.Deserialize(fileStream);
      Files = deserialized.Files;
      fileStream.Close();
    }
  }
}
