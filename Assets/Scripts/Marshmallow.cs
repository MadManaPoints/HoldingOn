using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Marshmallow : Item
{
    ParticleSystem particle;
    public bool nearFlame;
    new Renderer renderer;
    float t = 0f;
    float burnDuration = 5.0f;
    Color startColor = Color.white;
    Color endColor = Color.black;
    public bool burning;

    protected override void Start()
    {
        base.Start();

        renderer = transform.GetChild(0).GetComponent<Renderer>();
        particle = GetComponent<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();

        if (nearFlame && !burning)
            Burn();
    }

    void Burn()
    {
        if (t < 1)
        {
            renderer.material.color = Color.Lerp(startColor, endColor, t);
            t += Time.deltaTime / burnDuration;
        }
        else
        {
            renderer.material.color = endColor;
            burning = true;
            particle.Play();
        }
    }
}
