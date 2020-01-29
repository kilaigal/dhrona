using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionBackButton : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("ChapterSelect");
    }
}