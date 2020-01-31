using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

// On click of a chest or on answering a question, this function gets called.
// This function takes the curidx and checks if there exists history at that index
// for this chap and subject subset. If exists, it gets the qid of that entry.
// Else gets the qid for the next question and sends it. 


public static class NextQID
{

    public static int sub = PlayerStats.Sub;
    public static int chap = PlayerStats.Chap;
    public static Question[] allavailablequestions;
    public static Question[] questions;
    public static PlayerStatsClass[] allhistory;
    public static PlayerStatsClass[] history;

    public static void getnextid()
    {

        
    int chestidx = PlayerStats.Curidx;
   

        questions = GetQuestionSubset();
        PlayerStatsClass[] allhistory = PlayerStats.QuestionHistory;

        if (allhistory != null)
        {
            for (int i = 0; i < allhistory.Length - 1; i++)
            {
                allhistory[i].isQCurrent = 0;  // setting all other values 0.
            }

            PlayerStatsClass[] history = Array.FindAll(allhistory, c => c.sub == sub && c.chap == chap);
            if (history != null)
            {
                if (history.Length == 0 || history.Length < chestidx+1)
                {
                    PlayerStatsClass newentry = new PlayerStatsClass { chap = chap, sub = sub, isQCurrent = 1, staron = 0 };
                    newentry.qid = questions[chestidx].questionID;
                    // Will be modified to provide questionID of the correct level difficulty question.
                    Array.Resize(ref allhistory, allhistory.Length+1);
                    allhistory[allhistory.Length - 1] = newentry;
                    PlayerStats.QuestionHistory = allhistory;
                    PlayerStats.NextQID = newentry.qid;
                }
                else // chestidx is within history
                {
                    history[chestidx].isQCurrent = 1;
                    PlayerStats.NextQID = history[chestidx].qid;
                }


            }
            else // if no history exists for this chap, append to history.

            {
                PlayerStatsClass newentry = new PlayerStatsClass { chap = chap, sub = sub, isQCurrent = 1, staron = 0 };
                newentry.qid = questions[chestidx].questionID;
                // Will be modified to provide questionID of the correct level difficulty question.
                PlayerStats.QuestionHistory = new PlayerStatsClass[1];
                PlayerStats.QuestionHistory[0] = newentry;
                PlayerStats.NextQID = newentry.qid;
            }
        }
        else   // QuestionHistory is empty. Add this entry as Question History.
        {

            PlayerStatsClass newentry = new PlayerStatsClass { chap = chap, sub = sub,isQCurrent=1,staron=0 };
            newentry.qid = questions[chestidx].questionID;
            // Will be modified to provide questionID of the correct level difficulty question.
            PlayerStats.QuestionHistory  = new PlayerStatsClass[1];
            PlayerStats.QuestionHistory[0] = newentry;
            PlayerStats.NextQID = newentry.qid;

        }
    }


    public static Question[] GetQuestionSubset()

    {
        
        allavailablequestions = PlayerStats.AllAvailableQuestions;
        Question[] questions = Array.FindAll(allavailablequestions, c => c.questionSub == sub && c.questionChapter == chap);
        return questions;

    }


    public static PlayerStatsClass[] GetHistory()

    {
        chap = PlayerStats.Chap;
        sub = PlayerStats.Sub;

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


    public static PlayerStatsClass[] GetAllHistory()

    {
        PlayerStatsClass[] allhistory = PlayerStats.QuestionHistory;
       
            return allhistory;
        

    }


}
