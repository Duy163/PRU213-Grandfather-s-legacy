#region Using statements

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{
    public class WaterVolumeHelper1 : MonoBehaviour
    {
        #region Private static fields

        private static WaterVolumeHelper1 instance = null;

        #endregion

        #region Public fields

        public WaterVolumeBase WaterVolume = null;

        #endregion

        #region Private Cached Fields (Tối ưu hóa Performance)

        private Material cachedMaterial = null;
        private int waveFreqID;
        private int waveScaleID;
        private int waveSpeedID;

        #endregion

        #region Public static properties

        public static WaterVolumeHelper1 Instance { get { return instance; } }

        #endregion

        #region Public methods

        public float? GetHeight(Vector3 _position)
        {
            // ensure a water volume
            if (!WaterVolume)
            {
                return 0f;
            }

            // Lazy initialization cho Material để không phải gọi GetComponent liên tục
            if (cachedMaterial == null)
            {
                var renderer = WaterVolume.gameObject.GetComponent<MeshRenderer>();
                if (renderer)
                {
                    cachedMaterial = renderer.sharedMaterial;
                }
            }

            if (!cachedMaterial)
            {
                return 0f;
            }

            // lấy base height
            float? waterHeight = null;
            try
            {
                waterHeight = WaterVolume.GetHeight(_position);
            }
            catch (System.NullReferenceException)
            {
                return null;
            }

            if (!waterHeight.HasValue)
            {
                return null;
            }

            // SỬ DỤNG ID INT THAY VÌ STRING SẼ NHANH HƠN GẤP HÀNG CHỤC LẦN
            var _WaveFrequency = cachedMaterial.GetFloat(waveFreqID);
            var _WaveScale = cachedMaterial.GetFloat(waveScaleID);
            var _WaveSpeed = cachedMaterial.GetFloat(waveSpeedID);

            var time = Time.time * _WaveSpeed;
            var shaderOffset = (Mathf.Sin(_position.x * _WaveFrequency + time) + Mathf.Cos(_position.z * _WaveFrequency + time)) * _WaveScale;

            return waterHeight.Value + shaderOffset;
        }

        #endregion

        #region MonoBehaviour events

        private void Awake()
        {
            instance = this;

            // Chuyển đổi tên string thành ID nguyên thủy ngay từ đầu để tiết kiệm CPU
            waveFreqID = Shader.PropertyToID("_WaveFrequency");
            waveScaleID = Shader.PropertyToID("_WaveScale");
            waveSpeedID = Shader.PropertyToID("_WaveSpeed");
        }

        #endregion
    }
}