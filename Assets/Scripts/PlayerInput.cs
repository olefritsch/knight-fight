using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerInput : MonoBehaviour
{
    private MovementController m_Movement;

    private float m_Horizontal;
    private float m_Vertical;
    private bool m_IsJumping;

    private bool m_IsAttacking;

	void Start ()
    {
        m_Movement = GetComponent<MovementController>();
	}
	
	void Update ()
    {
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");

        if (!m_IsJumping)
            m_IsJumping = Input.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        m_Movement.Move(m_Horizontal, m_Vertical, m_IsJumping);
        m_IsJumping = false;
    }
}
