using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int p1Choice, p2Choice;
    public bool playersTogether;
    public bool tooFar;
    public bool tutorial = true;
    public bool bindPlayers = true;

    void Awake()
    {
        Instance = this;
    }
}
