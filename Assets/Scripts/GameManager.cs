using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int p1Choice, p2Choice;

    void Awake()
    {
        Instance = this;
    }
}
