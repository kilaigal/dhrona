using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public static class PlayerStats
    {
        private static int chap, sub;
        private static string chaptitle;
        private static Question[] allavailablequestions;
        private static PlayerStatsClass[] questionhistory;
        private static int curidx;
        private static int nextqid;
    public static int Chap
        {
            get
            {
                return chap;
            }
            set
            {
                chap = value;
            }
        }

    public static int Curidx
    {
        get
        {
            return curidx;
        }
        set
        {
            curidx = value;
        }
    }

    public static int Sub
        {
            get
            {
                return sub;
            }
            set
            {
                sub = value;
            }
        }
    public static int NextQID
    {
        get
        {
            return nextqid;
        }
        set
        {
            nextqid = value;
        }
    }

    public static string ChapTitle
        {
            get
            {
                return chaptitle;
            }
            set
            {
                 chaptitle = value;
        }
        }

    public static Question[] AllAvailableQuestions
    {
        get
        {
            return allavailablequestions;
        }
        set
        {
            allavailablequestions = value;
        }
    }

    public static PlayerStatsClass[] QuestionHistory
    {
        get
        {
            return questionhistory;
        }
        set
        {
            questionhistory = value;
        }
    }


}

