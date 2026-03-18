using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimingBarView : BasePanel
{
    private TimingBarViewModel vm;

    private int angle;
    private bool isSpining = false;

    [Header("References")]
    [SerializeField] RectTransform successZone;     // Tay quay
    [SerializeField] RectTransform trackZone;      // Vùng bắt cá
    [SerializeField] Image successImg;
    [SerializeField] Image itemImg;
    [SerializeField] Image processMaker;
    [SerializeField] Transform roundMarkerParent;
    [SerializeField] GameObject roundMarkerPrefab;
    [SerializeField] TextMeshProUGUI RemainingText;
    [SerializeField] TextMeshProUGUI TextNameItem;

    public void Bind(TimingBarViewModel vm)
    {
        this.vm = vm;
        OnInit();
        vm.OnStart += OnStart;
        vm.OnHandle += OnHandle;
        vm.OnFinish += OnFinish;
        vm.OnRestart += OnRestart;
    }

    public void UnBind()
    {
        if (vm == null) return;

        vm.OnStart -= OnStart;
        vm.OnHandle -= OnHandle;
        vm.OnFinish -= OnFinish;
        vm.OnRestart -= OnRestart;

        vm = null;
    }

    void Update()
    {
        if (isSpining)
        {
            MoveTrack(vm.trackSpeed);
        }
    }

    void OnInit()
    {
        itemImg.sprite = vm.award.icon;
        trackZone.localEulerAngles = Vector3.zero;
        processMaker.fillAmount = 0f;

        TextNameItem.text = vm.award.itemName;

        DeleteMaker(roundMarkerParent);
        AddMarker(roundMarkerPrefab, roundMarkerParent, vm.totalRounds);
    }

    void OnStart()
    {
        ResetZone();
        isSpining = true;
    }

    void OnHandle(bool result)
    {
        if (result) IncreaseProcess(vm.currentRound, vm.totalRounds);
        ResetZone();
    }

    void OnFinish()
    {
        isSpining = false;
    }

    void OnRestart()
    {
        processMaker.fillAmount = 0f;
        DeleteMaker(roundMarkerParent);
        AddMarker(roundMarkerPrefab, roundMarkerParent, vm.totalRounds);
    }

    void ResetZone()
    {
        angle = Random.Range(0, 360);
        successZone.localEulerAngles = new Vector3(0, 0, angle);
        successImg.fillAmount = vm.successZoneSize / 100f;
    }

    void DeleteMaker(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    void AddMarker(GameObject prefab, Transform parent, int count)
    {
        for (int i = 0; i < count - 1; i++)
        {
            Instantiate(prefab, parent);
        }
    }

    void MoveTrack(float trackSpeed)
    {
        trackZone.Rotate(0, 0, -trackSpeed * Time.deltaTime);
    }

    void IncreaseProcess(float currentRound, float totalRounds)
    {
        processMaker.fillAmount = (float)currentRound / totalRounds;
    }

    public void UpdateRemainingFish(int count)
    {
        RemainingText.text = count.ToString();
    }

    public bool IsSuccess()
    {
        float trackAngle = trackZone.localEulerAngles.z;
        float successBeginAngle = successZone.localEulerAngles.z;
        float zoneRange = successImg.fillAmount * 360f;

        bool result = vm.IsSuccess(trackAngle, successBeginAngle, zoneRange);
        return result;
    }
}
