using System;

namespace ConsoleApp4 { 
  public interface IOriginator { 
    object GetMemento();
    void SetMemento (object memento);
  }
}