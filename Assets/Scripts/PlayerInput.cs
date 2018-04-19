﻿using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(CombatController))]
public class PlayerInput : MonoBehaviour
{
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
        m_Horizontal = Input.GetAxis("Horizontal");
        m_Vertical = Input.GetAxis("Vertical");

        if (!m_IsJumping)
            m_IsJumping = Input.GetButtonDown("Jump");

        if (Input.GetButtonDown("Fire1"))
            m_Combat.ThrowWeapon();

        if (Input.GetButtonDown("Fire2"))
            m_Combat.ThrowWeapon();
    }

    void FixedUpdate()
    {
        m_Movement.Move(m_Horizontal, m_Vertical, m_IsJumping);
        m_IsJumping = false;
    }
}
