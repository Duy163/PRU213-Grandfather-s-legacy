using System.Security.Cryptography;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    // ================= State =============================
    [SerializeField] ShipManager shipManager;
    [SerializeField] PlayerController playerController;

    float waterDrag;
    float idleDrag;
    bool isRunning = false;

    Rigidbody rb;
    [SerializeField] Transform propeller;

    [Header("Visuals")] // MỚI THÊM: Tạo một mục cho dễ nhìn trong Inspector
    [SerializeField] float propellerSpeed = 800f; // MỚI THÊM: Tốc độ xoay chân vịt

    [SerializeField] Transform motorPivot; // Kéo thả MotorPivot tạo ở Bước 1 vào đây
    [SerializeField] float maxTiltAngle = 10f; // Góc nghiêng tối đa (độ)
    [SerializeField] float tiltSpeed = 5f; // Tốc độ nghiêng (càng lớn càng nhanh)

    // ================= Unity Lifecycle ===================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void FixedUpdate()
    {
        Vector2 move = InputManager.Instance.GetMovement();
        float h = move.x;
        float v = move.y;

        if (propeller != null)
        {
            float forwardSpeed = transform.InverseTransformDirection(rb.linearVelocity).z;
            float normalizedSpeed = forwardSpeed / shipManager.playerShipData.maxSpeed;

            if (Mathf.Abs(normalizedSpeed) > 0.01f)
            {
                propeller.Rotate(Vector3.right * normalizedSpeed * propellerSpeed * Time.fixedDeltaTime);

                if (!isRunning)
                {
                    isRunning = true;
                    playerController.SetPadding(isRunning);
                    AudioManager.Instance.PlayShipMove(isRunning);
                }
                AudioManager.Instance.UpdateShipEnginePitch(Mathf.Abs(normalizedSpeed));
            }
            else
            {
                // Nếu tàu đã dừng nhưng trạng thái âm thanh vẫn đang bật -> Tắt đi
                if (isRunning)
                {
                    isRunning = false;
                    playerController.SetPadding(isRunning);
                    AudioManager.Instance.PlayShipMove(isRunning);
                }
            }
        }

        if (motorPivot != null)
        {
            float targetAngle = h * maxTiltAngle; // maxTiltAngle giả sử là 30 độ
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            motorPivot.localRotation = Quaternion.Lerp(motorPivot.localRotation, targetRotation, Time.fixedDeltaTime * tiltSpeed);
        }

        // Di chuyển
        if (v != 0)
            rb.AddForce(transform.forward * v * shipManager.playerShipData.acceleration, ForceMode.Acceleration);

        // Giới hạn tốc độ
        if (rb.linearVelocity.magnitude > shipManager.playerShipData.maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * shipManager.playerShipData.maxSpeed;

        // Xoay
        if (rb.linearVelocity.magnitude > 0.5f)
            rb.AddTorque(Vector3.up * h * shipManager.playerShipData.turnStrength, ForceMode.Acceleration);

        // Chống trôi ngang
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = Mathf.Lerp(localVel.x, 0, 0.2f);
        rb.linearVelocity = transform.TransformDirection(localVel);

        // Drag
        rb.linearDamping = (Mathf.Abs(v) > 0.1f) ? waterDrag : idleDrag;

        // Giới hạn tốc độ xoay
        rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, 1.2f);

        // Giảm quán tính xoay khi không bấm A/D
        if (h == 0)
        {
            rb.angularVelocity = Vector3.Lerp(
                rb.angularVelocity,
                Vector3.zero,
                0.12f
            );
        }
    }
    // ================= Initialization ====================

    public void LoadShipData()
    {
        var transform = GetComponent<Transform>();
        Vector3 temp = transform.position;
        Vector3 rot = new Vector3(0, 0, 0);

        temp.x = shipManager.playerShipData.position.x;
        temp.z = shipManager.playerShipData.position.z;
        rot.y = shipManager.playerShipData.rotation.y;

        transform.position = temp;
        transform.eulerAngles = rot;

        waterDrag = shipManager.playerShipData.waterDrag;
        idleDrag = shipManager.playerShipData.idleDrag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > 2f)
        {
            AudioManager.Instance.PlayShipCrash(impactForce);
        }
    }
}
