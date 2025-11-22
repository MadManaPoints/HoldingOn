using UnityEngine;

public class AndThenThereWereTwo : MonoBehaviour
{
    [SerializeField] GameObject [] p1;
    [SerializeField] GameObject [] p2;

    void Start()
    {
        Debug.Log(GameManager.Instance.p1Choice);
        for(int i = 0; i < 2; i++)
        {   if(i != GameManager.Instance.p1Choice)
            Destroy(p1[i]);
            if(i != GameManager.Instance.p2Choice)
            Destroy(p2[i]);
        }

        Destroy(this.gameObject);
    }
}
