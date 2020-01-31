using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionBackButton : MonoBehaviour
{
    public void NextScene()
    {
        PlayerStatsClass[] allhistory = PlayerStats.QuestionHistory;

        if (allhistory != null)
        {
            for (int i = 0; i < allhistory.Length; i++)
            {
                PlayerStats.QuestionHistory[i].isQCurrent = 0;  // setting all other values 0.
            }
        }

             PlayerStats.QuestionHistory[PlayerStats.Curidx].isQCurrent = 1;
            SceneManager.LoadScene("QuestionSelect");
    }
}