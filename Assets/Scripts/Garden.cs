using UnityEngine;
using System.Collections.Generic;

public class Garden : MonoBehaviour
{
    Vector3 endPos;
    [SerializeField] Transform carrots;
    [SerializeField] Renderer dirtMat;
    [SerializeField] Material m;
    float targetTime = 3.0f;
    public bool grow;
    public bool readyToHarvest;
    public List<GameObject> carrotList = new List<GameObject>();
    void Start()
    {
        endPos = new Vector3(carrots.position.x, 1.76f, carrots.position.z);
    }

    void Update()
    {
        if (!grow) return;

        if (targetTime > 0f)
        {
            if (dirtMat.material != m) dirtMat.material = m;
            targetTime -= Time.deltaTime;
        }
        else
        {
            Sprout();
        }
    }

    void Sprout()
    {
        if (carrots.position.y < endPos.y)
        {
            carrots.position += Vector3.up * Time.deltaTime / 30f;
        }
        else if (carrots.position.y != endPos.y)
        {
            readyToHarvest = true;
            carrots.position = endPos;
        }
    }

    public void GiveItem(PlayerMovement player)
    {
        player.item = carrotList[carrotList.Count - 1].GetComponent<Carrot>();
        carrotList.Remove(carrotList[carrotList.Count - 1]);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            if (player.item != null && player.item.key == "Bucket")
            {
                grow = true;
            }

            player.garden = this;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            if (player.item != null && player.item.key == "Bucket")
            {
                //grow = true;
            }

            player.garden = null;
        }
    }
}
