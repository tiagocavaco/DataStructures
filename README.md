# .Net 6 - Data structures

## KeyValueStore
```c#
T Get<T>(object key);

void Set(object key, object value);

void Remove(object key);

void Undo();

void Redo();

int Count;
```

