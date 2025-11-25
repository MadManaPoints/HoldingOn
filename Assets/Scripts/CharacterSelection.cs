using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] int playerNum;
    [SerializeField] GameObject[] playerChoices;
    int index;
    bool click;
    void Start()
    {
        index = playerNum == 1 ? GameManager.Instance.p1Choice : GameManager.Instance.p2Choice;
    }

    void Update()
    {
        float dpad = Input.GetAxisRaw("Selection" + playerNum);

        if (dpad != 0f && !click)
        {
            index = index == 0 ? 1 : 0;

            for (int i = 0; i < playerChoices.Length; i++)
            {
                if (i == index && !playerChoices[i].activeInHierarchy) playerChoices[i].SetActive(true);
                else if (i != index && playerChoices[i].activeInHierarchy) playerChoices[i].SetActive(false);
            }

            if (playerNum == 1) GameManager.Instance.p1Choice = index;
            if (playerNum == 2) GameManager.Instance.p2Choice = index;

            click = true;
        }

        if (dpad == 0f && click) click = false;

        if (Input.GetButtonDown("Start1") || Input.GetButtonDown("Start2")) SceneManager.LoadScene("Main");
    }
}
