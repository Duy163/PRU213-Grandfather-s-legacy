using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightPreset lightPreset;
    [SerializeField] private Material skyboxMaterial; // Material skybox (Procedural)

    [Header("Variables")]
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private bool enableFog = true;

    [Header("Skybox Control")]
    [SerializeField] private bool controlSkybox = true;
    [SerializeField] private Gradient skyTintGradient; // Màu sky theo thời gian
    [SerializeField] private Gradient groundColorGradient; // Màu ground theo thời gian
    // [SerializeField, Range(0, 5)] private float atmosphereThickness = 1.0f;
    // [SerializeField, Range(0, 8)] private float exposure = 2.5f;
    // [SerializeField, Range(0, 1)] private float sunSize = 0.04f;
    // [SerializeField, Range(1, 10)] private float sunSizeConvergence = 5f;

    private void Start()
    {
    }

    private void Update()
    {
        if (lightPreset == null)
            return;

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // Cập nhật Skybox
        // if (controlSkybox && skyboxMaterial != null)
        // {
        UpdateSkybox(timePercent);
        // }

        // Cập nhật Ambient Light
        RenderSettings.ambientLight = lightPreset.ambientColor.Evaluate(timePercent);

        // Cập nhật Fog
        UpdateFog(timePercent);

        // Cập nhật Directional Light
        if (directionalLight != null)
        {
            directionalLight.color = lightPreset.directionalLightColor.Evaluate(timePercent);

            // Xoay ánh sáng (mặt trời/mặt trăng)
            directionalLight.transform.localRotation = Quaternion.Euler(
                new Vector3((timePercent * 360f) - 90f, 170f, 0)
            );
        }
    }
    private void UpdateSkybox(float timePercent)
    {

        // skyboxMaterial.SetColor("_SkyTint", skyTintGradient.Evaluate(timePercent));
        if (skyboxMaterial.HasProperty("_SkyTint"))
        {
            skyboxMaterial.SetColor("_SkyTint", skyTintGradient.Evaluate(timePercent));
        }

        if (skyboxMaterial.HasProperty("_GroundColor"))
        {
            skyboxMaterial.SetColor("_GroundColor", groundColorGradient.Evaluate(timePercent));
        }
    }

    // private void UpdateSkybox(float timePercent)
    // {
    //     // Chỉ áp dụng nếu là Procedural Skybox
    //     if (!skyboxMaterial.shader.name.Contains("Skybox/Procedural"))
    //     {
    //         Debug.LogWarning("Skybox không phải Procedural! Script chỉ hỗ trợ Skybox/Procedural.");
    //         return;
    //     }

    //     // Cập nhật Sky Tint (màu bầu trời)
    //     if (skyboxMaterial.HasProperty("_SkyTint"))
    //     {
    //         skyboxMaterial.SetColor("_SkyTint", skyTintGradient.Evaluate(timePercent));
    //     }

    //     // Cập nhật Ground Color (màu mặt đất)
    //     if (skyboxMaterial.HasProperty("_GroundColor"))
    //     {
    //         skyboxMaterial.SetColor("_GroundColor", lightPreset.directionalLightColor.Evaluate(timePercent));
    //     }

    //     // Cập nhật các thông số khác
    //     if (skyboxMaterial.HasProperty("_AtmosphereThickness"))
    //     {
    //         skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphereThickness);
    //     }

    //     if (skyboxMaterial.HasProperty("_Exposure"))
    //     {
    //         skyboxMaterial.SetFloat("_Exposure", exposure);
    //     }

    //     if (skyboxMaterial.HasProperty("_SunSize"))
    //     {
    //         skyboxMaterial.SetFloat("_SunSize", sunSize);
    //     }

    //     if (skyboxMaterial.HasProperty("_SunSizeConvergence"))
    //     {
    //         skyboxMaterial.SetFloat("_SunSizeConvergence", sunSizeConvergence);
    //     }

    //     // Gán directional light làm "mặt trời" cho skybox
    //     if (directionalLight != null)
    //     {
    //         RenderSettings.sun = directionalLight;
    //     }
    // }

    private void UpdateFog(float timePercent)
    {
        RenderSettings.fogColor = lightPreset.fogColor.Evaluate(timePercent);

        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;

            if (lightPreset.fogDensity != null)
            {
                RenderSettings.fogDensity = lightPreset.fogDensity.Evaluate(timePercent);
            }
        }
        else
        {
            RenderSettings.fog = false;
        }
    }

    // private void SetupDefaultGradients()
    // {
    //     // Setup Sky Tint Gradient mặc định
    //     if (skyTintGradient == null || skyTintGradient.colorKeys.Length == 0)
    //     {
    //         skyTintGradient = new Gradient();
    //         GradientColorKey[] skyColorKeys = new GradientColorKey[5];

    //         // Đêm (0:00 - 6:00)
    //         skyColorKeys[0] = new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 0f);
    //         // Bình minh (6:00)
    //         skyColorKeys[1] = new GradientColorKey(new Color(1f, 0.5f, 0.3f), 0.25f);
    //         // Ban ngày (12:00)
    //         skyColorKeys[2] = new GradientColorKey(new Color(0.5f, 0.7f, 1f), 0.5f);
    //         // Hoàng hôn (18:00)
    //         skyColorKeys[3] = new GradientColorKey(new Color(1f, 0.4f, 0.2f), 0.75f);
    //         // Đêm (24:00)
    //         skyColorKeys[4] = new GradientColorKey(new Color(0.1f, 0.1f, 0.2f), 1f);

    //         GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    //         alphaKeys[0] = new GradientAlphaKey(1f, 0f);
    //         alphaKeys[1] = new GradientAlphaKey(1f, 1f);

    //         skyTintGradient.SetKeys(skyColorKeys, alphaKeys);
    //     }

    //     // Setup Ground Color Gradient mặc định
    //     if (groundColorGradient == null || groundColorGradient.colorKeys.Length == 0)
    //     {
    //         groundColorGradient = new Gradient();
    //         GradientColorKey[] groundColorKeys = new GradientColorKey[5];

    //         // Đêm
    //         groundColorKeys[0] = new GradientColorKey(new Color(0.05f, 0.05f, 0.1f), 0f);
    //         // Bình minh
    //         groundColorKeys[1] = new GradientColorKey(new Color(0.3f, 0.2f, 0.1f), 0.25f);
    //         // Ban ngày
    //         groundColorKeys[2] = new GradientColorKey(new Color(0.4f, 0.35f, 0.3f), 0.5f);
    //         // Hoàng hôn
    //         groundColorKeys[3] = new GradientColorKey(new Color(0.3f, 0.15f, 0.1f), 0.75f);
    //         // Đêm
    //         groundColorKeys[4] = new GradientColorKey(new Color(0.05f, 0.05f, 0.1f), 1f);

    //         GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
    //         alphaKeys[0] = new GradientAlphaKey(1f, 0f);
    //         alphaKeys[1] = new GradientAlphaKey(1f, 1f);

    //         groundColorGradient.SetKeys(groundColorKeys, alphaKeys);
    //     }
    // }

    private void OnValidate()
    {
        // Tự động tìm directional light
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    break;
                }
            }
        }

        // Tự động lấy skybox material
        if (skyboxMaterial == null)
        {
            skyboxMaterial = RenderSettings.skybox;
        }
    }
}