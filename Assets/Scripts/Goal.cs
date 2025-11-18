using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") DemoEnd.Instance.playersAtEnd++;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player") DemoEnd.Instance.playersAtEnd--;
    }
}
