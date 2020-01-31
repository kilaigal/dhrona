using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public enum QuestionType { Text = 0, ImageWithCaption = 1, Audio = 2 }
    public QuestionType questionType;
    public int questionID;
    public int questionSub;
    public int questionChapter;
    public float questionDifficulty;
    public string questionText;
    public Sprite questionImage;
    public AudioClip questionAudio;
    public float correctAnswerID;
    public string correctAnswerKey;
    public AnswerChoice[] answerChoices;
}
