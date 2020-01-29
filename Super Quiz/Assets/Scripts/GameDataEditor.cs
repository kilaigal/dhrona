using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class GameDataEditor : EditorWindow
{

    public GameData gameData;
    public GameDataSub gameDataSub;
    public GameDataChap gameDataChap;

    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    private string gameDataProjectFilePathSub = "/StreamingAssets/datasub.json";
    private string gameDataProjectFilePathChap = "/StreamingAssets/datachap.json";

    [MenuItem("Window/Game Data Editor")]
    static void Init() => EditorWindow.GetWindow(typeof(GameDataEditor)).Show();


    Vector2 scrollPos;
        

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            scrollPos =
                EditorGUILayout.BeginScrollView(scrollPos);
            if (gameData != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");
                EditorGUILayout.PropertyField(serializedProperty, true);

                // Sub
                SerializedObject serializedObject1 = new SerializedObject(this);
                SerializedProperty serializedProperty1 = serializedObject.FindProperty("gameDataSub");
                EditorGUILayout.PropertyField(serializedProperty1, true);

                // Chap
                SerializedObject serializedObject2 = new SerializedObject(this);
                SerializedProperty serializedProperty2 = serializedObject.FindProperty("gameDataChap");
                EditorGUILayout.PropertyField(serializedProperty2, true);



                serializedObject.ApplyModifiedProperties();

                if (GUILayout.Button("Save data"))
                {
                    SaveGameData();
                }
            }

            if (GUILayout.Button("Load data"))
            {
                LoadGameData();
            }
        EditorGUILayout.EndScrollView();

    }



    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            gameData = new GameData();
        }
        // Subject

        string filePath1 = Application.dataPath + gameDataProjectFilePathSub;

        if (File.Exists(filePath1))
        {
            string dataAsJson = File.ReadAllText(filePath1);
            gameDataSub = JsonUtility.FromJson<GameDataSub>(dataAsJson);
        }
        else
        {
            gameDataSub = new GameDataSub();
        }

        // Chapter

        string filePath2 = Application.dataPath + gameDataProjectFilePathChap;

        if (File.Exists(filePath2))
        {
            string dataAsJson = File.ReadAllText(filePath2);
            gameDataChap = JsonUtility.FromJson<GameDataChap>(dataAsJson);
        }
        else
        {
            gameDataChap = new GameDataChap();
        }


    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

        // Subject
        string dataAsJson1 = JsonUtility.ToJson(gameDataSub);

        string filePath1 = Application.dataPath + gameDataProjectFilePathSub;
        File.WriteAllText(filePath1, dataAsJson1);


        // Chapter
        string dataAsJson2 = JsonUtility.ToJson(gameDataChap);

        string filePath2 = Application.dataPath + gameDataProjectFilePathChap;
        File.WriteAllText(filePath2, dataAsJson2);

    }
}