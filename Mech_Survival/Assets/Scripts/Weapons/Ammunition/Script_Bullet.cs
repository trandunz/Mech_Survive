using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] int m_Damage = 10;
    [SerializeField] float m_MoveVelocity = 10.0f;
    Rigidbody Rigidbody;
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
        if (collision.gameObject.tag == "Enemy" || (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet"))
        {
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Bullet"))
        {
            Destroy(this.gameObject);
        }
    }
    public int GetDamage()
    {
        return m_Damage;
    }
}
