using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Database")]
public class FishDatabase : ScriptableObject
{
    public List<ItemData> fishList;
    private Dictionary<string, ItemData> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, ItemData>();

        foreach (var item in fishList)
        {
            if (item != null && !lookup.ContainsKey(item.itemId))
                lookup[item.itemId] = item;
        }
    }

    public ItemData GetByID(string id)
    {
        if (lookup == null)
            OnEnable(); // safety net

        lookup.TryGetValue(id, out var item);
        return item;
    }
}
