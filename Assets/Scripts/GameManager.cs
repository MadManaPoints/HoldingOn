using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int p1Choice, p2Choice;
    public bool playersTogether;
    public bool tooFar;

    void Awake()
    {
        Instance = this;
    }
}
