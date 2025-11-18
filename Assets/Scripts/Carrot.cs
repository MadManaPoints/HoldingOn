using UnityEngine;

public class Carrot : Item
{
    protected override void Start()
    {
        startPos.y = 1.7f;
        startRot = transform.localEulerAngles;
    }

    protected override void Update()
    {
        base.Update();
    }
}
