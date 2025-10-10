using System;
using SpaceArcade.Managers;
using UnityEngine;

namespace SpaceArcade
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float damage = 5f;
        [SerializeField] float speed = 10f;
        [SerializeField] float lifetime = 3f;

        [Header("Visuals")] 
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] GameObject explosionEffect;
        
        Rigidbody2D _rb;
        GameObject _ownerObject;
        public void SetOwner(GameObject obj) => _ownerObject = obj;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            VisualFXManager.Instance.SpawnEffect(muzzleFlash, transform.position, Quaternion.identity, _ownerObject);

            _rb.linearVelocity = transform.up * speed;
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
            VisualFXManager.Instance.SpawnEffect(explosionEffect, transform.position, Quaternion.identity, other.gameObject);
            Destroy(gameObject);
        }
    }
}