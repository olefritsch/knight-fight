using UnityEngine;

public class Shootable : MonoBehaviour, IInteractable
{
    [Range(-360, 360)]
    [SerializeField] float m_MinAngle;
    [Range(-360, 360)]
    [SerializeField] float m_MaxAngle;

    [SerializeField] float m_Force;
    [SerializeField] GameObject m_ProjectilePrefab;
    [SerializeField] Transform m_ProjectileSpawn;

    private bool m_IsUsed;
    public bool IsUsed { get { return m_IsUsed; } }

	void Update()
	{
        if (m_IsUsed) 
        {
            Aim(); 
        }
	}

	public void Use() 
    {
        GameObject projectileObj = Instantiate(m_ProjectilePrefab, m_ProjectileSpawn.position, m_ProjectileSpawn.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.AddForce(m_Force);
    }

    public void Interact() 
    {
        m_IsUsed = true;
    }

    public void StopInteracting() 
    {
        m_IsUsed = false;
    }

    void Aim() 
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.z;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, m_MinAngle, m_MaxAngle);

        float rotateSpeed = 0;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, angle, ref rotateSpeed, 0.05f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, smoothAngle));    
    }
}
