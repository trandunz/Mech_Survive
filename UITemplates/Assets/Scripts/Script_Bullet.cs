using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] float m_TravelVelocity;
    [SerializeField] float m_Damage = 10;
    private void Awake()
    {
        GetComponent<Rigidbody>().velocity += transform.rotation * Vector3.forward * m_TravelVelocity;
    }
    void FixedUpdate()
    {
        
        /*transform.position += transform.forward * Time.deltaTime * m_TravelVelocity;*/
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
           
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit");
                collision.gameObject.GetComponent<Script_Enemy>().TakeDamage(m_Damage);
            }
            Destroy(gameObject);
        }
    }
    public void OnCollision(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("Hit");
                collision.gameObject.GetComponent<Script_Enemy>().TakeDamage(m_Damage);
            }
            Destroy(gameObject);
        }
    }
}
