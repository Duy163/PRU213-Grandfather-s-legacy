using UnityEngine;

public class ShipController : MonoBehaviour
{

    // ================= State =============================
    [SerializeField] ShipManager shipManager;

    float waterDrag;
    float idleDrag;

    Rigidbody rb;
    [SerializeField] Light lamp;

    // ================= Unity Lifecycle ===================

    void OnEnable()
    {
        InputEvent.OnLampPress += ToggerLamp;
    }

    void OnDisable()
    {
        InputEvent.OnLampPress -= ToggerLamp;
    }

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

    // ================= Input Handling ====================

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

    private void ToggerLamp()
    {
        if (shipManager.playerShipData.hasLamp)
            lamp.enabled = !lamp.enabled;
    }
}
