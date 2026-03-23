using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class boatBuoyancy1 : MonoBehaviour
{
    [Header("Water Material Sync")]
    [Tooltip("Kéo Material của mặt nước vào đây")]
    public Material waterMaterial;

    [Tooltip("KÉO THẢ GAMEOBJECT MẶT NƯỚC (MESH RENDERER) VÀO ĐÂY")]
    public Transform waterPlaneTransform;

    [Tooltip("Độ cao cơ bản của mặt nước (transform.position.y của mặt nước)")]
    public float waterBaseHeight = 0f;

    [Header("Shader Property Names (Ghi đúng tên Reference trong Shader)")]
    public string waveSpeed1Name = "_WaveSpeed";
    public string waveFreq1Name = "_WaveFrequency";
    public string waveAmp1Name = "_WaveAmplitude"; // Giả sử bạn có biến Amplitude (độ cao sóng)

    public string waveSpeed2Name = "_WaveSpeed (1)";
    public string waveFreq2Name = "_WaveFrequency (1)";
    public string waveAmp2Name = "_WaveAmplitude (1)";

    // Các biến lưu trữ giá trị đọc được từ Shader
    private float waveSpeed1, waveFreq1, waveAmp1;
    private float waveSpeed2, waveFreq2, waveAmp2;

    // Cache ID để tối ưu hiệu suất (GetFloat bằng ID nhanh hơn bằng String)
    private int speed1ID, freq1ID, amp1ID;
    private int speed2ID, freq2ID, amp2ID;

    [Header("Buoyancy")]
    public Transform[] floatPoints;
    public float floatForce = 25f;
    public float damping = 6f;
    public float heightOffset = 0f;

    [Header("Stability")]
    public float uprightStrength = 3f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.linearDamping = 1f;
        rb.angularDamping = 2f;
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        // Chuyển đổi tên string thành ID để code chạy mượt hơn
        speed1ID = Shader.PropertyToID(waveSpeed1Name);
        freq1ID = Shader.PropertyToID(waveFreq1Name);
        amp1ID = Shader.PropertyToID(waveAmp1Name);

        speed2ID = Shader.PropertyToID(waveSpeed2Name);
        freq2ID = Shader.PropertyToID(waveFreq2Name);
        amp2ID = Shader.PropertyToID(waveAmp2Name);
    }

    void FixedUpdate()
    {
        SyncWaterProperties();

        if (floatPoints == null || floatPoints.Length == 0) return;

        foreach (Transform point in floatPoints)
        {
            ApplyBuoyancy(point);
        }

        Debug.DrawRay(transform.position, Vector3.up * GetWaterHeightAtPosition(transform.position), Color.red);

        ApplyUprightForce();
    }

    // ================= ĐỒNG BỘ TỪ MATERIAL =================
    void SyncWaterProperties()
    {
        if (waterMaterial != null)
        {
            // Liên tục đọc giá trị từ Material cập nhật vào script
            waveSpeed1 = waterMaterial.GetFloat(speed1ID);
            waveFreq1 = waterMaterial.GetFloat(freq1ID);
            waveAmp1 = waterMaterial.GetFloat(amp1ID); // Lưu ý: Trong graph hiện tại bạn chưa nhân Amplitude, nếu sau này thêm vào Shader thì C# cũng tự hiểu.

            waveSpeed2 = waterMaterial.GetFloat(speed2ID);
            waveFreq2 = waterMaterial.GetFloat(freq2ID);
            waveAmp2 = waterMaterial.GetFloat(amp2ID);
        }
    }

    // ================= HÀM TÍNH TOÁN SÓNG =================
    float GetWaterHeightAtPosition(Vector3 position)
    {
        float time = Time.time;

        float wave1 = Mathf.Sin(position.x * waveFreq1 + time * waveSpeed1) * waveAmp1;
        float wave2 = Mathf.Sin(position.z * waveFreq2 + time * waveSpeed2) * waveAmp2;

        // Tổng hợp độ cao sóng cục bộ
        float localWaveHeight = wave1 + wave2;

        // BÍ QUYẾT: Nhân chiều cao sóng với Scale Y của mặt nước
        float scaledWaveHeight = localWaveHeight * waterPlaneTransform.lossyScale.y;

        return waterBaseHeight + scaledWaveHeight;
    }

    // ================= BUOYANCY =================
    void ApplyBuoyancy(Transform point)
    {
        float pointY = point.position.y + heightOffset;
        float currentWaterHeight = GetWaterHeightAtPosition(point.position);
        float depth = currentWaterHeight - pointY;

        if (depth > 0f)
        {
            float upwardForce = depth * floatForce;
            float velocityY = rb.GetPointVelocity(point.position).y;
            float dampingForce = -velocityY * damping;

            Vector3 force = Vector3.up * (upwardForce + dampingForce);
            rb.AddForceAtPosition(force, point.position, ForceMode.Force);
        }
    }

    // ================= AUTO BALANCE =================
    void ApplyUprightForce()
    {
        Vector3 currentUp = transform.up;
        Vector3 targetUp = Vector3.up;
        Vector3 torque = Vector3.Cross(currentUp, targetUp);
        rb.AddTorque(torque * uprightStrength, ForceMode.Acceleration);
    }
}
