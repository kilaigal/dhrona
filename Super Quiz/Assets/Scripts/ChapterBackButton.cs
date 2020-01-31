using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterBackButton : MonoBehaviour
{
    public void NextScene1()
    {
        SceneManager.LoadScene("SubjectSelect");
    }
    public void NextScene2()
    {
        SceneManager.LoadScene("ChapterSelect");
    }
}