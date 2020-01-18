using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ContinueGame()
    {
        SceneManager.LoadScene("Question");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("sub", 1);
        PlayerPrefs.SetInt("chap", 1);
        SceneManager.LoadScene("Question");
    }

}
