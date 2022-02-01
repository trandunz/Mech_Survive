using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    #region Serialized
    [SerializeField] float m_TurnSpeed = 0.1f;
    [SerializeField] float m_MovementSpeed = 10.0f;
    [SerializeField] Transform m_WeaponHolster;
    #endregion

    #region Private
    Script_WeaponMaster m_WeaponMaster;
    int m_ActiveSlotID = 0;
    int m_OldSlotID = 0;
    List<GameObject> m_HoldsteredWeapons;
    Transform m_Camera;
    CharacterController m_Controller;
    float m_TurnSmoothVelocity;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_WeaponMaster = GameObject.FindGameObjectWithTag("WeaponMaster").GetComponent<Script_WeaponMaster>();
        m_Controller = GetComponent<CharacterController>();
        m_Camera = Camera.main.transform;
        m_HoldsteredWeapons = new List<GameObject>();
        m_HoldsteredWeapons.Add(Instantiate(m_WeaponMaster.Weapons[0].gameObject, m_WeaponHolster));

    }
    void Update()
    {
        m_OldSlotID = m_ActiveSlotID;
        HandleMovement();
        HandleWeaponSwitching();
        if (m_OldSlotID != m_ActiveSlotID)
        {
            Debug.Log("Handled Weapon Nosense");
            HandleActiveWeapon();
        }

    }
    Script_Weapon GrabCurrentWeapon()
    {
        return m_HoldsteredWeapons[m_ActiveSlotID].GetComponent<Script_Weapon>();
    }
    Vector3 GrabInput()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            input.z += 1;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;
        if (Input.GetKey(KeyCode.S))
            input.z -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        return input.normalized;
    }
    float GrabScroll()
    {
        return Input.mouseScrollDelta.y;
    }
    void HandleMovement()
    {
        Vector3 direction = new Vector3(GrabInput().x, 0.0f, GrabInput().z);
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_Camera.eulerAngles.y;
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_TurnSpeed);
        transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);
        Vector3 movedir = Quaternion.Euler(0.0f, smoothedAngle, 0.0f) * Vector3.forward;

        if (direction.magnitude >= 0.1f)
            m_Controller.Move(movedir.normalized * Time.deltaTime * m_MovementSpeed);
    }
    void HandleWeaponSwitching()
    {
        m_ActiveSlotID = ((m_ActiveSlotID + (int)GrabScroll()) >= 0) ? m_ActiveSlotID + (int)GrabScroll() : m_ActiveSlotID;
    }
    void HandleActiveWeapon()
    {
        foreach (GameObject weapon in m_HoldsteredWeapons)
        {
            weapon.GetComponent<Script_Weapon>().Disable();
        }
        if ((m_HoldsteredWeapons.Count - 1) >= m_ActiveSlotID)
        {
            m_HoldsteredWeapons[m_ActiveSlotID].GetComponent<Script_Weapon>().Enable();
        }
    }
    #endregion
}
