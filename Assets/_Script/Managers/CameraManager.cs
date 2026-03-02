using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera fishingCamera;
    [SerializeField] private CinemachineCamera defaultCamera;
    [SerializeField] private CinemachineCamera quietIslandCamera;

    private CinemachineBrain brain;

    protected override void Awake()
    {
        base.Awake();
        brain = Camera.main.GetComponent<CinemachineBrain>();

        SetNormalView();
    }

    void OnEnable()
    {
        CameraEvent.OnFocusIslandCamera += EnterIslandView;
    }

    // ================= PUBLIC API =================

    public void EnterFishingView()
    {
        fishingCamera.Priority = 20;
        defaultCamera.Priority = 0;
    }

    public void EnterIslandView(string islandName)
    {
        switch (islandName)
        {
            case "QuietIsland":
                quietIslandCamera.Priority = 20;
                defaultCamera.Priority = 0;
                break;
            default:
                break;
        }
    }

    public void NormalView()
    {
        SetNormalView();
    }

    // ================= INTERNAL =================

    void SetNormalView()
    {
        defaultCamera.Priority = 20;
        fishingCamera.Priority = 0;
        quietIslandCamera.Priority = 0;
    }
}