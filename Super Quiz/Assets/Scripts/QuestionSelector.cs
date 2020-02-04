using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using System;
using System.Linq;
using UnityEngine.SceneManagement;


public class QuestionSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject thisCanvas;
    public GameObject totalcontainer;
    public GameObject bckgrndImage;
    public GameObject sidepanel;
    public Question[] questions;
    private Question[] allavailablequestions;
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
    private PlayerStatsClass[] history;
    private PlayerStatsClass newentry;


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
        amountPerPage = 8;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        int screenW = Screen.width;
        //float orig_aspect = 2583f / 5556.706f;
        bkgwidth = bckgrndImage.GetComponent<RectTransform>().rect.width;
        scalerat = screenW/bkgwidth;
        totalcontainer.GetComponent<RectTransform>().localScale = new Vector3(scalerat, scalerat, 0);
        float TotY =  (totalPages * panelDimensions.height);
        totalcontainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TotY);
        LoadPanels(totalPages);
        levelHolder.SetActive(false);
        Button[] buttons=totalcontainer.GetComponentsInChildren<Button>();
        history=NextQID.GetHistory();
        if (history == null)
        {
            Image[] img1 = buttons[0].GetComponentsInChildren<Image>();

            for (int i = 0; i <= buttons.Length - 1; i++)
            {
                Image[] img = buttons[i].GetComponentsInChildren<Image>();
                img[2].gameObject.SetActive(false); // star disabled for all questions
                img[4].gameObject.SetActive(false); // avatars disabled for all questions
                int copy = i;
                buttons[i].onClick.AddListener(() => ChestButtonClick(copy));
                buttons[i].gameObject.AddComponent<ChestHover>();
            }
            img1[3].gameObject.SetActive(false);// lock disabled for first question.
            img1[4].gameObject.SetActive(true);// Avatar on for first question.

        }
        // load question history and enable stars, current Q and locks based on question history.
        else
        {
            if (history.Length == 0)
            {
                Image[] img1 = buttons[0].GetComponentsInChildren<Image>();

                for (int i = 0; i <= buttons.Length - 1; i++)
                {
                    Image[] img = buttons[i].GetComponentsInChildren<Image>();
                    img[2].gameObject.SetActive(false); // star disabled for all questions
                    img[4].gameObject.SetActive(false); // avatars disabled for all questions
                    int copy = i;
                    buttons[i].onClick.AddListener(() => ChestButtonClick(copy));

                }
                img1[3].gameObject.SetActive(false);// lock disabled for first question.
                img1[4].gameObject.SetActive(true);// Avatar on for first question.

            }

            else
            {

                int histLen = history.Length;

                // leave lock on for everything  after history
                for (int i = 0; i <= buttons.Length - 1; i++)
                {
                    Image[] img = buttons[i].GetComponentsInChildren<Image>();
                    int copy = i;
                    buttons[i].onClick.AddListener(() => ChestButtonClick(copy));
                    img[2].gameObject.SetActive(false); // star disabled for all questions
                    img[4].gameObject.SetActive(false); // avatars disabled for all questions
                    if (i <= histLen - 1)
                    {
                        //

                        img[3].gameObject.SetActive(false);// lock disabled for cleared questions.

                        if (history[i].staron == 1)
                        {
                            img[2].gameObject.SetActive(true); // star enabled

                        }
                        else
                        {
                            img[2].gameObject.SetActive(false); // star disabled
                        }
                        if (history[i].isQCurrent == 1)
                        {
                            img[4].gameObject.SetActive(true); // avatar enabled

                        }
                        else
                        {
                            img[4].gameObject.SetActive(false); // avatar disabled
                        }
                    }

                }
            }
        }

        // Move canvas to have current question in view
        var curIdx = Array.FindIndex(history, row => row.isQCurrent == 1);

        int pos4 = 3770;  // for chests 9&10
        int off1 = Mathf.FloorToInt((curIdx) / 8);
        float totaloff = (off1 * pos4 + getoffset(curIdx + 1 - (off1 * 8)))*scalerat;

        totalcontainer.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, totaloff, 0);


        int getoffset(int diff)
        {
            int pos1 = 1050;  // for chests 3&4
            int pos2 = 1700;  // for chests 5&6
            int pos3 = 2500;  // for chests 7&8


            if (diff<5 && diff>2)  // 3-4
            { return pos1; }
            if (diff < 7 && diff > 4)  // 3-4
            { return pos2; }
            if (diff < 9 && diff > 6)  // 3-4
            { return pos3; }
            else
            { return 0; }

        }
    }

   
    private void ChestButtonClick(int chestidx)
    {
        Button[] buttons = totalcontainer.GetComponentsInChildren<Button>();
        Image[] img2 = buttons[chestidx].GetComponentsInChildren<Image>(true);
        bool isactive =img2[3].gameObject.activeSelf;

        if (isactive != true)   // lock is inactive
        {
            // if playerstats.history for this chapter has length more than the
            // idx, then open the corresponding idx in question scene. Else,
            // append a new entry in the total history tracker for this idx with
            // no star.
            PlayerStats.Curidx = chestidx;
            NextQID.getnextid();
            SceneManager.LoadScene("Question");

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
        allavailablequestions = PlayerStats.AllAvailableQuestions;
        Question[] questions = Array.FindAll(allavailablequestions, c => c.questionSub == sub && c.questionChapter == chap);
        return questions;

    }


    public PlayerStatsClass[] GetHistory()

    {
        PlayerStatsClass[] allhistory = PlayerStats.QuestionHistory;
        if (allhistory != null)
        {
            PlayerStatsClass[] history = Array.FindAll(allhistory, c => c.sub == sub && c.chap == chap);
            return history;
        }
        else
        {
            history = null;
            return history;
        }

    }

    private void LoadGameData()
    {
        allavailablequestions = PlayerStats.AllAvailableQuestions;
        //if playerstats was not populated before
        if (allavailablequestions == null)
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
                PlayerStats.AllAvailableQuestions = loadedData.allRoundData;
            }
            else
            {
                Debug.LogError("Cannot load game data!");
            }
        }
    }


}