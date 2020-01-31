using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ContinueGame()
    {
        SceneManager.LoadScene("SubjectSelect");
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("sub", 702);
        PlayerPrefs.SetInt("chap", 70201);
        SceneManager.LoadScene("SubjectSelect");
    }

}
