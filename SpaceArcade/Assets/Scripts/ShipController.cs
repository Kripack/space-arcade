using System.Collections.Generic;
using SpaceArcade.Ship;
using UnityEngine;

namespace SpaceArcade
{
    public class ShipController : MonoBehaviour, IDamageable
    {
        [SerializeField] List<TurretData> turrets;
        [SerializeField] ShipConfig shipConfig;
        [SerializeField] Rigidbody2D shipRb;
        [SerializeField] LayerMask obstacleLayer;
        [SerializeField] ShieldVisual shieldVisual;
        
        Health _health;
        Shield _shield;
        AIMovement _aimMovement;
        AutoTurret _autoTurret;

        float _turretTickTimer;
        const float TurretTickInterval = 0.1f;
        
         void Start()
         {
             _health = new Health(shipConfig.maxHealth);
             _health.OnDeath += Death;
             
             if (shipConfig.hasShield)
             {
                 _shield = new Shield(
                     shipConfig.maxShield, 
                     shipConfig.shieldRegenRate, 
                     shipConfig.shieldRegenDelay
                 );
                 
                 if (shieldVisual != null)
                 {
                     shieldVisual.Initialize(_shield);
                 }
             }
             
             Transform target = GameObject.FindGameObjectWithTag("Player").transform;
             _aimMovement = new AIMovement(target, transform, shipRb, shipConfig, obstacleLayer);
             
             TurretData turretData = turrets[0];
             _autoTurret = new AutoTurret(
                 turretData.turret,
                 turretData.barrels,
                 turretData.projectilePrefab,
                 turretData.fireRate,
                 turretData.rotationSpeed,
                 turretData.attackRange,
                 target
             );
         }

         void FixedUpdate()
         {
             float deltaTime = Time.fixedDeltaTime;

             _aimMovement.Tick();
             
             _shield?.Tick(deltaTime);

             _turretTickTimer += deltaTime;
             if (_turretTickTimer >= TurretTickInterval)
             {
                 _turretTickTimer = 0f;
                 _autoTurret.Tick();
             }
         }
         
         public void TakeDamage(float amount)
         {
             float remainingDamage = amount;
             
             if (_shield != null && _shield.IsActive)
             {
                 remainingDamage = _shield.AbsorbDamage(amount);
             }
             
             if (remainingDamage > 0)
             {
                 _health.TakeDamage(remainingDamage);
             }
         }
        
         void Death()
         {
             Destroy(gameObject);
             
             // TODO: add effects
         }
         
         void OnDestroy()
         {
             _health.OnDeath -= Death;
         }
    }
}