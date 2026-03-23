using UnityEngine;

public class InfiniteOcean : MonoBehaviour
{
    [Tooltip("Kéo con thuyền (OldShip1) hoặc Camera vào đây")]
    public Transform target;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Chỉ đi theo trục X và Z của mục tiêu. Giữ nguyên độ cao Y của mặt biển.
            Vector3 newPos = transform.position;
            newPos.x = target.position.x;
            newPos.z = target.position.z;

            transform.position = newPos;
        }
    }
}