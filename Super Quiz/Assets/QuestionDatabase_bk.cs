using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


[System.Serializable]
public class QuestionDatabase
{
    public QuestionSet questionSet;
    public QuestionSet GetQuestionSet(int sub, int chap)
    {
        Debug.LogError(questionSet.questions.Count);

       
        return new QuestionSet();
    }
}

[System.Serializable]

public class QuestionSet
{
    public List<Question> questions;
}
