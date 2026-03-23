#region Using statements

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{
    public class WateverVolumeFloater1 : MonoBehaviour
    {
        #region Public fields

        public WaterVolumeHelper1 WaterVolumeHelper = null;

        [Header("Height Settings")]
        [Tooltip("Điều chỉnh độ cao của thuyền so với mặt nước. Số dương để nâng lên, số âm để hạ xuống.")]
        public float heightOffset = 0.5f;

        [Header("Tilt Settings")]
        [Tooltip("Khoảng cách lấy mẫu để đo độ dốc sóng. Để nhỏ (vd: 0.1 tới 0.5) để đo chính xác bề mặt")]
        public float sampleLength = 0.5f;

        [Tooltip("Tốc độ nghiêng của vật thể. Giúp thuyền nổi mượt mà, có cảm giác nặng/quán tính")]
        public float rotationSpeed = 5f;

        #endregion

        #region MonoBehaviour events

        void Update()
        {
            var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper1.Instance;
            if (!instance)
            {
                return;
            }

            // 1. CẬP NHẬT ĐỘ CAO (Vị trí Trung tâm)
            Vector3 centerPos = transform.position;
            float centerHeight = instance.GetHeight(centerPos) ?? centerPos.y;

            // Cập nhật vị trí Y ngay lập tức
            transform.position = new Vector3(centerPos.x, centerHeight + heightOffset, centerPos.z);

            // 2. TÍNH TOÁN ĐỘ NGHIÊNG CỦA SÓNG (MỚI)
            // Lấy mẫu độ cao tại 2 điểm xung quanh trên trục Thế giới (World Space)
            Vector3 forwardPos = centerPos + Vector3.forward * sampleLength;
            float forwardHeight = instance.GetHeight(forwardPos) ?? centerPos.y;

            Vector3 rightPos = centerPos + Vector3.right * sampleLength;
            float rightHeight = instance.GetHeight(rightPos) ?? centerPos.y;

            // Tạo các vector chỉ hướng từ tâm ra 2 điểm đó (tạo thành 2 cạnh của tam giác trên mặt nước)
            Vector3 centerToForward = new Vector3(0, forwardHeight - centerHeight, sampleLength);
            Vector3 centerToRight = new Vector3(sampleLength, rightHeight - centerHeight, 0);

            // Dùng Tích có hướng (Cross Product) để tìm ra Vector Pháp Tuyến vuông góc với mặt nước
            Vector3 waterNormal = Vector3.Cross(centerToForward, centerToRight).normalized;

            // Đảm bảo vector hợp lệ trước khi tính toán góc xoay
            if (waterNormal != Vector3.zero)
            {
                // Giữ nguyên hướng quay đầu hiện tại của vật thể (Yaw), chỉ nghiêng trục X và Z
                Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, waterNormal).normalized;

                // Tạo góc xoay mới dựa trên hướng nhìn và độ nghiêng của nước
                Quaternion targetRotation = Quaternion.LookRotation(projectedForward, waterNormal);

                // Dùng Slerp để xoay vật thể từ từ, tạo cảm giác mượt mà
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        #endregion
    }
}