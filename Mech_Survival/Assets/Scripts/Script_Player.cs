using UnityEngine;

public class Script_Player : MonoBehaviour
{
    [SerializeField] Transform Camera;
    [SerializeField] float m_MovementSpeed = 10.0f;
    CharacterController controller;

    [SerializeField] float m_TurnSpeed = 0.1f;
    float turnsmoothVelocity;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 direction = new Vector3(GrabInput().x, 0.0f, GrabInput().z).normalized;
        float TargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, TargetAngle, ref turnsmoothVelocity, m_TurnSpeed);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        Vector3 movedir = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
        if (direction.magnitude >= 0.1f)
            controller.Move(movedir.normalized * Time.deltaTime * m_MovementSpeed);
    }

    Vector3 GrabInput()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            input.z += 1;
        if (Input.GetKey(KeyCode.A))
            input.x -= 1;
        if (Input.GetKey(KeyCode.S))
            input.z -= 1;
        if (Input.GetKey(KeyCode.D))
            input.x += 1;
        return input;
    }
}
