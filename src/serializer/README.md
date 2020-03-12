# SE code collection - Serializer

Набор классов и интерфейсов для выполнения сериализации/десериализации объектов.

Поддерживаются следующие типы данных:

* bool;
* byte;
* short;
* int;
* long;
* float;
* double;
* string;
* Vector3.

Ссылка в мастерской Steam: [TN-code-collection Serializator](https://steamcommunity.com/sharedfiles/filedetails/?id=2020520319).

## Описание

### `TN.SerializeStream`

Класс, описывающий базовый набор методов для потока де/сериализации.

### `TN.ISerializable`

Интерфейс для всех пользовательских классов, которые могут быть де/сериализованы через `TN.SerializeStream`.

### `TN.BinarySerializeStream`

Класс бинарного потока сериализации объектов.

### `TN.Formatter`

Выполняет сериализацию объектов, которые реализуют интерфейс `TN.ISerializable`.

## Example

```
...

class TestSerializeClass : TN.ISerializable
{
    public string StrVal = "";
    public uint UIntVal = 0;
    public List<ulong> ListULongVal = new List<ulong>();
    public TestSerializeClass() { }
    public TestSerializeClass(string strVal, uint uintVal, List<ulong> listULongVal)
    {
        StrVal = strVal;
        UIntVal = uintVal;
        ListULongVal = listULongVal;
    }

    public void Serialize(TN.SerializeStream outStream)
    {
        outStream.SetString(StrVal).SetInt((int)UIntVal);
        outStream.SetInt(ListULongVal.Count);
        for (var i = 0; i < ListULongVal.Count; i++)
        {
            outStream.SetLong((long)ListULongVal[i]);
        }
    }
    public void Deserialize(TN.SerializeStream inStream)
    {
        int uintVal = 0;
        inStream.GetString(out StrVal).GetInt(out uintVal);
        UIntVal = (uint)uintVal;
        int listCount = 0;
        inStream.GetInt(out listCount);
        long listVal = 0;
        for (var i = 0; i < listCount; i++)
        {
            inStream.GetLong(out listVal);
            ListULongVal.Add((ulong)listVal);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(StrVal);
        sb.Append(',');
        sb.Append(UIntVal);
        sb.Append(",{ ");
        for (var i = 0; i < ListULongVal.Count; i++)
        {
            sb.Append(ListULongVal[i]);
            sb.Append(' ');
        }
        sb.Append('}');
        return sb.ToString();
    }
}

public Program()
{

}

public void Save()
{

}

public void Main(string argument, UpdateType updateSource)
{
    TN.BinarySerializeStream stream = new TN.BinarySerializeStream();
    stream.SetLong(456233L).SetString("Test string").SetInt(89439)
          .SetVector3(new Vector3(1, 2, 3));
    long retVal = 0;
    string retString = "";
    int retInt = 0;
    Vector3 retV3;
    stream.GetLong(out retVal).GetString(out retString).GetInt(out retInt)
          .GetVector3(out retV3);
    Echo($"Test stream: out {retVal} {retString} {retInt} {retV3}");

    TN.Formatter fm = new TN.Formatter(
        new TN.BinarySerializeStream()
    );
    fm.Serialize(new TestSerializeClass("Test class", 1, new List<ulong> { 1, 2, 3, 4, 5, 6 }));
    TestSerializeClass testOutClass = new TestSerializeClass();
    fm.Deserialize(testOutClass);
    Echo($"Test class out {testOutClass.ToString()}");
}

```