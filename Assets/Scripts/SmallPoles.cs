using UnityEngine;

public class SmallPoles : MonoBehaviour
{
    [SerializeField] PlayerMovement playerOne, playerTwo;
    bool split;


    void Update()
    {
        AddPlayers();

        if (playerOne.transform.position.z > transform.position.z &&
          playerTwo.transform.position.z > transform.position.z && !split)
        {
            if (playerOne.attached && playerTwo.attached)
                StartCoroutine(LevelTimer.Instance.AddTime());
            split = true;
        }

    }

    void AddPlayers()
    {
        if (playerOne == null) playerOne = GameObject.Find("Player 1").GetComponent<PlayerMovement>();
        if (playerTwo == null) playerTwo = GameObject.Find("Player 2").GetComponent<PlayerMovement>();

        if (playerOne == null || playerTwo == null) return;
    }
}