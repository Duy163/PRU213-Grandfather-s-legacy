using UnityEngine;

public class PlayerInventoryController : BaseInventoryController
{
    protected override bool IsPlayerInventory() => true;

    public override void Initialize(InventoryData data, FishDatabase fishDatabase)
    {
        base.Initialize(data, fishDatabase);
    }
}
