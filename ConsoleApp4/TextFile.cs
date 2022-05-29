using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


namespace ConsoleApp4 {
  [Serializable]
  public class TextFile : IOriginator {
    public string Name { get; set; }
    public string[] KeyWords { get; set; } = new string[2];
    public string Content { get; set; }

    public TextFile(string name) {
      Name = name;
    }

    public TextFile(string name, string[] keyWords) {
      Name = name;
      KeyWords[0] = keyWords[0];
      KeyWords[1] = keyWords[1];
    }

    public void SerializeBinary(FileStream fileStream) {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      binaryFormatter.Serialize(fileStream, this);
      fileStream.Close();
    }

    public void DeserializeBinary(FileStream fileStream) {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      TextFile deserialized = (TextFile)binaryFormatter.Deserialize(fileStream);
      Name = deserialized.Name;
      KeyWords = deserialized.KeyWords;
      fileStream.Close();
    }

    public void SerializeXml(FileStream fileStream) {
      XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
      xmlSerializer.Serialize(fileStream, this);
      fileStream.Close();
    }

    public void DeserializeXml(FileStream fileStream) {
      XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
      TextFile deserialized = (TextFile)xmlSerializer.Deserialize(fileStream);
      Name = deserialized.Name;
      KeyWords = deserialized.KeyWords;
      fileStream.Close();
    }

    object IOriginator.GetMemento() {
      return new Memento
      { Content = this.Content };
    }
    
    void IOriginator.SetMemento(object memento) {
      if (memento is Memento) {
        var mem = memento as Memento;
        Content = mem.Content;
      }
    }
  }
}