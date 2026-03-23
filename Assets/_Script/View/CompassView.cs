using UnityEngine;

public class CompassView : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform compassBarTransform;
    public RectTransform northMarkerTransform;
    public RectTransform southMarkerTransform;
    public RectTransform eastMarkerTransform;  // Thêm biến cho hướng Đông
    public RectTransform westMarkerTransform;  // Thêm biến cho hướng Tây
    // Bạn có thể thêm East và West vào đây

    [Header("Quest Waypoint")]
    public RectTransform questMarkerTransform; // Kéo thả icon nhiệm vụ (UI) vào đây
    public Transform currentQuestTarget;       // Kéo thả vật thể/NPC làm nhiệm vụ vào đây

    [Header("Player/Camera References")]
    public Transform cameraObjectTransform;

    // Cache lại Camera để tối ưu hiệu năng
    private Camera mainCamera;

    // Tùy chỉnh độ nhạy/khoảng cách của la bàn trên UI
    public float compassMultiplier = 1f;

    void Start()
    {
        // Gán camera một lần duy nhất khi bắt đầu game
        mainCamera = Camera.main;

        // Nếu cameraObjectTransform chính là Camera, bạn có thể dùng:
        // mainCamera = cameraObjectTransform.GetComponent<Camera>();
    }

    void Update()
    {
        if (cameraObjectTransform == null || mainCamera == null) return;

        // Cập nhật vị trí cả 4 hướng
        SetMarkerPosition(northMarkerTransform, Vector3.forward * 1000f); // Bắc (Z+)
        SetMarkerPosition(southMarkerTransform, Vector3.back * 1000f);    // Nam (Z-)
        SetMarkerPosition(eastMarkerTransform, Vector3.right * 1000f);    // Đông (X+)
        SetMarkerPosition(westMarkerTransform, Vector3.left * 1000f);     // Tây (X-)

        UpdateQuestMarker();
    }

    private void UpdateQuestMarker()
    {
        // Kiểm tra xem có mục tiêu nhiệm vụ nào đang được gán không và UI có tồn tại không
        if (currentQuestTarget != null && questMarkerTransform != null)
        {
            // Hiển thị icon nếu nó đang bị ẩn
            if (!questMarkerTransform.gameObject.activeSelf)
            {
                questMarkerTransform.gameObject.SetActive(true);
            }

            // Dùng chính hàm SetMarkerPosition để đặt vị trí cho icon nhiệm vụ
            SetMarkerPosition(questMarkerTransform, currentQuestTarget.position);
        }
        else if (questMarkerTransform != null)
        {
            // Nếu không có nhiệm vụ, ẩn icon đi
            questMarkerTransform.gameObject.SetActive(false);
        }
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        // 1. Tìm hướng từ Camera đến mục tiêu
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;

        // 2. Chiếu (flatten) các vector xuống mặt phẳng ngang (trục Y = 0) để tính góc xoay chính xác
        Vector3 flatPlayerForward = new Vector3(cameraObjectTransform.forward.x, 0, cameraObjectTransform.forward.z);
        Vector3 flatDirectionToTarget = new Vector3(directionToTarget.x, 0, directionToTarget.z);

        // 3. Tính góc lệch (từ -180 đến 180 độ)
        float signedAngle = Vector3.SignedAngle(flatPlayerForward, flatDirectionToTarget, Vector3.up);

        // 4. Đưa góc lệch về tỷ lệ hiển thị trên thanh UI (Tùy chỉnh bằng compassMultiplier)
        // Dùng góc xem ngang ước tính (thường khoảng 60-90 độ tùy game)
        float compassPosition = signedAngle / (mainCamera.fieldOfView * compassMultiplier);
        // Khi bỏ Clamp, các marker sẽ bay xa ra ngoài khung khi không nằm trong góc nhìn
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width * compassPosition, 0);
    }
}