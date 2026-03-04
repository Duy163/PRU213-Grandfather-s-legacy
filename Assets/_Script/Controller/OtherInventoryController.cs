using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class OtherInventoryController : BaseInventoryController
{
    protected override bool IsPlayerInventory() => false;

    public override void Initialize(InventoryData data, FishDatabase fishDatabase)
    {
        base.Initialize(data, fishDatabase);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
