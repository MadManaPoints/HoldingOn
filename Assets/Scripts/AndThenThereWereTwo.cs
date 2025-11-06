using UnityEngine;

public class AndThenThereWereTwo : MonoBehaviour
{
    [SerializeField] GameObject [] p1;
    [SerializeField] GameObject [] p2;

    void Start()
    {
        if(GameManager.Instance.p1Choice == 0)
            Destroy(p1[0]);
        else
            Destroy(p1[1]);

        if(GameManager.Instance.p2Choice == 0)
            Destroy(p2[0]);
        else
            Destroy(p2[1]);

        Destroy(this.gameObject);
    }
}
