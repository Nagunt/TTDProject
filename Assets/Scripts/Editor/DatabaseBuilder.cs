using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DatabaseBuilder
{
    public static string rootDataPath = "Database";

    [MenuItem("Database/Build Monster DB")]
    public static void BuildElementDatabase()
    {
        DatabaseMaker<MonsterDatabase, MonsterData> databaseMaker = new DatabaseMaker<MonsterDatabase, MonsterData>();
        databaseMaker.Make(rootDataPath, "monsterDB", "monsterData", "Monster", "monster");
    }
}

public class DatabaseMaker<TDatabase, TData> where TDatabase : ScriptableObject, IDatabase<TData>
                                            where TData : ScriptableObject, IData
{
    private TDatabase database = null;

    public void Make(string rootDataPath, string databaseFileName, string csvFileName, string dataDirectoryName, string dataFilePrefix)
    {
        SetDatabase(rootDataPath, databaseFileName);
        SetData(rootDataPath, csvFileName, dataDirectoryName, dataFilePrefix);
    }

    public void SetDatabase(string rootDataPath, string databaseFileName)
    {
        database = Resources.Load<TDatabase>(string.Format("{0}/{1}", rootDataPath, databaseFileName));

        if (!database)
        {
            database = ScriptableObject.CreateInstance<TDatabase>();

            string databasePath = string.Format("Assets/Resources/{0}/{1}.asset", rootDataPath, databaseFileName);
            AssetDatabase.CreateAsset(database, databasePath);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[DB] SO is created at " + databasePath);
        }
        else
        {
            database.ClearDatabase();
        }
    }

    public void SetData(string rootDataPath, string csvFileName, string dataDirectoryName, string dataFilePrefix)
    {
        List<Dictionary<string, object>> parsedDataReference = CSVParser.Read(string.Format("{0}/CSVFiles/{1}", rootDataPath, csvFileName));

        // 데이터 디렉토리 체크
        string dataDirectoryPath = string.Format("Assets/Resources/{0}/{1}", rootDataPath, dataDirectoryName);
        if (!Directory.Exists(dataDirectoryPath))
        {
            Directory.CreateDirectory(dataDirectoryPath);
        }

        // 기존 파일 탐색
        TData[] unconnectedData = Resources.LoadAll<TData>(string.Format("{0}/{1}", rootDataPath, dataDirectoryName));
        for (int i = 0; i < unconnectedData.Length; i++)
        {
            database.AddData(unconnectedData[i]);
            database.GetData(i).SetData(parsedDataReference[i]);
            EditorUtility.SetDirty(database.GetData(i));
        }

        // 재사용할 데이터 파일 없으면 생성
        int charCount = parsedDataReference.Count;
        for (int i = unconnectedData.Length; i < charCount; i++)
        {
            TData newData = ScriptableObject.CreateInstance<TData>();
            newData.SetData(parsedDataReference[i]);

            string dataPath = string.Format("{0}/{1}_{2}.asset", dataDirectoryPath, dataFilePrefix, (i + 1).ToString());
            AssetDatabase.CreateAsset(newData, dataPath);
            EditorUtility.SetDirty(newData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            database.AddData(newData);
        }
        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[DB] Build Success");
    }
}