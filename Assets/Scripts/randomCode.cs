using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class randomCode : MonoBehaviour
{

    public TMP_Text[] texts = new TMP_Text[3];
    public int maxNumber = 99;
    public static int[] correctNumbers = new int[3];

    public static int[] GetCorrectNumbers() { return correctNumbers; }

    void Start()
    {
        int halfMax = (int) Mathf.Ceil(maxNumber / 2f);

        correctNumbers[0] = Random.Range(0, maxNumber);
        texts[0].text = correctNumbers[0].ToString("00");

        for (int i = 1; i < texts.Length; i++) {

            int lastNum = int.Parse(texts[i - 1].text);
            int oppositeLastNum = (lastNum + halfMax) % maxNumber;

            if (lastNum < oppositeLastNum)
                correctNumbers[i] = Random.Range(oppositeLastNum, maxNumber);
            else
                correctNumbers[i] = Random.Range(0, oppositeLastNum);

            texts[i].text = correctNumbers[i].ToString("00");
        }

    }

}
