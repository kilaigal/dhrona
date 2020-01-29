using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using System;
using System.Linq;


public class QuestionSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject thisCanvas;
    public GameObject totalcontainer;
    public GameObject bckgrndImage;
    public GameObject sidepanel;
    public Question[] questions;
    static Question[] allavailablequestions;
    public TextMeshProUGUI questiontext;
    public int chap;
    //public int numberOfLevels = 50;
    //public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect TotalDimensions;
    private int amountPerPage;
    public float bkgheight;
    public float bkgwidth;
    private readonly string gameDataFileName = "data.json";
    public int sub;
    private int numberOfLevels;
    static public string currentchaptitle;
    private float scalerat;

    // Start is called before the first frame update
    void Start()
    {
        sub = PlayerPrefs.GetInt("sub", 702);
        chap = PlayerPrefs.GetInt("chap", 70201);
        questiontext.text= PlayerStats.ChapTitle;
        LoadGameData();
        questions = GetQuestionSubset();
        numberOfLevels = questions.Length;
        if (numberOfLevels == 0)
        {
            Debug.Log("No Questions for this chapter");
            ReturntoMain();
            return;
        }
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        amountPerPage = 18;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        int screenW = Screen.width;
        //float orig_aspect = 2583f / 5556.706f;
        bkgwidth = bckgrndImage.GetComponent<RectTransform>().rect.width;
        scalerat = screenW/bkgwidth;
        totalcontainer.GetComponent<RectTransform>().localScale = new Vector3(scalerat, scalerat, 0);
        //panelDimensions.width = screenW;
        //levelHolder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,bkgheight);
        //TotalDimensions = totalcontainer.GetComponent<RectTransform>().rect;
        //TotalDimensions.width = screenW;
        //float TotHeight = totalPages * panelDimensions.height;
        //totalcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotHeight);
        //totalcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenW);
        float TotY =  (totalPages * panelDimensions.height);
        totalcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotY);
        LoadPanels(totalPages);
        levelHolder.SetActive(false);
        Button[] buttons=totalcontainer.GetComponentsInChildren<Button>();
        Image[] img1 = buttons[0].GetComponentsInChildren<Image>();
        img1[3].gameObject.SetActive(false);

        for (int i = 0; i <= buttons.Length-1; i++)
        {
            Image[] img = buttons[i].GetComponentsInChildren<Image>();
            img[2].gameObject.SetActive(false);
        }



    }
    void LoadPanels(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiperQuestion swiper = totalcontainer.AddComponent<PageSwiperQuestion>();

        for (int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(0, panelDimensions.height * (i - 1)*-1);
            panel.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
            //SetUpGrid(panel);
            panel.transform.SetParent(totalcontainer.transform);
        
        }
        Destroy(panelClone);    
    }
    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(panelDimensions.width*scalerat, panelDimensions.height*scalerat);
        grid.childAlignment = TextAnchor.MiddleCenter;
        //grid.spacing = iconSpacing;

    }
    


    public void ReturntoMain()

    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }


    public Question[] GetQuestionSubset()

    {

        Question[] questions = Array.FindAll(allavailablequestions, c => c.questionSub == sub && c.questionChapter == chap);
        return questions;

    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allavailablequestions = loadedData.allRoundData;

        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

   
}