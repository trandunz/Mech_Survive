using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Weapon : MonoBehaviour
{
    [SerializeField] GameObject m_Bullet;
    [SerializeField] Transform m_Muzzle;
    bool m_Enabled = false;
    public void Fire()
    {
        Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation, m_Bullet.transform);
    }
    public void Enable()
    {
        m_Enabled = true;
        foreach(SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            mesh.enabled = true;
        }
    }
    public void Disable()
    {
        m_Enabled = false;
        foreach (SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            mesh.enabled = false;
        }
    }

}
