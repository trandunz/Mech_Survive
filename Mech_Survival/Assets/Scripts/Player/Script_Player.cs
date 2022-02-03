using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    #region Public
    public Cinemachine.CinemachineVirtualCamera m_FPSCam;
    public string GamerTag = "Human";
    #endregion

    #region Serialized
    [SerializeField]
    float 
    m_TurnSpeed = 0.1f,
    m_MovementSpeed = 10.0f,
    m_GravityValue = -9.81f,
    m_JumpHeight = 5.0f,
    m_InteractRange = 3.0f,
    m_RaycastGroundLength = 3.0f;

    [SerializeField] Transform m_WeaponHolster;
    [SerializeField] Cinemachine.CinemachineVirtualCamera m_ThirdPersonCam;
    [SerializeField] LayerMask GroundedLayer;
    #endregion

    #region Private
    float m_YMove = 0.0f,
          m_TurnSmoothVelocity = 0;
    int m_OldSlotID = 0,
        m_ActiveSlotID = 0;
    bool m_IsEnabled = true,
         m_Grounded = true,
         m_FirstPerson = true;
    List<GameObject> m_HoldsteredWeapons;
    Transform m_Camera;
    CharacterController m_Controller;
    Ray InteractRay;
    RaycastHit InteractHit;
    Script_WeaponMaster m_WeaponMaster;

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
        if (m_IsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                m_FirstPerson = !m_FirstPerson;
                HandleCameraState();
            }

            m_Grounded = (m_Controller.collisionFlags == CollisionFlags.CollidedBelow) ? true : m_Grounded;
            HandleMovement();
            HandleWeaponSwitching();
            HandleActiveWeapon();
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
        if (Input.GetKeyDown(KeyCode.Space))
            input.y += 1;
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
        RayCastIsGrounded();
        Vector3 direction = new Vector3(GrabInput().x, 0, GrabInput().z);
        float targetAngle;
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + m_Camera.eulerAngles.y;
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref m_TurnSmoothVelocity, m_TurnSpeed);
        Vector3 movedir = Quaternion.Euler(0.0f, smoothedAngle, 0.0f) * Vector3.forward;
        movedir.Normalize();

        transform.rotation = Quaternion.Euler(0.0f, smoothedAngle, 0.0f);

        if (direction.magnitude >= 0.1f)
            m_Controller.Move(movedir * Time.deltaTime * m_MovementSpeed);

        if (m_Grounded && m_YMove < 0)
        {
            m_YMove = -0.1f;
        }
        if (GrabInput().y > 0)
        {
            Jump();
        }
        Gravity();
    }
    void HandleWeaponSwitching()
    {
        m_OldSlotID = m_ActiveSlotID;
        m_ActiveSlotID = ((m_ActiveSlotID + (int)GrabScroll()) >= 0) ? m_ActiveSlotID + (int)GrabScroll() : m_ActiveSlotID;
    }
    void HandleActiveWeapon()
    {
        if (m_OldSlotID != m_ActiveSlotID)
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
    }

    void RayCastIsGrounded()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, m_RaycastGroundLength, GroundedLayer))
        {
            m_Grounded = true;
        }
        else
        {
            m_Grounded = false;

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
    void Jump()
    {
        if (m_Grounded)
            m_YMove += Mathf.Sqrt(m_JumpHeight * -m_GravityValue);
    }
    void Gravity()
    {
        m_YMove += m_GravityValue * Time.deltaTime;
        m_Controller.Move(Vector3.up * m_YMove * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down * m_RaycastGroundLength);

    }
    #endregion
}
