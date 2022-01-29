using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<string> itemNames = new List<string>();
    public List<int> itemAmount = new List<int>();

    public GameObject[] lamps;
    public int[] costLamps;
    public Transform lampSpawn;
    // Start is called before the first frame update
    void Start()
    {
        ItemAdd(0, 0);
        ItemAdd(1, 0);
        ItemAdd(2, 0);
        ItemAdd(3, 0);
        ItemAdd(4, 0);
        ItemAdd(5, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemAdd(int id, int amount)
    {
        itemAmount[id] += amount;
        GameManager.Instance.canvas.ResourceAdd(itemAmount[id], id);
        UpdateResource();
    }

    public void CreateLamp(int id)
    {
        GameObject lamp = Instantiate(lamps[id], lampSpawn.position, lampSpawn.rotation) as GameObject;
        ItemAdd(id, -costLamps[id]);
        UpdateResource();
    }

    public void UpdateResource()
    {
        if(itemAmount[0] < costLamps[0])
        {
            GameManager.Instance.canvas.UpdateButtonLamp(0, false);
        }else
        {
            GameManager.Instance.canvas.UpdateButtonLamp(0, true);
        }

        if (itemAmount[1] < costLamps[1])
        {
            GameManager.Instance.canvas.UpdateButtonLamp(1, false);
        }
        else
        {
            GameManager.Instance.canvas.UpdateButtonLamp(1, true);
        }

        if (itemAmount[2] < costLamps[2])
        {
            GameManager.Instance.canvas.UpdateButtonLamp(2, false);
        }
        else
        {
            GameManager.Instance.canvas.UpdateButtonLamp(2, true);
        }

        if (itemAmount[3] < costLamps[3])
        {
            GameManager.Instance.canvas.UpdateButtonLamp(3, false);
        }
        else
        {
            GameManager.Instance.canvas.UpdateButtonLamp(3, true);
        }


    }
}
