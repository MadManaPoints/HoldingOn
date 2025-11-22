using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Pairs : MonoBehaviour
{
    [SerializeField] GameObject display; // All text objects
    public bool active; // true when testing
    float targetTime = 5.0f; // Timer to close textbox
    [SerializeField] TextMeshProUGUI[] leftPairs, rightPairs; // Text to display for each player

    // Arrays of text for each player 
    String[] pairsA = new String[] { "Peanut Butter", "Rice", "Bonnie", "Salt", "Sun", "You", "Rock" };
    String[] pairsB = new String[] { "Jelly", "Beans", "Clyde", "Pepper", "Moon", "Me", "Roll" };
    String[] matches; // Combines strings from A and B 
    bool matched;
    [SerializeField] String[] left = new String[3], right = new String[3];

    int player1Index, player2Index; // Tracks player UI positions
    bool click1, click2;
    LevelTimer time;
    [SerializeField] Animator anim;
    public bool tutorialStart;
    bool tutorialEnd;

    void Start()
    {
        matches = new String[pairsA.Length];
        for (int i = 0; i < pairsA.Length; i++)
        {
            matches[i] = pairsA[i] + " & " + pairsB[i]; // Combine A and B and make new String
        }

        time = GetComponentInChildren<LevelTimer>();
        //anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (!tutorialEnd) PlayTutorial();
        Display(); // Turn text objects on and off
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
        // Shuffle arrays and grab three pairs to display for players
        RandomizeText(pairsA, pairsB, left, right);
    }


    // Knuth shuffle algorithm courtesty of https://discussions.unity.com/t/randomize-array-in-c/443241
    void RandomizeText(String[] textA, String[] textB, String[] sideA, String[] sideB)
    {
        for (int t = 0; t < 3; t++)
        {
            String tmpA = textA[t];
            String tmpB = textB[t];

            int r = UnityEngine.Random.Range(t, textA.Length);

            textA[t] = textA[r];
            textB[t] = textB[r];

            sideA[t] = textA[t];
            sideB[t] = textB[t];

            textA[r] = tmpA;
            textB[r] = tmpB;
        }

        ShufflePairs(left, leftPairs);
        ShufflePairs(right, rightPairs);
        ResetPairs(); // Reset pairs A and B arrays to ready for next shuffle
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
        // Choose joystick based on player number
        float dpad = Input.GetAxisRaw("D Pad" + playerNum);

        if (dpad != 0f && !click)
        {
            // Use player index to cycle through text
            if (dpad > 0f)
                playerIndex = (playerIndex < 2) ? playerIndex + 1 : 0;
            else
                playerIndex = (playerIndex == 0) ? 2 : playerIndex - 1;

            click = true; // Prevent holding down dpad 

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

        // Change text color based on current selection
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
        if (GameManager.Instance.tutorial) return;

        if (targetTime > 0 && active)
        {
            targetTime -= Time.deltaTime;
        }
        else if (active)
        {
            active = false; // Turn off textbox
            targetTime = 2.5f; // Reset target time

            // Combine selected Strings
            String answer = left[player1Index] + " & " + right[player2Index];

            for (int i = 0; i < matches.Length; i++)
            {
                // Determine whether the new String is a match
                if (answer == matches[i]) matched = true;
            }

            if (matched)
            {
                Debug.Log("YOU GOT A MATCH");
            }
            else
            {
                StartCoroutine(time.SubtractTime());
                Debug.Log("NADA");
            }

            matched = false; // Reset
        }
    }

    void PlayTutorial()
    {
        if (!GameManager.Instance.tutorial || !transform.GetChild(transform.childCount - 1).gameObject.activeInHierarchy) return;

        if (!tutorialStart) tutorialStart = true;
        anim.SetBool("Tutorial", true);

        if (active && (Input.GetButtonDown("Action1") || Input.GetButtonDown("Action2") || Input.GetKeyDown(KeyCode.U)))
        {
            tutorialStart = false;
            GameObject.Find("Time").GetComponent<LevelTimer>().tutorial = false;
            anim.SetBool("Tutorial", false);
            GameManager.Instance.tutorial = false;
            tutorialEnd = true;
            anim = null;
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }
    }
}

