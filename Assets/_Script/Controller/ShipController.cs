using UnityEngine;

public class ShipController : MonoBehaviour
{
    // ================= CONSTANTS =========================

    // ================= Serialized Fields =================

    // ================= State =============================

    float acceleration;
    float maxSpeed;
    float turnStrength;

    float waterDrag;
    float idleDrag;

    Rigidbody rb;

    // ================= Public Properties =================

    // ================= Unity Lifecycle ===================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        // rb.constraints = RigidbodyConstraints.FreezeRotationX
        //        | RigidbodyConstraints.FreezeRotationZ;

        LoadShipData();
    }

    void FixedUpdate()
    {
        Vector2 move = InputManager.Instance.GetMovement();
        float h = move.x;
        float v = move.y;

        // Di chuyển
        if (v != 0)
            rb.AddForce(transform.forward * v * acceleration, ForceMode.Acceleration);

        // Giới hạn tốc độ
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Xoay
        if (rb.linearVelocity.magnitude > 0.5f)
            rb.AddTorque(Vector3.up * h * turnStrength, ForceMode.Acceleration);

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

    // ================= Input Handling ====================

    // ================= Initialization ====================

    private void LoadShipData()
    {
        PlayerShipData playerShipData = DataManager.Instance.currentGameData.playerShipData;

        acceleration = playerShipData.acceleration;
        maxSpeed = playerShipData.maxSpeed;
        turnStrength = playerShipData.turnStrength;

        waterDrag = playerShipData.waterDrag;
        idleDrag = playerShipData.idleDrag;
    }

    // ================= Core Logic ========================

    private void ApplyForwardMovement(float verticalInput)
    {
        if (Mathf.Abs(verticalInput) < 0.01f)
            return;

        Vector3 forwardForce = transform.forward * verticalInput * acceleration;
        rb.AddForce(forwardForce, ForceMode.Acceleration);
    }

    // private void ApplyTurning(float horizontalInput)
    // {
    //     if (Mathf.Abs(horizontalInput) < 0.01f)
    //         return;

    //     // Only turn when boat is moving
    //     if (rb.linearVelocity.magnitude < minSpeedToTurn)
    //         return;

    //     Vector3 turnTorque = Vector3.up * horizontalInput * turnStrength;
    //     rb.AddTorque(turnTorque, ForceMode.Acceleration);
    // }

    // ================= Subsystem =========================

    // ================= Event Handlers ====================

    // ================= Public API ========================

    // ================= Helpers ===========================

    // ================= Debug / Editor ====================
}
