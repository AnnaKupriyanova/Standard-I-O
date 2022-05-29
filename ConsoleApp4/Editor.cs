using System;

namespace ConsoleApp4 { 
  public static class TextEditor { 
    private static Search CreateSearcher() {
      Search searcher;
      while (true) {
        Console.WriteLine("Enter directory:");
        var directory = Console.ReadLine();

        try { 
          searcher = new Search(directory);
          break;
        }
        catch (Exception exception){
          Console.WriteLine(exception.Message);
          Console.WriteLine("Try again.");
          Console.WriteLine("\n");
        }

      }
      return searcher;
    }

    public static void FindFile(Search searcher) { 
      Console.Clear();
      Console.WriteLine("Show all files in directory   0");
      Console.WriteLine("Search file by key words      1");
      Console.WriteLine("Add key words to file         2");
      Console.WriteLine("Exit                          3");
      Console.WriteLine("\n");
      
      while (true) {
        switch (Console.ReadLine()) {
          case "0":
            searcher.GetFiles();
            break;

          case "1":
            var fileStream = new FileStream($"{searcher.DirectoryName + "/search.bin"}", FileMode.OpenOrCreate, FileAccess.Read);
            if (fileStream.Length > 0) {
              searcher.Deserialize(fileStream);
              fileStream.Close();
            } else { 
              Console.WriteLine("No files found.");
              fileStream.Close();
              break;
            }

            Console.WriteLine("Enter key words:");
            string[] keyWords = new string[2];
            keyWords[0] = Console.ReadLine();
            keyWords[1] = Console.ReadLine();
            
            try { 
              searcher.SearchFile(keyWords); 
              break;
            }
            catch (Exception exception) {
              Console.WriteLine(exception.Message);
              break;
            }

          case "2":
            var fileDirectory = new FileStream($"{searcher.DirectoryName + "/search.bin"}", FileMode.OpenOrCreate, FileAccess.Read);
            if (fileDirectory.Length > 0) {
              searcher.Deserialize(fileDirectory);       
            }
            fileDirectory.Close();

            Console.WriteLine("Enter file name:");
            var name = Console.ReadLine();

            try { 
              searcher.AddFile(name);         
            }
            catch (Exception exception) {
              Console.WriteLine(exception.Message);
              break;
            }

            fileDirectory = new FileStream($"{searcher.DirectoryName + "/search.bin"}", FileMode.OpenOrCreate, FileAccess.Write);
            searcher.Serialize(fileDirectory);
            fileDirectory.Close();
            break;

            case "3":
              return;

            default:
            Console.WriteLine("Error.");
            break;
        }
        
        Console.WriteLine("\n");
      }
    }

    private static void Editor(Search searcher) { 
      Console.WriteLine("Enter file name:");
      var name = Console.ReadLine();
      name = searcher.DirectoryName + "/" + name;
      
      if (!File.Exists(name)) {
        var file = File.Create(name);
        file.Close();
      }

      Console.Clear();

      var textFile = new TextFile(name);
      var textReader = new StreamReader(textFile.Name);
      textFile.Content = textReader.ReadToEnd();
      Console.WriteLine(textFile.Content);
      textReader.Close();

      using (var textWriter = new StreamWriter(textFile.Name)) {
        Console.WriteLine("Entere 'stop' to stop.\n");
        var key = new ConsoleKeyInfo();
        var text = "";
        var caretaker = new Caretaker();

        while (true) { 
          caretaker.SaveState(textFile);
          text += Console.ReadLine();
          textFile.Content += $"\n{text}";

          if (text == "stop") { 
            break;          
          }

          key = Console.ReadKey();

          if (key.Key == ConsoleKey.Escape) { 
            caretaker.RestoreState(textFile);
            Console.Clear();
            Console.WriteLine(textFile.Content);
            text = "";
          } else if (key.Key == ConsoleKey.Escape) { 
            textWriter.WriteLine(text);
            textWriter.Flush();
            Console.WriteLine("Editing finished.");
            return;
          } else { 
            textWriter.WriteLine(text);
            textWriter.Flush();
            text = key.Key.ToString().ToLower();
          }
        }
      }
    }

    public static void Menu() { 
      var searcher = CreateSearcher();
      
      while (true) {
        Console.Clear();
        Console.WriteLine("Text editor     0");
        Console.WriteLine("Searcher        1");
        Console.WriteLine("Exit            2");
        
        switch (Console.ReadLine()) {
          case "0":
            Console.Clear();
            Editor(searcher);
            break;

          case "1":
            Console.Clear();
            FindFile(searcher);
            break;

          case "2":
            return;
          
          default:
            Console.WriteLine("Error.");
            break;
        }
      }
    }
  }
}
