using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Gun : MonoBehaviour
{
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] Transform m_Muzzel;
    [SerializeField] float m_Firerate_s = 2;
    float m_firerateecountdown;
    private void Update()
    {
        if (m_firerateecountdown > 0)
        {
            m_firerateecountdown -= Time.deltaTime;
        }
    }
    public void Fire()
    {
        if (m_firerateecountdown <= 0)
        {
            m_firerateecountdown = m_Firerate_s;
            Instantiate(m_BulletPrefab, m_Muzzel.position, new Quaternion(0, m_Muzzel.rotation.y, 0, m_Muzzel.rotation.w));
        }
    }
        
}
