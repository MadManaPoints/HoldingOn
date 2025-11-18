using System.Collections;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public bool hiding;
    public bool eating;
    public Carrot carrot;
    Vector3 startPos, endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos;
        endPos.x = -6.26f;
    }

    void Update()
    {
        if (hiding)
        {
            MoveToSpot(endPos, 5.0f);
        }
        else if (carrot != null)
        {
            MoveToSpot(carrot.transform.position, 5.0f);
        }
        else
        {
            MoveToSpot(startPos, 3.0f);
        }
    }

    void MoveToSpot(Vector3 target, float speed)
    {
        if (transform.position != target)
        {
            Vector3 dir = (target - transform.position).normalized;
            float dist = Vector3.Distance(transform.position, target);
            if (dist > 0.1)
            {
                transform.position += dir * speed * Time.deltaTime;
            }
            else
            {
                transform.position = target;
            }

            transform.LookAt(target);
        }
    }
}
