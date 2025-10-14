using System.Collections.Generic;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public class ShipController : MonoBehaviour, IDamageable
    {
        [SerializeField] List<TurretData> turrets;
        [SerializeField] ShipConfig shipConfig;
        [SerializeField] Rigidbody2D shipRb;
        [SerializeField] LayerMask obstacleLayer;
        
        Health _health;
        AIMovement _aimMovement;
        AutoTurret _autoTurret;

        float _turretTickTimer;
        const float TurretTickInterval = 0.1f;
        
         void Start()
         {
             _health = new Health(shipConfig.maxHealth);
             _health.OnDeath += Death;
             
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

             _turretTickTimer += deltaTime;
             if (_turretTickTimer >= TurretTickInterval)
             {
                 _turretTickTimer = 0f;
                 _autoTurret.Tick();
             }
         }
         
         public void TakeDamage(float amount)
         {
             _health.TakeDamage(amount);
         }
        
         void Death()
         {
             Destroy(gameObject);
         }
         
         void OnDestroy()
         {
             _health.OnDeath -= Death;
         }
    }
}