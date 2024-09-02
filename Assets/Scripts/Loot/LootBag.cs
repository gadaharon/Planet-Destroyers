using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] List<Loot> lootList;

    Loot GetLootByRandomDropRate()
    {
        if (lootList.Count <= 0) { return null; }

        int randomDropRate = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();

        foreach (Loot item in lootList)
        {
            if (randomDropRate <= item.dropRate)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0)
        {
            Loot itemToDrop = possibleItems[Random.Range(0, possibleItems.Count)];
            return itemToDrop;
        }

        Debug.Log("NO ITEM TO DROP");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetLootByRandomDropRate();
        if (droppedItem != null)
        {
            // Instantiate(droppedItem.prefab, spawnPosition, Quaternion.identity, lootParentTransform);
            Instantiate(droppedItem.prefab, spawnPosition, Quaternion.identity);
        }
    }
}
