using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEnd : MonoBehaviour
{
    public static DemoEnd Instance;
    public int playersAtEnd;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (playersAtEnd == 2)
        {
            SceneManager.LoadScene("CharacterSelection");
        }
    }
}
