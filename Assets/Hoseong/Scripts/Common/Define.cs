using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDatabase<T> where T : IData
{
    void ClearDatabase();
    void AddData(T data);
    T GetData(int idx);
}

public interface IData
{
    void SetData(Dictionary<string, object> parsedData);
}
