using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Weapon : MonoBehaviour
{
    [SerializeField] GameObject m_Bullet;
    [SerializeField] Transform m_Muzzle;
    [SerializeField] float m_FireRate = 0.5f;
    bool m_Enabled = true;
    bool m_Interacting = false;
    GameObject Player;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (m_Enabled)
        {
            transform.rotation = Camera.main.transform.rotation;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,1000, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Terrain") | LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide))
            {
                m_Muzzle.LookAt(hit.point);

            }
            if (Input.GetMouseButton(0))
            {
                if (!m_Interacting)
                    StartCoroutine(Fire());
            }
        }
    }
    IEnumerator Fire()
    {
        m_Interacting = true;
        Destroy(Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation), 10);
        yield return new WaitForSeconds(m_FireRate);
        m_Interacting = false;
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
