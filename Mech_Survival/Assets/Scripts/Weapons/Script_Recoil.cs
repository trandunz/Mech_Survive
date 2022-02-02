using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Recoil : MonoBehaviour
{
    public Quaternion rotation;
    Vector3 m_CurrentRotation;
    Vector3 m_TargetRotation;

    [SerializeField] float m_RecoilX;
    [SerializeField] float m_RecoilY;
    [SerializeField] float m_RecoilZ;

    [SerializeField] float m_Smoothing;
    [SerializeField] float m_ReturnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_TargetRotation = Vector3.Lerp(m_TargetRotation, Vector3.zero, m_ReturnSpeed * Time.deltaTime);
        m_CurrentRotation = Vector3.Lerp(m_CurrentRotation, m_TargetRotation, m_Smoothing * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(m_CurrentRotation) * transform.localRotation;
        rotation = Quaternion.Euler(m_CurrentRotation); ;
    }

    public void RecoilFire()
    {
        m_TargetRotation += new Vector3(m_RecoilX, Random.Range(-m_RecoilY, m_RecoilY), Random.Range(-m_RecoilZ, m_RecoilZ));
    }
}
