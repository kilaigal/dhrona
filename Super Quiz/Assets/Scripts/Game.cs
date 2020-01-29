using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;                                                        // The System.IO namespace contains functions related to loading and saving files
using System;
using System.Linq;

public class Game : MonoBehaviour
{
    public QuestionDatabase questionDatabase;
    private int sub;
    private int chap;
    private Question currentQuestion;
    private int currentQuestionIndex;
    [SerializeField]
    private Transform questionPanel;
    [SerializeField]
    private Transform answerPanel;
    

    [SerializeField]
    private Transform scoreScreen, questionScreen;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreStats, scorePercentage;

    private PlayerProgress playerProgress;
    private Question[] questions;
    public Question[] allavailablequestions;
    private readonly string gameDataFileName = "data.json";

    public int next_Question_index;
    private int q_id;

    private int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        sub = PlayerPrefs.GetInt("sub", 1);
        chap = PlayerPrefs.GetInt("chap", 1);
        LoadGameData();
        LoadPlayerProgress();
        LoadQuestionSet();
        if (questions.Length != 0)
        {
            UseQuestionTemplate(currentQuestion.questionType);
        }
    }

    void LoadQuestionSet()
    {
        //hard coded initializing. Will be modified by user selection in previous scenes.
        //sub = 702;
        //chap = 70203;
        questions = GetQuestionSubset();

        if (questions.Length == 0)
        {
            Debug.Log("No Questions for this chapter");
            ReturntoMain();
            return;
        }

        q_id = questions[0].questionID;   // Initializing to first question available.
        // Will be modified to provide questionID of the correct level difficulty question. 
        
        //Need a function that would take a question id and spit out it's index in question array
        next_Question_index = NextQuestionIdx(q_id);
        // Initial Question
        currentQuestion = questions[next_Question_index];
    }

    public void ReturntoMain()

    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    void ClearAnswers()
    {
        foreach (Transform buttons in answerPanel)
        {
            Destroy(buttons.gameObject);
        }
        
    }
    public void SubmitNewPlayerScore(int newScore)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
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

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }
    void UseQuestionTemplate(Question.QuestionType questionType)
    {
        for (int i = 0; i < questionPanel.childCount; i++)
        {
            questionPanel.GetChild(i).gameObject.SetActive(i == (int)questionType);
            if (i == (int)questionType)
            {
                questionPanel.GetChild(i).GetComponent<QuestionUI>().UpdateQuestionInfo(currentQuestion);
            }
        }
    }

    void NextQuestion()
    {
        if (currentQuestionIndex < questions.Length-1)
        {
            currentQuestionIndex++;
            q_id = questions[currentQuestionIndex].questionID;   // Initializing to next question in array.
             //Will be modified to take input from a function that spits out questionID of the next question to ask.
            next_Question_index = NextQuestionIdx(q_id);
            // Initial Question

            currentQuestion = questions[next_Question_index];
            UseQuestionTemplate(currentQuestion.questionType);
        }
        else
        {
            scoreScreen.gameObject.SetActive(true);
            questionScreen.gameObject.SetActive(false);
            scorePercentage.text = string.Format("Score:\n{0}%", (float)correctAnswers/(float)questions.Length * 100);
            scoreStats.text = string.Format("Questions: {0}\nCorrect: {1}", questions.Length, correctAnswers);
        }
    }

    public void CheckAnswer(string answer)
    {
        if (answer == currentQuestion.correctAnswerKey)
        {
            correctAnswers++;
            Debug.Log("That's correct!");
        }

        ClearAnswers();
        NextQuestion();
        
    }

public int NextQuestionIdx(int q_id)
    {
        var next_Question_index = Array.FindIndex(questions, row => row.questionID == q_id);

        if (next_Question_index != -1)
        { return next_Question_index; }
        else
        {
            Debug.LogError("Cannot find question!");
            next_Question_index = 0;
            return next_Question_index;  
        }
    }



    public Question[] GetQuestionSubset()

    {
       
        Question[] questions = Array.FindAll(allavailablequestions,c => c.questionSub == sub && c.questionChapter == chap);
        return questions;

    }


}
