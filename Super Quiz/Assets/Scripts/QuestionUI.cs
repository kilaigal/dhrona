using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class QuestionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject answerButton;
    [SerializeField]
    private Transform answerPanel;
    private AnswerChoice[] ans_tmp;
    private string answer;
    private AnswerChoice[] crct_ans_tmp_array;
    private AnswerChoice[] combined_ans_array_tmp;
    private AnswerChoice[] combined_ans_array;

    public virtual void UpdateQuestionInfo(Question question)
    {
        question.answerChoices = question.answerChoices.OrderBy(answer => UnityEngine.Random.value).ToArray();
        ans_tmp = question.answerChoices;

        // Creating a new array of 4 answers including the correct ans in random order
        //
        crct_ans_tmp_array = new AnswerChoice[] { new AnswerChoice { answerID = question.correctAnswerID, answerChoice = question.correctAnswerKey } };


        // correct ans combined with all available options
        var combined_ans_array_tmp = crct_ans_tmp_array.Concat(ans_tmp).ToArray();



        if (combined_ans_array_tmp.Length < 3)  // Less than 4 options 
        {
            //Display in random order including answer
            combined_ans_array = combined_ans_array_tmp;
            combined_ans_array = combined_ans_array.OrderBy(answer => UnityEngine.Random.value).ToArray();

        }
        else
        {
              combined_ans_array = combined_ans_array_tmp.Take(4).ToArray();
              combined_ans_array = combined_ans_array.OrderBy(answer => UnityEngine.Random.value).ToArray();

        }


        for (int i = 0; i < combined_ans_array.Length; i++)
        {
            answer = combined_ans_array[i].answerChoice;
            if (string.IsNullOrEmpty(answer))
            {
                //Do not create answers
            }
            else
            {
                Transform answerButtonInstance = Instantiate(answerButton, answerPanel).transform;
                answerButtonInstance.GetComponent<AnswerButton>().SetAnswerButton(answer);
            }   

        }
    }
}
