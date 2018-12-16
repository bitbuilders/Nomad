using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    [System.Serializable]
    public enum PlayerState
    {
        NONE = 1,
        CHAT_ROOM = 2,
        EMOTE = 4,
        DIRECT_MESSAGE = 8,
        PARTY_MESSAGE = 16,
        IN_AIR = 32,
        DIALOG = 64,
        GAME = 128
    }

    [Header("Movement")]
    [SerializeField] [Range(0.0f, 50.0f)] float m_acceleration = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_maxSpeed = 10.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_idleFriction = 5.0f;
    [SerializeField] [Range(0.0f, 5.0f)] float m_idleCountdown = 0.01f;
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
    [SerializeField] LayerMask m_playerMask = 0;
    [Header("Camera")]
    [SerializeField] Camera m_camera;
    [Header("Network")]
    [SerializeField] [Range(0.0f, 20.0f)] float m_movementUpdateRate = 0.1f;
    
    public bool OnGround { get; private set; }
    public bool TouchingPlayer { get; private set; }
    public PlayerState State { get; private set; }

    Animator m_animator;
    Rigidbody m_rigidbody;
    Quaternion m_lastRotation;
    GameObject m_childAvatar;
    PlayerState m_cannotMoveState;
    Vector3 m_velocity;
    Vector3 m_rotation;
    float m_rigidFactor = 80.0f;
    float m_updateTime;
    float m_idleTime;
    bool m_idle;

    private void OnEnable()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_childAvatar = m_animator.gameObject;
        
        m_idle = true;

        m_cannotMoveState = (PlayerState.CHAT_ROOM | PlayerState.DIRECT_MESSAGE | PlayerState.PARTY_MESSAGE | PlayerState.DIALOG | PlayerState.GAME);
    }

    private void Update()
    {
        Collider[] points = Physics.OverlapSphere(m_groundTouch.position, 0.231f, m_groundMask);
        OnGround = points.Length > 0;
        m_animator.SetBool("OnGround", OnGround);
        Collider[] points2 = Physics.OverlapSphere(m_groundTouch.position, 0.233f, m_playerMask);
        TouchingPlayer = true;
        foreach (Collider c in points2)
        {
            if (c.gameObject == gameObject && points2.Length == 1)
            {
                TouchingPlayer = false;
                break;
            }
        }

        if (!isLocalPlayer)
            return;

        m_updateTime += Time.deltaTime;
        if (m_updateTime >= m_movementUpdateRate)
        {
            m_updateTime = 0.0f;
            CmdUpdateMotionData(m_velocity, m_rotation, m_idle);
        }

        if (!CanMove())
            return;

        if (Input.GetButtonDown("Jump") && OnGround && !HasState(PlayerState.IN_AIR))
        {
            CmdSendAnimationTrigger("Jump");
            m_animator.SetTrigger("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer && CanMove())
            UpdateMovement();
        else if (isLocalPlayer)
        {
            m_rotation = Vector3.zero;
            m_velocity = Vector3.zero;
            m_rigidbody.velocity = new Vector3(0.0f, m_rigidbody.velocity.y, 0.0f);
            m_idle = true;
        }

        UpdateAnimator();
    }

    void UpdateMovement()
    {
        float inZ = Input.GetAxis("Vertical");
        if (inZ == 0.0f)
        {
            m_idleTime += Time.deltaTime;
            if (m_idleTime >= m_idleCountdown)
            {
                m_idle = true;
            }
        }
        else
        {
            m_idle = false;
            m_idleTime = 0.0f;
        }

        if (OnGround && !HasState(PlayerState.IN_AIR))
        {
            float speed = m_acceleration * Time.deltaTime;
            m_velocity.z += inZ * speed;
            if (m_velocity.magnitude > m_maxSpeed)
            {
                m_velocity = m_velocity.normalized * m_maxSpeed;
            }
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

        if (TouchingPlayer)
        {
            m_velocity = new Vector3(0.0f, m_rigidbody.velocity.y, 0.0f);
            m_rigidbody.velocity = m_velocity;
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

        if (m_rigidbody.velocity.y > 0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_jumpResistance - 1.0f) * Time.deltaTime;
        }
        else if (m_rigidbody.velocity.y < 0.1f)
        {
            m_rigidbody.velocity += (Vector3.up * Physics.gravity.y) * (m_fallSpeed - 1.0f) * Time.deltaTime;
        }

        m_childAvatar.transform.rotation *= Quaternion.Euler(m_rotation);

        float magnitude = m_velocity.magnitude;
        Quaternion camRot = m_camera.transform.rotation;
        Vector3 rotatedVelocity = /*camRot * */ m_velocity;
        rotatedVelocity.y = 0.0f;
        rotatedVelocity = rotatedVelocity.normalized * magnitude;

        Vector3 rigidVelocity = new Vector3(m_velocity.x * m_rigidFactor, m_rigidbody.velocity.y, m_velocity.z * m_rigidFactor);
        if (OnGround)
        {
            m_rigidbody.velocity = m_childAvatar.transform.rotation * rigidVelocity;
            //transform.position += m_childAvatar.transform.rotation * rotatedVelocity;
            m_lastRotation = m_childAvatar.transform.rotation;
        }
        else
        {
            m_rigidbody.velocity = m_lastRotation * rigidVelocity;
            //transform.position += m_lastRotation * rotatedVelocity;
        }
    }

    void UpdateAnimator()
    {
        float runSpeed = m_idle ? 0.0f : m_velocity.magnitude + 0.02f;
        m_animator.SetFloat("RunSpeed", runSpeed);

        float dir = m_velocity.z > 0.0f ? 1.0f : -1.0f;
        m_animator.SetFloat("RunDirection", dir);

        bool right = m_rotation.y > 0.0f;
        m_animator.SetBool("TurnRight", right);
        bool turn = m_rotation.magnitude > 0.5f;
        m_animator.SetBool("Turn", turn);
    }

    public void Jump()
    {
        Vector3 jumpForce = Vector3.up * m_jumpForce;
        m_rigidbody.AddForce(jumpForce, ForceMode.Impulse);
        m_rotation = Vector3.zero;
    }

    public void DisableMovement()
    {
        AddState(PlayerState.IN_AIR);
    }

    public void EnableMovement()
    {
        RemoveState(PlayerState.IN_AIR);
    }

    [Command]
    void CmdUpdateMotionData(Vector3 velocity, Vector3 rotation, bool idle)
    {
        RpcReceiveMotionData(velocity, rotation, idle);
    }

    [ClientRpc]
    void RpcReceiveMotionData(Vector3 velocity, Vector3 rotation, bool idle)
    {
        m_velocity = velocity;
        m_rotation = rotation;
        m_idle = idle;
    }

    [Command]
    void CmdSendAnimationTrigger(string trigger)
    {
        RpcRecieveAnimationTrigger(trigger);
    }

    [ClientRpc]
    void RpcRecieveAnimationTrigger(string trigger)
    {
        if (!isLocalPlayer)
            m_animator.SetTrigger(trigger);
    }

    public void AddState(PlayerState state)
    {
        State |= state;
    }

    public void RemoveState(PlayerState state)
    {
        State &= ~state;
    }

    public bool HasState(PlayerState state)
    {
        return (State & state) == state;
    }

    public bool ContainsStates(PlayerState states)
    {
        return (~(State & states) & states) != states;
    }

    public bool CanMove()
    {
        bool canRotate = true;

        if (ContainsStates(m_cannotMoveState))
        {
            canRotate = false;
        }

        return canRotate;
    }
}
