using UnityEngine;

//[System.Serializable]
public class MathProblem
{
    public int operand1;
    public int operand2;
    public string operationSymbol;
    public int correctAnswer;

    public MathProblem()
    {
        operand1 = Random.Range(1, 11); // Zufällige Auswahl des ersten Operanden (1 bis 10)
        operand2 = Random.Range(1, 11); // Zufällige Auswahl des zweiten Operanden (1 bis 10)

        // Zufällige Auswahl der Operation (+, -, x, ÷)
        int operation = Random.Range(0, 4); // 0: Addition, 1: Subtraktion, 2: Multiplikation, 3: Division

        switch (operation)
        {
            case 0:
                operationSymbol = "+";
                correctAnswer = operand1 + operand2;
                break;
            case 1:
                operationSymbol = "-";
                int maxOperand = Mathf.Max(operand1, operand2);
                int minOperand = Mathf.Min(operand1, operand2);
                correctAnswer = maxOperand - minOperand;
                break;
            case 2:
                operationSymbol = "x";
                correctAnswer = operand1 * operand2;
                break;
            case 3:
                operationSymbol = "÷";
                correctAnswer = operand1;
                operand1 = operand1 * operand2; // Sicherstellen, dass das Ergebnis der Division korrekt ist
                break;
            default:
                operationSymbol = "?";
                correctAnswer = 0;
                break;
        }
    }
}
