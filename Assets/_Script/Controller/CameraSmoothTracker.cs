using UnityEngine;

public class CameraSmoothTracker : MonoBehaviour
{
    [Header("Tracking Settings")]
    [Tooltip("Thuyền cần được theo dõi (ví dụ OldShip1)")]
    public Transform targetShip;

    [Tooltip("Dùng để offset vị trí (vị trí tâm camera nhìn vào)")]
    public Vector3 targetOffset = Vector3.zero;

    [Header("Damping Settings")]
    [Tooltip("Độ mượt khi theo dõi vị trí Y (Damping càng lớn, camera càng chậm). Trục Y là trục gây giật chính do sóng.")]
    public float ySmoothingTime = 0.5f;

    [Tooltip("Damping của X và Z (có thể nhỏ hơn)")]
    public float xzSmoothingTime = 0.2f;

    private Vector3 currentVelocity; // Variable để Mathf.SmoothDamp dùng

    void Update()
    {
        if (targetShip == null) return;

        // TÍNH TOÁN VỊ TRÍ MỤC TIÊU
        Vector3 targetPos = targetShip.position + targetOffset;

        // CHỈ LÀM MỊN TỌA ĐỘ Y (Chỉ làm mịn trục nhấp nhô)
        // Đây là bí mật của sự mượt mà
        float smoothedY = Mathf.SmoothDamp(transform.position.y, targetPos.y, ref currentVelocity.y, ySmoothingTime);

        // LÀM MỊN X VÀ Z (có thể mượt hơn X và Z gốc)
        float smoothedX = Mathf.SmoothDamp(transform.position.x, targetPos.x, ref currentVelocity.x, xzSmoothingTime);
        float smoothedZ = Mathf.SmoothDamp(transform.position.z, targetPos.z, ref currentVelocity.z, xzSmoothingTime);

        // Cập nhật vị trí của Tracker object này sang vị trí đã làm mịn
        transform.position = new Vector3(smoothedX, smoothedY, smoothedZ);
    }
}