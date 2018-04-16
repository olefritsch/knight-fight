using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    // Serializeable Member Variables
    [SerializeField] float m_MovementSpeed;
    [SerializeField] float m_ClimbingSpeed;
    [SerializeField] float m_JumpPower;
    [SerializeField] float m_GravityMultiplier;
    [SerializeField] float m_GroundCheckDistance;

    // Component References
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;

    // Private Member Variables
    private bool m_IsGrounded;
    private bool m_IsClimbing;
    private float m_OriginalGroundCheckDistance;
    private Vector3 m_GroundNormal;

    void Start ()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();

        m_OriginalGroundCheckDistance = m_GroundCheckDistance;
	}

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.tag == Tags.ClimbableTag)
        {
            m_IsClimbing = true;
            m_Rigidbody.useGravity = false;
        }    
    }

    void OnCollisionExit(Collision collision)
    {
		if (collision.gameObject.tag == Tags.ClimbableTag)
        {
            m_IsClimbing = false;
            m_Rigidbody.useGravity = true;
        }
    }

    public void Move(float hMovement, float vMovement, bool jump)
    {
        CheckGroundStatus();

        if (m_IsGrounded)
        {
            HandleGroundedMovement(hMovement, vMovement, jump);
        }
        else if (m_IsClimbing)
        {
            HandleClimbing(hMovement, vMovement);
        }
        else
        {
            HandleAirborneMovement(hMovement);
        }

        // TODO: Implement Animations
    }

    void HandleGroundedMovement(float hMovement, float vMovement, bool jump)
    {
        m_Rigidbody.velocity = new Vector3(hMovement * m_MovementSpeed, 0f);

        if (jump)
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower);
            m_IsGrounded = false;
            m_IsClimbing = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void HandleClimbing(float hMovement, float vMovement)
    {
        m_Rigidbody.velocity = new Vector3(hMovement * m_MovementSpeed * 0.5f, vMovement * m_ClimbingSpeed, 0f);
    }

    void HandleAirborneMovement(float movement)
    {
        m_Rigidbody.velocity = new Vector3(movement * m_MovementSpeed, m_Rigidbody.velocity.y, 0f);

        // Apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y > 0 ? 0.01f : m_OriginalGroundCheckDistance;
    }

    void CheckGroundStatus()
    {
        #if UNITY_EDITOR
        // Helper to visualise the ground check ray in the scene view
        Debug.DrawLine(m_Transform.position + (Vector3.up * 0.1f), m_Transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
        #endif

        RaycastHit hitInfo;
        
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(m_Transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
    
}
