using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    public ParticleSystem m_BulletImpact;
    [SerializeField] int m_Damage = 10;
    [SerializeField] float m_MoveVelocity = 10.0f;
    Rigidbody Rigidbody;

    bool m_hit = false;
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Rigidbody.velocity = Rigidbody.rotation * Vector3.forward * Time.deltaTime * m_MoveVelocity;
    }
    void OnCollisionStay(Collision collision)
    {
        if (!m_hit)
        {
            if (collision.gameObject.tag == "Enemy" || (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet"))
            {
                StartCoroutine(OnImpact());
            }
        }
        m_hit = true;
    }
    void OnCollisionEnter(Collision collision)
    {

        if (!m_hit)
        {
            if (collision.gameObject.tag == "Enemy" || (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet"))
            {
                StartCoroutine(OnImpact());
            }
        }
        m_hit = true;
    }
    IEnumerator OnImpact()
    {
        yield return new WaitUntil(() => !m_BulletImpact.isPlaying);
        Destroy(gameObject);
    }
    public int GetDamage()
    {
        return m_Damage;
    }
}
