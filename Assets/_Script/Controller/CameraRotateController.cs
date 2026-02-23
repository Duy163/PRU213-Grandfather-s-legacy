using Unity.Cinemachine;
using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    [SerializeField] private CinemachineInputAxisController freeLook;

    void OnEnable()
    {
        InputEvent.OnCameraRotate += OnCameraRotate;
    }

    void OnDisable()
    {
        InputEvent.OnCameraRotate -= OnCameraRotate;
    }

    void OnCameraRotate(bool rotating)
    {
        HandleCursor(rotating);
        freeLook.enabled = rotating;
    }

    void HandleCursor(bool rotating)
    {
        if (rotating)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
