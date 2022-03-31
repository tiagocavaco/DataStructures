using System.Collections;

namespace DataStructures
{
  public class KeyValueStore
  {
    private readonly Hashtable store;

    private readonly Stack<KeyValueStoreLogEntry> undoStack;

    private readonly Stack<KeyValueStoreLogEntry> redoStack;

    public KeyValueStore()
    {
      store = new Hashtable();
      undoStack = new Stack<KeyValueStoreLogEntry>();
      redoStack = new Stack<KeyValueStoreLogEntry>();
    }

    public int Count { get => store.Count; }

    public T Get<T>(object key)
    {
      return (T)store[key];
    }

    public void Set(object key, object value)
    {
      LogAction(key, KeyValueStoreAction.SET);

      store[key] = value;
    }

    public void Remove(object key)
    {
      if (store.ContainsKey(key))
      {
        LogAction(key, KeyValueStoreAction.REMOVE);

        store.Remove(key);
      }
    }

    public void Undo()
    {
      if (undoStack.Count > 0)
      {
        var lastAction = undoStack.Pop();

        var currentValue = store[lastAction.Key];

        switch (lastAction.Action)
        {
          case KeyValueStoreAction.SET:
            {
              if (lastAction.LastValue is not null)
              {              
                store[lastAction.Key] = lastAction.LastValue;
              }
              else
              {
                store.Remove(lastAction.Key);
              }

              break;
            }
          case KeyValueStoreAction.REMOVE:
            {
              store[lastAction.Key] = lastAction.LastValue;
              break;
            }
        }

        lastAction.LastValue = currentValue;

        redoStack.Push(lastAction);
      }
    }

    public void Redo()
    {
      if (redoStack.Count > 0)
      {
        var lastEntry = redoStack.Pop();

        var currentValue = store[lastEntry.Key];

        switch (lastEntry.Action)
        {
          case KeyValueStoreAction.SET:
            {
              store[lastEntry.Key] = lastEntry.LastValue;

              break;
            }
          case KeyValueStoreAction.REMOVE:
            {
              store.Remove(lastEntry.Key);
              break;
            }
        }

        lastEntry.LastValue = currentValue;

        undoStack.Push(lastEntry);
      }
    }

    private void LogAction(object key, KeyValueStoreAction action)
    {
      undoStack.Push(new KeyValueStoreLogEntry
      {
        Key = key,
        LastValue = store[key],
        Action = action
      });

      redoStack.Clear();
    }
  }

  internal class KeyValueStoreLogEntry
  {
    public object Key { get; set; }

    public object LastValue { get; set; }

    public KeyValueStoreAction Action { get; set; }
  }

  internal enum KeyValueStoreAction
  {
    SET,
    REMOVE
  }
}