using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayLengthInSeconds = 600f; // 10 phút = 1 ngày

    [Header("References")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient ambientColor;
    [SerializeField] private Gradient directionalLightColor;
    [SerializeField] private Gradient fogColor;
    [SerializeField] private AnimationCurve fogDensity;
    [Header("Variables")]
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private bool enableFog = true;

    [Header("Skybox Control")]
    [SerializeField] private Material skyboxMaterial; // Material skybox (Procedural)

    [SerializeField] private Gradient skyTintGradient; // Màu sky theo thời gian
    [SerializeField] private Gradient groundColorGradient; // Màu ground theo thời gian

    private void Update()
    {
        // if (lightPreset == null)
        //     return;

        if (Application.isPlaying)
        {
            float delta = Application.isPlaying ? Time.deltaTime : 0f;

            timeOfDay += delta * (24f / dayLengthInSeconds);
            timeOfDay %= 24f;

            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {

        UpdateSkybox(timePercent);

        RenderSettings.ambientLight = ambientColor.Evaluate(timePercent);

        UpdateFog(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = directionalLightColor.Evaluate(timePercent);

            // Xoay ánh sáng (mặt trời/mặt trăng)
            directionalLight.transform.localRotation = Quaternion.Euler(
                new Vector3((timePercent * 360f) - 90f, 170f, 0)
            );
        }
    }
    private void UpdateSkybox(float timePercent)
    {
        if (skyboxMaterial.HasProperty("_SkyTint"))
        {
            skyboxMaterial.SetColor("_SkyTint", skyTintGradient.Evaluate(timePercent));
        }

        if (skyboxMaterial.HasProperty("_GroundColor"))
        {
            skyboxMaterial.SetColor("_GroundColor", groundColorGradient.Evaluate(timePercent));
        }
    }

    private void UpdateFog(float timePercent)
    {
        RenderSettings.fogColor = fogColor.Evaluate(timePercent);

        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;

            if (fogDensity != null)
            {
                RenderSettings.fogDensity = fogDensity.Evaluate(timePercent);
            }
        }
        else
        {
            RenderSettings.fog = false;
        }
    }

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
            Light[] lights = GameObject.FindObjectsByType<Light>(FindObjectsSortMode.None);
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