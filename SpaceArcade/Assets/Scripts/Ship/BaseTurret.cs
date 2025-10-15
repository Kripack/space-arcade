using SpaceArcade.ObjectPool;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public abstract class BaseTurret
    {
        protected Transform[] Barrels;
        protected Transform Turret;
        protected Projectile ProjectilePrefab;

        protected float FireRate;
        protected float RotationSpeed;
        protected bool IsFiring;

        int _currentBarrelIndex;
        float _nextFireTime;

        public BaseTurret(Transform turret, Transform[] barrels, Projectile projectilePrefab, float fireRate, float rotationSpeed)
        {
            Turret = turret;
            Barrels = barrels;
            ProjectilePrefab = projectilePrefab;
            FireRate = Mathf.Max(0.001f, fireRate);
            RotationSpeed = rotationSpeed;
        }


        public virtual void Tick()
        {
            if (!IsFiring || Time.time < _nextFireTime)
                return;

            ShootFromBarrel(Barrels[_currentBarrelIndex]);
            _currentBarrelIndex = (_currentBarrelIndex + 1) % Barrels.Length;
            _nextFireTime = Time.time + FireRate;
        }

        protected void AimAtTarget(Vector2 targetPosition, Vector2 turretPosition)
        {
            Vector2 direction = targetPosition - turretPosition;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            Turret.rotation = Quaternion.Slerp(Turret.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        public void StartShooting()
        {
            IsFiring = true;
        }

        public void EndShooting()
        {
            IsFiring = false;
        }

        protected virtual void ShootFromBarrel(Transform barrel)
        {
            Projectile projectile = PoolManager.Instance.Spawn<Projectile>(
                ProjectilePrefab.gameObject,
                barrel.position,
                barrel.rotation
            );
        }
    }
}
