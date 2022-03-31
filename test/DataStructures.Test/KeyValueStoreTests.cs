using Xunit;

namespace DataStructures.Test
{
  public class KeyValueStoreTests
  {
    [Fact]
    public void Get_NonExistingKey_ReturnNull()
    {
      // arrange
      var store = new KeyValueStore();

      // act
      var nullValue = store.Get<string>("Key");

      // assert
      Assert.Equal(0, store.Count);
      Assert.Null(nullValue);
    }

    [Fact]
    public void Get_ExistingKey_ReturnValue()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");

      // act
      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", firstValue);
    }

    [Fact]
    public void Set_MultipleKeys_SetKeys()
    {
      // arrange
      var store = new KeyValueStore();

      // act
      store.Set("FirstKey", "FirstValue");
      store.Set("SecondKey", "SecondValue");

      var firstValue = store.Get<string>("FirstKey");
      var secondValue = store.Get<string>("SecondKey");

      // assert
      Assert.Equal(2, store.Count);
      Assert.Equal("FirstValue", firstValue);
      Assert.Equal("SecondValue", secondValue);
    }

    [Fact]
    public void Set_ExistingKey_UpdateValue()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");

      // act
      store.Set("FirstKey", "SecondValue");

      var secondValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("SecondValue", secondValue);
    }

    [Fact]
    public void Remove_NonExistingKey_DoNothing()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");

      // act
      store.Remove("SecondKey");

      // assert
      Assert.Equal(1, store.Count);
    }

    [Fact]
    public void Remove_ExistingKey_RemoveKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");

      // act
      store.Remove("FirstKey");

      // assert
      Assert.Equal(0, store.Count);
    }

    [Fact]
    public void Undo_NoActions_DoNothing()
    {
      // arrange
      var store = new KeyValueStore();

      // act
      store.Undo();

      // assert
      Assert.Equal(0, store.Count);
    }

    [Fact]
    public void Undo_SetAction_RemoveKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");

      // act
      store.Undo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(0, store.Count);
      Assert.Null(firstValue);
    }

    [Fact]
    public void Undo_SetActionExisting_UpdateValue()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");

      // act
      store.Undo();

      var secondValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", secondValue);
    }

    [Fact]
    public void Undo_RemoveAction_SetKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Remove("FirstKey");

      // act
      store.Undo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", firstValue);
    }

    [Fact]
    public void Redo_NoActions_DoNothing()
    {
      // arrange
      var store = new KeyValueStore();

      // act
      store.Redo();

      // assert
      Assert.Equal(0, store.Count);
    }

    [Fact]
    public void Redo_UndoSetAction_SetKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Undo();

      // act
      store.Redo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", firstValue);
    }

    [Fact]
    public void Redo_UndoSetActionExisting_UpdateValue()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Undo();

      // act
      store.Redo();

      var secondValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("SecondValue", secondValue);
    }

    [Fact]
    public void Redo_UndoRemoveAction_RemoveKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Remove("FirstKey");
      store.Undo();

      // act
      store.Redo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(0, store.Count);
      Assert.Null(firstValue);
    }

    [Fact]
    public void Undo_EntireStore_KeepOneKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Set("SecondKey", "SecondValue");
      store.Set("ThirdKey", "ThirdValue");
      store.Remove("SecondKey");
      store.Set("FourthKey", "FourthValue");

      // act
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", firstValue);
    }

    [Fact]
    public void Redo_UndoEntireStore_KeepAllKeys()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Set("SecondKey", "SecondValue");
      store.Set("ThirdKey", "ThirdValue");
      store.Remove("SecondKey");
      store.Set("FourthKey", "FourthValue");
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();

      // act
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();

      var firstValue = store.Get<string>("FirstKey");
      var secondValue = store.Get<string>("SecondKey");
      var thirdValue = store.Get<string>("ThirdKey");
      var fourthValue = store.Get<string>("FourthKey");

      // assert
      Assert.Equal(3, store.Count);
      Assert.Equal("SecondValue", firstValue);
      Assert.Null(secondValue);
      Assert.Equal("ThirdValue", thirdValue);
      Assert.Equal("FourthValue", fourthValue);
    }

    [Fact]
    public void Undo_Redo_UndoEntireStore_KeepOneKey()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Set("SecondKey", "SecondValue");
      store.Set("ThirdKey", "ThirdValue");
      store.Remove("SecondKey");
      store.Set("FourthKey", "FourthValue");
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();
      store.Redo();

      // act
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();
      store.Undo();

      var firstValue = store.Get<string>("FirstKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("FirstValue", firstValue);
    }

    [Fact]
    public void SetKeyAfterUndo_ResetRedo_RedoDoesNothing()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Set("SecondKey", "SecondValue");
      store.Set("ThirdKey", "ThirdValue");
      store.Undo();
      store.Set("FourthKey", "FourthValue");

      // act
      store.Redo();

      var firstValue = store.Get<string>("FirstKey");
      var secondValue = store.Get<string>("SecondKey");
      var thirdValue = store.Get<string>("ThirdKey");
      var fourthValue = store.Get<string>("FourthKey");

      // assert
      Assert.Equal(3, store.Count);
      Assert.Equal("SecondValue", firstValue);
      Assert.Equal("SecondValue", secondValue);
      Assert.Null(thirdValue);
      Assert.Equal("FourthValue", fourthValue);
    }

    [Fact]
    public void RemoveKeyAfterUndo_ResetRedo_RedoDoesNothing()
    {
      // arrange
      var store = new KeyValueStore();

      store.Set("FirstKey", "FirstValue");
      store.Set("FirstKey", "SecondValue");
      store.Set("SecondKey", "SecondValue");
      store.Set("ThirdKey", "ThirdValue");
      store.Undo();
      store.Remove("SecondKey");

      // act
      store.Redo();

      var firstValue = store.Get<string>("FirstKey");
      var secondValue = store.Get<string>("SecondKey");
      var thirdValue = store.Get<string>("ThirdKey");

      // assert
      Assert.Equal(1, store.Count);
      Assert.Equal("SecondValue", firstValue);
      Assert.Null(secondValue);
      Assert.Null(thirdValue);
    }
  }
}