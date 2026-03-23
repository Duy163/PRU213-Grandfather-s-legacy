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
    [SerializeField, Range(0, 24)] public float timeOfDay = 6f;
    [SerializeField] private bool enableFog = true;

    [Header("Skybox Control")]
    [SerializeField] private Material skyboxMaterial; // Material skybox (Procedural)
    [SerializeField] private Gradient skyTintGradient; // Màu sky theo thời gian
    [SerializeField] private Gradient groundColorGradient; // Màu ground theo thời gian

    private int lastSavedHour = -1;
    private bool hasTriggered6 = false;

    void Start()
    {
        if (!Application.isPlaying)
            return;

        timeOfDay = WorldDataManager.Instance.worldData.gameTime;



        // if (timeOfDay >= 6.0f && timeOfDay < 19.0f)
        // {
        //     WorldDataManager.Instance.LoadSpotWithTimeOfDay(true);
        //     TimeEvent.OnDayTime?.Invoke();
        //     hasTriggered6 = true;
        // }
        // else
        // {
        //     WorldDataManager.Instance.LoadSpotWithTimeOfDay(false);
        //     TimeEvent.OnNightTime?.Invoke();
        //     hasTriggered6 = false;
        // }
    }

    private void Update()
    {
        if (!hasTriggered6 && timeOfDay >= 6f && timeOfDay <= 19f)
        {
            WorldDataManager.Instance.worldData.gameTime += 1f;
            WorldDataManager.Instance.LoadSpotWithTimeOfDay(true);
            TimeEvent.OnDayTime?.Invoke();
            hasTriggered6 = true;
        }

        if (hasTriggered6 && timeOfDay >= 19f)
        {
            hasTriggered6 = false;
            WorldDataManager.Instance.LoadSpotWithTimeOfDay(false);
            TimeEvent.OnNightTime?.Invoke();
        }

        if (Application.isPlaying)
        {
            float delta = Application.isPlaying ? Time.deltaTime : 0f;

            timeOfDay += delta * (24f / dayLengthInSeconds);
            timeOfDay %= 24f;

            int currentHour = Mathf.FloorToInt(timeOfDay);

            if (currentHour != lastSavedHour)
            {
                lastSavedHour = currentHour;

                WorldDataManager.Instance.worldData.gameTime = currentHour;
                DataManager.Instance.Save(); // hoặc autosave
                Debug.Log($"[Time] Saved at hour {currentHour}");
            }

            WorldDataManager.Instance.UpdateClock(timeOfDay);
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
            // RenderSettings.fogMode = FogMode.Exponential;

            // if (fogDensity != null)
            // {
            //     RenderSettings.fogDensity = fogDensity.Evaluate(timePercent);
            // }
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

    public void SetDayLength()
    {
        if (dayLengthInSeconds == 1200f) dayLengthInSeconds = 20f;
        else dayLengthInSeconds = 1200f;
    }
}