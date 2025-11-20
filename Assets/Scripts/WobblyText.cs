using System;
using UnityEngine;

public class WobblyText : MonoBehaviour
{
    [SerializeField] bool p1;
    PlayerMovement player;
    [SerializeField] Transform start, end;
    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        AddPlayers();

        if (player.actualMove != Vector3.zero)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y,
            transform.localEulerAngles.z + Mathf.Sin(Time.time * 6.0f) * 20.0f * Time.deltaTime);
        else
            transform.localEulerAngles = Vector3.zero;


        float distFromGoal = end.position.z - player.transform.position.z;
        float clampedDist = Mathf.Clamp(distFromGoal, 0, 171f);

        float pos = Map(clampedDist, 171.0f, 0, -200.0f, 175.0f);

        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, pos);
    }

    void AddPlayers()
    {
        String p = p1 ? "Player 1" : "Player 2";
        if (player == null) player = GameObject.Find(p).GetComponent<PlayerMovement>();

        if (player == null) return;
    }

    float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float range = maxA - minA;
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;

        return valuePercent * newRange + minB;
    }
}
