using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    #region Public
    public Cinemachine.CinemachineVirtualCamera m_FPSCam;
    public string GamerTag = "Human";
    #endregion

    #region Serialized
    [SerializeField] float m_TurnSpeed = 0.1f,
                           m_MovementSpeed = 10.0f;
    [SerializeField] Transform m_WeaponHolster;
    [SerializeField] float m_InteractRange = 3.0f;
    [SerializeField] Cinemachine.CinemachineVirtualCamera m_ThirdPersonCam;
    #endregion

    #region Private
    [SerializeField] bool m_IsEnabled = true;
    bool m_FirstPerson = true;
    Script_WeaponMaster m_WeaponMaster;
    int m_ActiveSlotID = 0;
    int m_OldSlotID = 0;
    List<GameObject> m_HoldsteredWeapons;
    Transform m_Camera;
    CharacterController m_Controller;
    float m_TurnSmoothVelocity;
    Ray InteractRay;
    RaycastHit InteractHit;
    public void SetFunctionallityEnabled(bool _value)
    {
        m_IsEnabled = _value;
    }
    public void ToggleFunctionallity()
    {
        m_IsEnabled = !m_IsEnabled;
    }
    public List<GameObject> GetAllWeapons()
    {
        return m_HoldsteredWeapons;
    }
    void Start()
    {
        m_FPSCam = GameObject.FindGameObjectWithTag("FPSCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        m_ThirdPersonCam = GameObject.FindGameObjectWithTag("3RDCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        Cursor.lockState = CursorLockMode.Locked;
        m_WeaponMaster = GameObject.FindGameObjectWithTag("WeaponMaster").GetComponent<Script_WeaponMaster>();
        m_Controller = GetComponent<CharacterController>();
        m_Camera = Camera.main.transform;
        m_HoldsteredWeapons = new List<GameObject>();
        m_HoldsteredWeapons.Add(Instantiate(m_WeaponMaster.Weapons[3].gameObject, m_WeaponHolster));
        m_HoldsteredWeapons.Add(Instantiate(m_WeaponMaster.Weapons[2].gameObject, m_WeaponHolster));
        m_HoldsteredWeapons.Add(Instantiate(m_WeaponMaster.Weapons[1].gameObject, m_WeaponHolster));
        m_HoldsteredWeapons.Add(Instantiate(m_WeaponMaster.Weapons[0].gameObject, m_WeaponHolster));
        HandleActiveWeapon();
    }
    void Update()
    {
        float m_mouseYTotal = 0.0f;
        m_mouseYTotal = Input.mousePosition.y;
        m_FPSCam.transform.rotation = Quaternion.Euler(0.0f, 1.0f * 20.0f * Time.deltaTime, 0.0f);
        if (m_IsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                m_FirstPerson = !m_FirstPerson;
                HandleCameraState();
            }

            m_OldSlotID = m_ActiveSlotID;
            HandleMovement();
            HandleWeaponSwitching();
            if (m_OldSlotID != m_ActiveSlotID)
            {
                Debug.Log("Handled Weapon Nosense");
                HandleActiveWeapon();
            }
            HandleInteract();
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
    void HandleCameraState()
    {
        if (m_FirstPerson)
        {
            m_FPSCam.Priority = 10;
        }
        else
        {
            m_FPSCam.Priority = 9;
        }
    }
    void HandleMovement()
    {
        Vector3 direction = new Vector3(GrabInput().x, 0.0f, GrabInput().z);
        float targetAngle;
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_Camera.eulerAngles.y;

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
    void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(InteractRay, out InteractHit, m_InteractRange, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Terrain") | LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide))
            {
                if (InteractHit.transform.gameObject.tag is "Chest")
                {
                    InteractHit.transform.GetComponentInParent<Script_Chest>().Interact();
                }
            }
        }
    }
    #endregion
}
