using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Pairs : MonoBehaviour
{
    public bool active; // true when testing
    float targetTime = 3.0f;
    [SerializeField] TextMeshProUGUI[] leftPairs;
    [SerializeField] TextMeshProUGUI[] rightPairs;
    String[] pairsA = new String[] { "Peanut Butter", "Rice", "Bonnie", "Salt", "Sun", "You", "Rock" };
    String[] pairsB = new String[] { "Jelly", "Beans", "Clyde", "Pepper", "Moon", "Me", "Roll" };
    [SerializeField] String[] left = new String[3], right = new String[3];

    void Start()
    {
        PairGenerator();
        //ShufflePairs(pairsA);
    }

    void Update()
    {
        if (active && !this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(active);
        }

        else if (!active && this.gameObject.activeInHierarchy)
            this.gameObject.SetActive(active);
    }

    void PairGenerator()
    {
        RandomizeText(pairsA, pairsB, left, right);
        ShufflePairs(left, leftPairs);
        ShufflePairs(right, rightPairs);
    }


    void RandomizeText(String[] textA, String[] textB, String[] sideA, String[] sideB)
    {
        for (int t = 0; t < textA.Length; t++)
        {
            String tmpA = textA[t];
            String tmpB = textB[t];
            int r = UnityEngine.Random.Range(t, textA.Length);

            textA[t] = textA[r];
            textB[t] = textB[r];

            if (t > 2) break;

            sideA[t] = textA[t];
            sideB[t] = textB[t];

            //Debug.Log(sideA[t] + "  " + sideB[t]);

            textA[r] = tmpA;
            textB[r] = tmpB;
        }
    }


    void ShufflePairs(String[] side, TextMeshProUGUI[] pair)
    {
        for (int t = 0; t < 3; t++)
        {
            String tmp = side[t];
            int r = UnityEngine.Random.Range(t, side.Length);
            side[t] = side[r];
            pair[t].text = side[t];
            side[r] = tmp;
        }
    }
}

