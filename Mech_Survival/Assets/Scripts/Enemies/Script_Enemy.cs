using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Enemy : MonoBehaviour
{
    [SerializeField] float m_MaxHealth = 100.0f;
    [SerializeField] float m_Health;
    bool m_TakingDamage = false;
    bool m_IsDead = false;

    void Start()
    {
        m_Health = m_MaxHealth;
    }
    void Update()
    {
        if (m_Health <= 0 && !m_IsDead)
        {
            StartCoroutine(Death());
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag  is "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Script_Bullet>().GetDamage());
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag is "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Script_Bullet>().GetDamage());
        }
    }
    public void TakeDamage(int _amount)
    {
        if (!m_TakingDamage)
        {
            StartCoroutine(Damage(_amount));
        }
    }
    IEnumerator Damage(int _amount)
    {
        m_TakingDamage = true;
        for (int i = _amount; m_Health > 0; i--)
        {
            m_Health--;
        }
        yield return new WaitForSeconds(0.25f);
        m_TakingDamage = false;
    }
    IEnumerator Death()
    {
        m_IsDead = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator Revive()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        m_IsDead = false;
    }
}
