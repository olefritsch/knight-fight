using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class CombatController : MonoBehaviour
{
    // Serializeable Member Variables
    [SerializeField] float m_ThrowForce;
    [SerializeField] GameObject m_Weapon;
    [SerializeField] Transform m_WeaponHand;

    private MovementController m_Movement;
    private IInteractable m_Interacting;

    void Start()
    {
        m_Movement = GetComponent<MovementController>();
    }

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
        if (m_Interacting != null)
        {
            m_Interacting.Use();
        }

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

    public void Interact()
    {
        if (m_Interacting != null) 
        {
            m_Interacting.StopInteracting();
            m_Interacting = null;

            m_Movement.enabled = true;
        }
        else 
        {
            int interactableLayer = 1 << LayerMask.NameToLayer(Layers.Interactable);
            RaycastHit hitInfo;

            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.BoxCast(transform.position + Vector3.up, new Vector3(0.4f, 0.8f, 0.5f), transform.right, out hitInfo, Quaternion.identity, 0.4f, interactableLayer))
            {
                IInteractable interactable = hitInfo.transform.root.GetComponent<IInteractable>();
                if (!interactable.IsUsed)
                {
                    m_Interacting = interactable;
                    m_Interacting.Interact();

                    m_Movement.enabled = false;
                }
            }
        }
    }
}
