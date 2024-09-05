using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MathProblemManager : MonoBehaviour
{
    public TMP_Text problemText;
    public TMP_Text timerText;
    public TMP_Text resultText;
    public float problemDisplayTime = 5f;
    public float timerDuration = 10f;

    private MathProblemGenerator problemGenerator;
    private MathProblem currentProblem;
    private float timer;
    private bool showingResult;

    private void Start()
    {
        problemGenerator = GetComponent<MathProblemGenerator>();
        GenerateNewProblem();
        timer = timerDuration;
        showingResult = false;
    }

    private void Update()
    {
        if (!showingResult)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(timer).ToString();

            if (timer <= 0)
            {
                ShowResult();
            }
        }
    }

    public void GenerateNewProblem()
    {
        currentProblem = problemGenerator.GenerateRandomProblem();
        problemText.text = $"{currentProblem.operand1} {currentProblem.operationSymbol} {currentProblem.operand2}";
    }

    private void ShowResult()
    {
        resultText.text = currentProblem.correctAnswer.ToString();
        showingResult = true;
        Invoke(nameof(HideResult), problemDisplayTime);
    }

    private void HideResult()
    {
        resultText.text = "";
        GenerateNewProblem();
        timer = timerDuration;
        showingResult = false;
    }
}
