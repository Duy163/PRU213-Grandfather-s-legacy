using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // ================= CONSTANTS =========================

    // ================= Serialized Fields =================

    // ================= State =============================

    // ================= Public Properties =================

    // ================= Unity Lifecycle ===================

    // ================= Input Handling ====================

    // ================= Initialization ====================

    // ================= Core Logic ========================

    // ================= Subsystem =========================

    // ================= Event Handlers ====================

    // ================= Public API ========================

    // ================= Helpers ===========================

    // ================= Debug / Editor ====================

    private DataManager dataManager;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnApplicationQuit()
    {
        DataManager.Instance.SaveGame();
    }
}
