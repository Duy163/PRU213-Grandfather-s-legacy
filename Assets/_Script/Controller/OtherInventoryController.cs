using UnityEngine;

public class OtherInventoryController : BaseInventoryController
{
    protected override bool IsPlayerInventory() => false;

    public override void Initialize(InventoryData data)
    {
        base.Initialize(data);
    }
}
