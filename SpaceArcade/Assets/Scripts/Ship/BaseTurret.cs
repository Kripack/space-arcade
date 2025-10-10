using UnityEngine;
using System.Collections;

namespace SpaceArcade.Ship
{
    public abstract class BaseTurret : MonoBehaviour
    {
        [Header("Turret Settings")]
        [SerializeField] protected Transform[] barrels;
        [SerializeField] protected Projectile projectilePrefab;
        [SerializeField] protected float fireRate = 1f;
        [SerializeField] protected float rotationSpeed = 5f;
        
        protected bool isFiring;
        protected Coroutine shootingCoroutine;
        
        int _currentBarrelIndex;
        float _nextTimeToFire;
        
        protected float AimAtTarget(Vector2 targetPosition, Vector2 turretPosition)
        {
            Vector2 direction = targetPosition - turretPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            return angle;
        }

        protected IEnumerator ShootRoutine()
        {
            while (isFiring)
            {
                ShootFromBarrel(barrels[_currentBarrelIndex]);
                _currentBarrelIndex = (_currentBarrelIndex + 1) % barrels.Length;
                yield return new WaitForSeconds(fireRate);
            }
        }

        void ShootFromBarrel(Transform barrel)
        {
            _nextTimeToFire = Time.time + fireRate;

            var projectile = Instantiate(projectilePrefab, barrel.position, barrel.rotation);
            projectile.SetOwner(transform.parent.gameObject);
        }

        protected void StartShooting()
        {
            if (shootingCoroutine != null) return;

            if (Time.time >= _nextTimeToFire)
            {
                isFiring = true;
                shootingCoroutine = StartCoroutine(ShootRoutine());
            }
        }
        
        protected void EndShooting()
        {
            isFiring = false;
            if (shootingCoroutine != null) 
                StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }
}