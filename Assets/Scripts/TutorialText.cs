using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] GameObject firstText;
    bool hit;
    bool fin;
    [SerializeField] TextMeshProUGUI text;

    void Update()
    {
        if (hit && !fin)
        {
            if (text.color.a < 1.0f)
            {
                Color c = text.color;
                c.a += Time.deltaTime;
                text.color = c;
            }
            else
            {
                fin = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && !hit)
        {
            hit = true;
            firstText.SetActive(false);
        }
    }
}
