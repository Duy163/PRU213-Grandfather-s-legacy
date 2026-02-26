using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatBuoyancy : MonoBehaviour
{
    [Header("Water Settings")]
    [Tooltip("Chiều cao mặt nước (Y world position)")]
    public float waterHeight = 0f;

    [Header("Buoyancy")]
    [Tooltip("Các điểm nổi đặt dưới đáy tàu")]
    public Transform[] floatPoints;

    [Tooltip("Độ mạnh lực nổi")]
    public float floatForce = 25f;

    [Tooltip("Giảm dao động khi lên xuống")]
    public float damping = 6f;

    [Tooltip("Offset tinh chỉnh độ chìm")]
    public float heightOffset = 0f;

    [Header("Stability")]
    [Tooltip("Giữ tàu ổn định hơn")]
    public float uprightStrength = 3f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 2f;

        // Hạ trọng tâm giúp tàu tự cân bằng tốt hơn
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void FixedUpdate()
    {
        if (floatPoints == null || floatPoints.Length == 0)
            return;

        foreach (Transform point in floatPoints)
        {
            ApplyBuoyancy(point);
        }

        ApplyUprightForce();
    }

    // ================= BUOYANCY =================

    void ApplyBuoyancy(Transform point)
    {
        float pointY = point.position.y + heightOffset;
        float depth = waterHeight - pointY;

        // Nếu điểm nằm dưới nước
        if (depth > 0f)
        {
            // lực nổi
            float upwardForce = depth * floatForce;

            // damping theo velocity tại điểm đó
            float velocityY = rb.GetPointVelocity(point.position).y;
            float dampingForce = -velocityY * damping;

            Vector3 force = Vector3.up * (upwardForce + dampingForce);

            // QUAN TRỌNG: force tại vị trí → tạo torque cân bằng
            rb.AddForceAtPosition(force, point.position, ForceMode.Force);
        }
    }

    // ================= AUTO BALANCE =================

    void ApplyUprightForce()
    {
        // giữ tàu luôn hướng lên trời
        Vector3 currentUp = transform.up;
        Vector3 targetUp = Vector3.up;

        Vector3 torque = Vector3.Cross(currentUp, targetUp);

        rb.AddTorque(torque * uprightStrength, ForceMode.Acceleration);
    }
}