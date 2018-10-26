using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Range(0.0f, 50.0f)] float m_acceleration = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_maxSpeed = 10.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_idleFriction = 5.0f;
    [Header("Rotation")]
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationAcceleration = 90.0f;
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationMaxSpeed = 180.0f;
    [SerializeField] [Range(0.0f, 900.0f)] float m_rotationIdleFriction = 90.0f;
    [Header("Jumping")]
    [SerializeField] [Range(0.0f, 50.0f)] float m_jumpForce = 15.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_airControl = 0.5f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_jumpResistance = 3.0f;
    [SerializeField] [Range(0.0f, 20.0f)] float m_fallSpeed = 3.0f;
    [Header("Ground")]
    [SerializeField] Transform m_groundTouch = null;
    [SerializeField] LayerMask m_groundMask = 0;
    [Header("Camera")]
    [SerializeField] Camera m_camera;
    
    public bool OnGround { get; private set; }

    Animator m_animator;
    Rigidbody m_rigidbody;
    Quaternion m_lastRotation;
    Vector3 m_velocity;
    Vector3 m_rotation;
    bool m_canMove;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_canMove = true;
    }

    private void Update()
    {
        Collider[] points = Physics.OverlapSphere(m_groundTouch.position, 0.15f, m_groundMask);
        OnGround = points.Length > 0;
        m_animator.SetBool("OnGround", OnGround);

        transform.rotation *= Quaternion.Euler(m_rotation);

        float magnitude = m_velocity.magnitude;
        Quaternion camRot = m_camera.transform.rotation;
        Vector3 rotatedVelocity = /*camRot * */ m_velocity;
        rotatedVelocity.y = 0.0f;
        rotatedVelocity = rotatedVelocity.normalized * magnitude;

        if (OnGround)
        {
            transform.position += transform.rotation * rotatedVelocity;
            m_lastRotation = transform.rotation;
        }
        else
        {
            transform.position += m_lastRotation * rotatedVelocity;
        }


        if (Input.GetButtonDown("Jump") && OnGround && m_canMove)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        float inZ = Input.GetAxis("Vertical");
        if (OnGround && m_canMove)
        {
            float speed = m_acceleration * Time.deltaTime;
            m_velocity.z += inZ * speed;
            if (m_velocity.magnitude > m_maxSpeed)
            {
                m_velocity = m_velocity.normalized * m_maxSpeed;
            }

            if (inZ == 0.0f && Mathf.Abs(m_velocity.z) > 0.05f)
            {
                float opp = m_velocity.z >= 0.0f ? -1.0f : 1.0f;
                m_velocity.z += opp * m_idleFriction * Time.deltaTime;
            }
            else if (inZ == 0.0f && Mathf.Abs(m_velocity.z) < 0.05f)
            {
                m_velocity.z = 0.0f;
            }
        }

        float forward = m_velocity.z >= 0.0f ? 1.0f : -1.0f;
        float rotSpeed = m_rotationAcceleration * forward * Time.deltaTime;
        float inR = Input.GetAxis("Horizontal");
        float airControl = OnGround ? 1.0f : m_airControl;
        m_rotation.y += inR * rotSpeed * airControl;
        if (m_rotation.magnitude > m_rotationMaxSpeed)
        {
            m_rotation = m_rotation.normalized * m_rotationMaxSpeed;
        }

        if (inR == 0.0f && Mathf.Abs(m_rotation.y) > 0.1f)
        {
            float opp = m_rotation.y >= 0.0f ? -1.0f : 1.0f;
            m_rotation.y += opp * m_rotationIdleFriction * Time.deltaTime;
        }
        else if (inR == 0.0f && Mathf.Abs(m_rotation.y) < 0.1f)
        {
            m_rotation.y = 0.0f;
        }
        
        bool right = m_rotation.y > 0.0f;
        m_animator.SetBool("TurnRight", right);
        bool turn = m_rotation.magnitude > 0.5f;
        m_animator.SetBool("Turn", turn);

        if (m_rigidbody.velocity.y > 0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < 0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallSpeed - 1.0f) * Time.deltaTime;
        }

        float runSpeed = Mathf.Abs(m_velocity.z) + inZ * 0.05f;
        m_animator.SetFloat("RunSpeed", runSpeed);

        float dir = m_velocity.z > 0.0f ? 1.0f : -1.0f;
        m_animator.SetFloat("RunDirection", dir);
    }

    public void Jump()
    {
        Vector3 jumpForce = Vector3.up * m_jumpForce;
        m_rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        m_rotation = Vector3.zero;
    }

    public void DisableMovement()
    {
        m_canMove = false;
    }

    public void EnableMovement()
    {
        m_canMove = true;
    }
}
