using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using System;
using System.Linq;


public class SubSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;
    public SubjectClass[] allsubjects;
    public int sub;
    //public int numberOfLevels = 50;
    public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;
    private readonly string gameDataFileName = "datasub.json";
    private int numberOfLevels;

    // Start is called before the first frame update
    void Start()
    {
        LoadsubData();
        numberOfLevels = allsubjects.Length;
        if (numberOfLevels == 0)
        {
            Debug.Log("No Subjects for this grade");
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
            icon.GetComponentInChildren<TextMeshProUGUI>().SetText(allsubjects[((panelnum-1)*amountPerPage)+i - 1].SubTitle);
            icon.GetComponentInChildren<Button>().onClick.AddListener(() => OpenChap(idx));
        }
    }

    public void OpenChap(int index)
    {
        sub= allsubjects[index].SubID;
        PlayerPrefs.SetInt("sub", sub);
        PlayerStats.Sub = sub;
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChapterSelect");

    }

    public void ReturntoMain()

    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    private void LoadsubData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameDataSub loadedData = JsonUtility.FromJson<GameDataSub>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allsubjects = loadedData.allsubs;

        }
        else
        {
            Debug.LogError("Cannot load chapter data!");
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}