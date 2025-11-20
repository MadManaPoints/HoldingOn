using UnityEngine;

public class AllowRelease : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hand Area")
        {
            col.gameObject.GetComponent<HandTarget>().handsIn = 0;
            GameManager.Instance.bindPlayers = false;
            Destroy(gameObject);
        }
    }
}
