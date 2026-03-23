using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    public Transform handSocket; // Kéo 'R_Weapon_Socket' vào đây trong Inspector
    public GameObject Oar;
    public GameObject Rod;
    private GameObject currentWeapon;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetPadding(bool value)
    {
        anim.SetBool("isPaddling", value);
        EquipWeapon(Oar);
    }

    public void SetFishing(bool value)
    {
        if (value == true)
            anim.SetTrigger("castRod");

        EquipFishingRod();
        anim.SetBool("isFishing", value);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // 1. Xóa vũ khí cũ nếu đang cầm
        if (currentWeapon != null) Destroy(currentWeapon);

        // 2. Tạo vũ khí mới làm con của Socket
        currentWeapon = Instantiate(weaponPrefab, handSocket);

        // 3. Reset vị trí về 0 để khớp với Socket
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(0, 180f, 90f);
    }

    public void EquipFishingRod()
    {
        // 1. Xóa vũ khí cũ nếu đang cầm
        if (currentWeapon != null) Destroy(currentWeapon);

        // 2. Tạo vũ khí mới làm con của Socket
        currentWeapon = Instantiate(Rod, handSocket);

        // 3. Reset vị trí về 0 để khớp với Socket
        currentWeapon.transform.localPosition = new Vector3(-0.6f, 0.08f, 0.1f);
        currentWeapon.transform.localRotation = Quaternion.Euler(0, 0, 90f);
    }

}