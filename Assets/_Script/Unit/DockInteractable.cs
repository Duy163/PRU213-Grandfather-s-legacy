using UnityEngine;

public class DockInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Transform ship;
    [SerializeField] private Transform dockPoint; // Vị trí neo đậu

    [Header("Settings")]

    [SerializeField] private string dockName;
    [SerializeField] private bool isAvailable = true;
    [SerializeField] private float dockingSpeed = 1f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float stopDistance = 0.5f;
    private bool isDocking = false;


    void Update()
    {
        if (isDocking)
        {
            DockShip();
        }
    }

    public void StartDocking()
    {
        isDocking = true;
    }

    public string GetInteractPrompt()
    {
        return "Giữ [F] để neo";
    }

    public bool CanInteract()
    {
        return isAvailable;
    }

    public void Interact()
    {
        StartDocking();
        WorldDataManager.Instance.StartDocking(dockName);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void DockShip()
    {
        // Di chuyển tàu đến dock
        float distance = Vector3.Distance(ship.position, dockPoint.position);

        if (distance > stopDistance)
        {
            // Lerp vị trí
            ship.position = Vector3.Lerp(
                ship.position,
                dockPoint.position,
                dockingSpeed * Time.deltaTime
            );

            // Lerp xoay
            ship.rotation = Quaternion.Lerp(
                ship.rotation,
                dockPoint.rotation,
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            // Đã đến - snap vào vị trí chính xác
            ship.position = dockPoint.position;
            ship.rotation = dockPoint.rotation;
            isDocking = false;
            DataManager.Instance.currentGameData.playerShipData.position = ship.position;
            DataManager.Instance.currentGameData.playerShipData.rotation = ship.eulerAngles;
            DataManager.Instance.Save();
            Debug.Log($"Ship docked at {dockName}. Position saved.");
        }
    }
}
