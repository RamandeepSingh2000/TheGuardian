using Player;
using System;
using UnityEngine;

public class DeactivatePortalScript : MonoBehaviour
{
    [SerializeField] private float collideRadious = 0.3f;
    [SerializeField] private int spawnerHP = 3;
    public delegate void PortalStateChange();
    public event PortalStateChange OnPortalDeactivated;
    public event PortalStateChange OnPortalActivated;
    public event PortalStateChange OnPortalAttacked;


    private SphereCollider portalCollider;
    void Start()
    {
        portalCollider = gameObject.AddComponent<SphereCollider>();
        portalCollider.isTrigger = true;
        portalCollider.radius = collideRadious;
    }

    // Function to deactivate portal
    public void DeactivatePortal()
    {
        OnPortalDeactivated?.Invoke();
        gameObject.SetActive(false);
    }

    // Seemed reasonable to make an ActivatePortal function
    public void ActivatePortal()
    {
        gameObject.SetActive(true);
        OnPortalActivated?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.gameObject.GetComponent<PoolableProjectile>();
        //Debug.Log("collision");
        if (projectile != null)
        {
            spawnerHP -= 1;
            if (spawnerHP < 0) DeactivatePortal();
            OnPortalAttacked?.Invoke();
        }
    }

}
