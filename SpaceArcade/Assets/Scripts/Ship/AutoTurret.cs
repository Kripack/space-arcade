using UnityEngine;

namespace SpaceArcade.Ship
{
    public class AutoTurret : BaseTurret
    {
        float _attackRange;
        Transform _target;

        public AutoTurret(Transform turret, Transform[] barrels, Projectile projectilePrefab, float fireRate, float rotationSpeed, float attackRange, Transform target) 
            : base(turret, barrels, projectilePrefab, fireRate, rotationSpeed)
        {
            _attackRange = attackRange;
            _target = target;
        }

        public override void Tick()
        {
            if (ShouldShoot())
            {
                AimAtTarget(_target.position, Turret.position);
                StartShooting();
            }
            else EndShooting();
            base.Tick();
        }
        
        bool ShouldShoot()
        {
            float distance = Vector2.Distance(Turret.position, _target.position);
            if (distance > _attackRange) return false;
            return true;
        }
    }
}