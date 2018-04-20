using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour 
{
    protected Transform m_Transform;
    protected Rigidbody m_Rigidbody;

	void Start()
	{
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
	}

	void Update()
	{
        if (m_Rigidbody.velocity.magnitude > 0) 
        {
            // Rotate towards direction of movement 
            float angle = Mathf.Atan2(m_Rigidbody.velocity.y, m_Rigidbody.velocity.x) * Mathf.Rad2Deg;
            m_Transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);    
        }
	}

	public void AddForce(float force) 
    {
        if (m_Rigidbody == null)
            m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.AddForce(transform.right * force);
    }
}
