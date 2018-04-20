using UnityEngine;

public class Arrow : Projectile 
{
	void OnCollisionEnter(Collision collision)
	{
        string collisionTag = collision.transform.root.tag;
        if (collisionTag == Tags.GroundTag || collisionTag == Tags.ClimbableTag || collisionTag == Tags.PlayerTag) 
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.isKinematic = true;
            this.enabled = false;
        }
	}
}
