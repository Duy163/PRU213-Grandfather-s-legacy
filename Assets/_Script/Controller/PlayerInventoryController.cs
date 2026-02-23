using UnityEngine;

public class PlayerInventoryController : BaseInventoryController
{
    protected override bool IsPlayerInventory() => true;

    public override void Initialize(InventoryData data)
    {
        base.Initialize(data);
    }
}
