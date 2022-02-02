using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Weapon : MonoBehaviour
{
    public Sprite HotBarIcon;
    
    [SerializeField] GameObject m_Bullet;
    [SerializeField] Transform m_Muzzle;
    [SerializeField] float m_FireRate = 0.5f;
    Script_Recoil m_Recoil;
    ParticleSystem m_MuzzleFlash;
    AudioSource m_ShotSound;
    ParticleSystem m_BulletImpact;
    int m_BulletDamage;
    bool m_Enabled = true;
    bool m_Interacting = false;
    Ray ray;
    RaycastHit hit;
    public ParticleSystem GetMuzzleFlash()
    {
        return m_MuzzleFlash;
    }
    public AudioSource GetShotSound()
    {
        return m_ShotSound;
    }
    private void Start()
    {
        m_BulletDamage = m_Bullet.GetComponent<Script_Bullet>().GetDamage();
        m_BulletImpact = m_Bullet.GetComponent<Script_Bullet>().m_BulletImpact;
        m_MuzzleFlash = m_Muzzle.GetComponentInChildren<ParticleSystem>();
        m_ShotSound = m_MuzzleFlash.GetComponent<AudioSource>();
        m_Recoil = GetComponent<Script_Recoil>();
    }
    void Update()
    {
        if (m_Enabled)
        {
            transform.rotation = Camera.main.transform.rotation;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        m_ShotSound.Play();
        m_MuzzleFlash.Play();
        HandleRecoil();
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Enemy") | LayerMask.GetMask("Terrain") | LayerMask.GetMask("Default"), QueryTriggerInteraction.Collide))
        {
            Destroy(Instantiate(m_BulletImpact, hit.point, Quaternion.identity).gameObject, m_BulletImpact.GetComponent<AudioSource>().clip.length / 2);
            if (hit.transform.tag is "Enemy")
            {
                hit.transform.GetComponent<Script_Enemy>().TakeDamage(m_BulletDamage);
            }
        }
        yield return new WaitForSeconds(m_FireRate);
        m_Interacting = false;
    }
    IEnumerator Interact()
    {
        m_Interacting = true;
        
        yield return new WaitForSeconds(m_FireRate);
        m_Interacting = false;
    }
    void HandleRecoil()
    {
        m_Recoil.RecoilFire();
        ray.direction = m_Recoil.rotation * ray.direction;
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
