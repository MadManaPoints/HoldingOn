using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Pairs : MonoBehaviour
{
    [SerializeField] GameObject display;
    public bool active; // true when testing
    float targetTime = 2.5f;
    [SerializeField] TextMeshProUGUI[] leftPairs, rightPairs;
    String[] pairsA = new String[] { "Peanut Butter", "Rice", "Bonnie", "Salt", "Sun", "You", "Rock" };
    String[] pairsB = new String[] { "Jelly", "Beans", "Clyde", "Pepper", "Moon", "Me", "Roll" };
    String[] matches;
    bool matched;
    [SerializeField] String[] left = new String[3], right = new String[3];

    [SerializeField] int[] indexA, indexB, answerIndexA, answerIndexB;

    int player1Index, player2Index;
    bool click1, click2;


    void Start()
    {
        matches = new String[pairsA.Length];
        for (int i = 0; i < pairsA.Length; i++)
        {
            matches[i] = pairsA[i] + " & " + pairsB[i];
        }
    }


    void Update()
    {
        Display();
        Timer();

        PlayerInput(1, player1Index, click1, leftPairs);
        PlayerInput(2, player2Index, click2, rightPairs);
    }


    void Display()
    {
        if (active && !display.activeInHierarchy)
        {
            display.SetActive(active);
        }

        else if (!active && display.activeInHierarchy)
        {
            display.SetActive(active);
        }
    }


    public void PairGenerator()
    {
        RandomizeText(pairsA, pairsB, left, right);
        ShufflePairs(left, leftPairs);
        ShufflePairs(right, rightPairs);
        ResetPairs();
    }


    // Knuth shuffle algorithm courtesty of https://discussions.unity.com/t/randomize-array-in-c/443241
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

    void ResetPairs()
    {
        pairsA = new String[] { "Peanut Butter", "Rice", "Bonnie", "Salt", "Sun", "You", "Rock" };
        pairsB = new String[] { "Jelly", "Beans", "Clyde", "Pepper", "Moon", "Me", "Roll" };
    }

    void PlayerInput(int playerNum, int playerIndex, bool click, TextMeshProUGUI[] text)
    {
        float dpad = Input.GetAxisRaw("D Pad" + playerNum);

        if (dpad != 0f && !click)
        {
            if (dpad > 0f)
                playerIndex = (playerIndex < 2) ? playerIndex + 1 : 0;
            else
                playerIndex = (playerIndex == 0) ? 2 : playerIndex - 1;

            click = true;

            if (playerNum == 1)
            {
                player1Index = playerIndex;
                click1 = click;
            }
            else
            {
                player2Index = playerIndex;
                click2 = click;
            }
        }
        else if (dpad == 0f && click)
        {
            click = false;

            if (playerNum == 1)
            {
                click1 = click;
            }
            else
            {
                click2 = click;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i == playerIndex)
                text[i].color = Color.yellow;
            else
                text[i].color = Color.white;
        }
    }


    void Timer()
    {
        if (targetTime > 0 && active)
        {
            targetTime -= Time.deltaTime;
        }
        else if (active)
        {
            active = false;
            targetTime = 2.5f;

            String answer = left[player1Index] + " & " + right[player2Index];

            for (int i = 0; i < matches.Length; i++)
            {
                if (answer == matches[i]) matched = true;
            }

            if (matched)
            {
                Debug.Log("YOU GOT A MATCH");
            }
            else
            {
                Debug.Log("NADA");
            }

            matched = false;
        }
    }
}

