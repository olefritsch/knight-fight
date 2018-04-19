using UnityEngine;

public class CombatController : MonoBehaviour
{
    // Serializeable Member Variables
    [SerializeField] float m_ThrowForce;
	[SerializeField] GameObject m_Weapon;
	[SerializeField] Transform m_WeaponHand;

	void OnCollisionEnter(Collision collision)
	{
		if (m_Weapon == null && collision.gameObject.tag == Tags.WeaponTag)
		{
			m_Weapon = collision.gameObject;
			m_Weapon.transform.rotation = Quaternion.identity;
			m_Weapon.transform.position = m_WeaponHand.position;
			m_Weapon.transform.parent = m_WeaponHand;

			Rigidbody rb = m_Weapon.GetComponent<Rigidbody>();
			rb.isKinematic = true;
		}
	}

    public void Attack() 
    {
        // TODO: Implement Animations
    }

    public void ThrowWeapon() 
    {
        if (m_Weapon != null)
        {
            Vector3 objectPos = Camera.main.WorldToScreenPoint(m_Weapon.transform.position);
            Vector3 mousePos = Input.mousePosition;
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            mousePos.z = transform.position.z;

            Rigidbody rb = m_Weapon.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(mousePos.normalized * m_ThrowForce);
            rb.AddTorque(-transform.forward * m_ThrowForce);

            m_Weapon.transform.parent = null;
            m_Weapon = null;
        }
    }
}
