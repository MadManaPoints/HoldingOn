using UnityEngine;

public class SplitThePole : MonoBehaviour
{
    PlayerMovement playerOne, playerTwo;
    [SerializeField] Pairs pairs;
    bool split;
    bool tutorial;
    [SerializeField] GameObject tutorialObj;

    void Start()
    {
        tutorial = GameManager.Instance.tutorial;
    }

    void Update()
    {
        AddPlayers();
        if (!split) BadLuck();
    }

    void AddPlayers()
    {
        if (playerOne == null) playerOne = GameObject.Find("Player 1").GetComponent<PlayerMovement>();
        if (playerTwo == null) playerTwo = GameObject.Find("Player 2").GetComponent<PlayerMovement>();

        if (playerOne == null || playerTwo == null) return;
    }

    void BadLuck()
    {
        if (playerOne.transform.position.z > transform.position.z &&
          playerTwo.transform.position.z > transform.position.z)
        {
            if (tutorial)
            {
                playerOne.playerControl = false;
                playerTwo.playerControl = false;
                tutorialObj.SetActive(true);
                tutorial = false;
            }

            pairs.PairGenerator();
            pairs.active = true;
            split = true;
        }
    }
}
