using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using System;
using System.Linq;


public class ChapterSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public ChapterClass[] allchaps;
    public int chap;
    //public int numberOfLevels = 50;
    public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;
    private readonly string gameDataFileName = "datachap.json";
    public ChapterClass[] allavailablechapters;
    public int sub;
    public ChapterClass[] currentSubChaplist;
    private int numberOfLevels;
    public string currentchaptitle;

    // Start is called before the first frame update
    void Start()
    {
        sub = PlayerPrefs.GetInt("sub", 702);
        LoadChapData();
        CurrentChaps(sub);
        numberOfLevels = currentSubChaplist.Length;
        if (numberOfLevels == 0)
        {
            Debug.Log("No Questions for this chapter");
            ReturntoMain();
            return;
        }
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
       // int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
        int maxInARow = 1;
        int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y));
        amountPerPage = maxInARow * maxInACol;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        LoadPanels(totalPages);
    }
    void LoadPanels(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfPanels;

        for (int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(0,panelDimensions.height * (i - 1)*-1);
            SetUpGrid(panel);
            int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
            LoadIcons(numberOfIcons,i, panel);
        }
        Destroy(panelClone);    
    }
    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.spacing = iconSpacing;

    }
    void LoadIcons(int numberOfIcons, int panelnum, GameObject parentObject)
    {
        for (int i = 1; i <= numberOfIcons; i++)
        {
            currentLevelCount++;    
            GameObject icon = Instantiate(levelIcon) as GameObject;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            icon.name = "Level " + (panelnum*i);
            int idx = ((panelnum - 1) * amountPerPage) + i - 1;
            icon.GetComponentInChildren<TextMeshProUGUI>().SetText(currentSubChaplist[((panelnum-1)*amountPerPage)+i - 1].ChapterTitle);
            icon.GetComponentInChildren<Button>().onClick.AddListener(() => OpenChap(idx));
        }
    }

    public void OpenChap(int index)
    {
        sub= currentSubChaplist[index].SubID;
        chap =currentSubChaplist[index].ChapterID;
        PlayerPrefs.SetInt("sub", sub);
        PlayerPrefs.SetInt("chap", chap);
        currentchaptitle= currentSubChaplist[index].ChapterTitle;
        PlayerStats.ChapTitle = currentchaptitle;
        UnityEngine.SceneManagement.SceneManager.LoadScene("QuestionSelect");

    }

    public void ReturntoMain()

    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    private void LoadChapData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameDataChap loadedData = JsonUtility.FromJson<GameDataChap>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allavailablechapters = loadedData.allChaps;

        }
        else
        {
            Debug.LogError("Cannot load chapter data!");
        }
    }


    private void CurrentChaps(int sub)

    {
        currentSubChaplist = Array.FindAll(allavailablechapters, c => c.SubID == sub);

    }


    // Update is called once per frame
    void Update()
    {

    }
}