using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Script_Chest : MonoBehaviour
{
    [SerializeField]
    Transform m_Hinge;

    [SerializeField]
    float m_OpenSpeed_s = 2;

    [SerializeField]
    Image m_InventoryUI;

    Quaternion m_HingeStart;
    Quaternion m_HingeDesired;

    float m_OpenValue = 0;
    bool m_Interacting = false;
    private void Start()
    {
        m_HingeStart = new Quaternion(m_Hinge.rotation.x, m_Hinge.rotation.y, m_Hinge.rotation.z, m_Hinge.rotation.w);
        m_HingeDesired = new Quaternion(m_Hinge.rotation.x, m_Hinge.rotation.y, m_Hinge.rotation.z, m_Hinge.rotation.w);
    }
    private void Update()
    {
        if (m_Interacting)
        {
            if (m_OpenValue < 1)
            {
                m_OpenValue += Time.deltaTime / m_OpenSpeed_s;
                m_Hinge.rotation = Quaternion.Lerp(m_Hinge.rotation, m_HingeDesired, m_OpenValue);
            }
            else if (m_OpenValue >= 1)
            {
                m_Interacting = false;
                m_OpenValue = 0;
            }
        }
    }
    public void Interact()
    {
        Debug.Log("Interact With Chest");
        if (m_Hinge.transform.rotation == m_HingeStart)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
    void Open()
    {
        m_Interacting = true;
        m_OpenValue = 0;
       /* m_InventoryUI.gameObject.SetActive(true);*/
        m_HingeDesired = Quaternion.Euler(m_HingeDesired.x + 90, m_HingeDesired.y, m_HingeDesired.z);
    }
    void Close()
    {
        m_Interacting = true;
        m_OpenValue = 0;
       /* m_InventoryUI.gameObject.SetActive(false);*/
        m_HingeDesired = new Quaternion(m_HingeStart.x, m_HingeStart.y, m_HingeStart.z, m_HingeStart.w);
    }
}
