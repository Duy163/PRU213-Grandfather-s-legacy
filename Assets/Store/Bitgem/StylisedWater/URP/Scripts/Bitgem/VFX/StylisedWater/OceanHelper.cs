using UnityEngine;

public class OceanHelper : MonoBehaviour
{
    public static OceanHelper Instance;

    [Header("Cấu hình Biển")]
    [Tooltip("Kéo tấm OceanPlane khổng lồ của bạn vào đây")]
    public Transform oceanPlane;

    [Tooltip("Kéo cái Material mặt nước vào đây")]
    public Material oceanMaterial;

    // Cache lại thông số để tránh giật lag khi gọi nhiều lần
    private float waveFreq;
    private float waveScale;
    private float waveSpeed;

    private void Awake()
    {
        Instance = this;

        // Đọc thông số Shader 1 lần duy nhất lúc bật game
        if (oceanMaterial != null)
        {
            waveFreq = oceanMaterial.GetFloat("_WaveFrequency");
            waveScale = oceanMaterial.GetFloat("_WaveScale");
            waveSpeed = oceanMaterial.GetFloat("_WaveSpeed");
        }
    }

    // Hàm này cho con thuyền gọi
    public float GetOceanHeightAt(Vector3 position)
    {
        if (oceanPlane == null || oceanMaterial == null) return 0f;

        // 1. Lấy độ cao tĩnh của mặt biển
        float baseHeight = oceanPlane.position.y;

        // 2. Tính toán lại độ nhấp nhô của Shader tại vị trí của thuyền
        float time = Time.time * waveSpeed;
        float shaderOffset = (Mathf.Sin(position.x * waveFreq + time) + Mathf.Cos(position.z * waveFreq + time)) * waveScale;

        // 3. Cộng lại và trả về
        return baseHeight + shaderOffset;
    }
}