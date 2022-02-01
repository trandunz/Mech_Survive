using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Bullet : MonoBehaviour
{
    [SerializeField] float m_MoveVelocity = 10.0f;
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * m_MoveVelocity);
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
