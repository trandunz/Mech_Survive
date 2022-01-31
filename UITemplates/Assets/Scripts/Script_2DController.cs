using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_2DController : MonoBehaviour
{
    [SerializeField] float m_ScrollSpeed = 50.0f;
    [SerializeField] float m_MovementSpeed = 5.0f;
    [SerializeField] Camera m_Camera;
    [SerializeField] GameObject m_Weapon;
    Script_Gun Gun;


    // Update is called once per frame
    private void Start()
    {
        Gun = m_Weapon.GetComponentInChildren<Script_Gun>();
    }
    void Update()
    {
        HandleCamZoom();
        transform.Translate(new Vector3(ReturnInput().x,0 , ReturnInput().y) * Time.deltaTime * m_MovementSpeed) ;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            m_Weapon.transform.LookAt(hit.point);
        }
        m_Camera.transform.position = new Vector3(transform.position.x, m_Camera.transform.position.y, transform.position.z);

        m_Weapon.transform.rotation = new Quaternion(0, m_Weapon.transform.rotation.y, 0, m_Weapon.transform.rotation.w);
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Gun.Fire();
        }
    }

    void HandleCamZoom()
    {
        m_Camera.transform.Translate(new Vector3(0, 0, ReturnScroll()) * Time.deltaTime * m_ScrollSpeed);
    }
    float ReturnScroll()
    {
        return Input.mouseScrollDelta.y;
    }

    Vector2 ReturnInput()
    {
        Vector2 inputVec;
        inputVec = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVec.y += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVec.x -= 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVec.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVec.x += 1;
        }
        return inputVec;
    }
}
