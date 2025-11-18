using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public bool on;
    [SerializeField] Material onMat, offMat;
    MeshRenderer r;

    void Start()
    {
        r = GetComponent<MeshRenderer>();
        r.material = offMat;
    }

    void Update()
    {
        if (on && r.material != onMat) r.material = onMat;
        else if (!on && r.material != offMat) r.material = offMat;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Bunny")
            on = true;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Bunny")
            on = false;
    }
}
