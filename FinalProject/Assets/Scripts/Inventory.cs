using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>(); 

    public void AddItem(GameObject item)
    {
        items.Add(item);
        item.SetActive(false);
    }

    public void ItemDrop(Vector3 dropPosition)
    {
        if (items.Count > 0)
        {
            GameObject itemToDrop = items[0];
            items.RemoveAt(0);
            itemToDrop.SetActive(true);
            itemToDrop.transform.position = dropPosition;
        }
    }

    public void DropItem(Vector3 dropPosition)
    {
        if (items.Count > 0)
        {
            GameObject itemToDrop = items[0];
            items.RemoveAt(0);
            itemToDrop.SetActive(true);
            itemToDrop.transform.position = dropPosition;
        }
    }
}
