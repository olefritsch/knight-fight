using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(CombatController))]
public class PlayerInput : MonoBehaviour
{
    // Collection of Inputs to simplify string references
    public struct InputCommands
    {
        public const string HorizontalMovement = "Horizontal";
        public const string VerticalMovement = "Vertical";
        public const string Jump = "Jump";
        public const string Attack = "Attack";
        public const string Defend = "Defend";
        public const string Throw = "Throw";
        public const string Interact = "Interact";
    }

    private MovementController m_Movement;
    private CombatController m_Combat;

    private float m_Horizontal;
    private float m_Vertical;
    private bool m_IsJumping;

    private bool m_IsAttacking;

	void Start ()
    {
        m_Movement = GetComponent<MovementController>();
        m_Combat = GetComponent<CombatController>();
	}
	
	void Update ()
    {
        m_Horizontal = Input.GetAxis(InputCommands.HorizontalMovement);
        m_Vertical = Input.GetAxis(InputCommands.VerticalMovement);

        if (!m_IsJumping)
            m_IsJumping = Input.GetButtonDown(InputCommands.Jump);

        if (Input.GetButtonDown(InputCommands.Attack))
            m_Combat.Attack();

        if (Input.GetButtonDown(InputCommands.Throw))
            m_Combat.ThrowWeapon();

        if (Input.GetButtonDown(InputCommands.Interact))
            m_Combat.Interact();
    }

    void FixedUpdate()
    {
        m_Movement.Move(m_Horizontal, m_Vertical, m_IsJumping);
        m_IsJumping = false;
    }
}

