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

    private Question[] allRoundData;
    private PlayerProgress playerProgress;
    private Question[] questions;

    private string gameDataFileName = "data.json";

    private int correctAnswers;
    // Start is called before the first frame update
    void Start()
    {
        sub = PlayerPrefs.GetInt("sub", 1);
        chap = PlayerPrefs.GetInt("chap", 1);
        LoadGameData();
        LoadPlayerProgress();
        LoadQuestionSet();
        UseQuestionTemplate(currentQuestion.questionType);
    }

    void LoadQuestionSet()
    {
        // Initial Question
        currentQuestion = questions[0];
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
            questions = loadedData.allRoundData;
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
            currentQuestion = questions[currentQuestionIndex];
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
}
