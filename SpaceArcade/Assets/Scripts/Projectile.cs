using SpaceArcade.Managers;
using SpaceArcade.ObjectPool;
using UnityEngine;

namespace SpaceArcade
{
    public class Projectile : PoolableObject
    {
        [Header("Settings")] 
        [SerializeField] float damage = 5f;
        [SerializeField] float speed = 10f;

        [Header("Effects")] 
        [SerializeField] GameObject muzzleFlash;
        [SerializeField] GameObject explosionEffect;
        [SerializeField] AudioClip impactSound;
        [SerializeField] float impactSoundVolume = 0.5f;
        [SerializeField] AudioClip fireSound;
        [SerializeField] float fireSoundVolume = 1f;

        [SerializeField] Rigidbody2D rb;
        [SerializeField] TrailRenderer trail;

        public override void OnSpawn()
        {
            base.OnSpawn();
            
            rb.linearVelocity = transform.up * speed;
    
            VisualFXManager.Instance.SpawnEffect(muzzleFlash, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfx(fireSound, fireSoundVolume);
        }

        public override void OnReturn()
        {
            base.OnReturn();
    
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (trail != null)
            {
                trail.Clear();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }
            
            VisualFXManager.Instance.SpawnEffect(explosionEffect, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySfx(impactSound, impactSoundVolume);
            
            ReturnToPool();
        }
    }
}