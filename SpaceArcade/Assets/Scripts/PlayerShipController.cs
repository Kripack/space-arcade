using System.Collections.Generic;
using SpaceArcade.Input;
using SpaceArcade.Managers;
using SpaceArcade.Ship;
using UnityEngine;

namespace SpaceArcade
{
    public class PlayerShipController : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealth = 100f;
        [SerializeField] float maxShield = 50f;
        [SerializeField] float shieldRegenRate = 10f;
        [SerializeField] float shieldRegenDelay = 6f;
        [SerializeField] List<TurretData> manualTurrets;
        [SerializeField] InputReader inputReader;
        [SerializeField] ShieldVisual shieldVisual;
        
        Health _health;
        Shield _shield;
        List<ManualTurret> _manualTurrets = new ();
        
        void Start()
        {
            _health = new Health(maxHealth);
            _health.OnDeath += Death;
            
            _shield = new Shield(maxShield, shieldRegenRate, shieldRegenDelay);
            
            if (shieldVisual != null)
            {
                shieldVisual.Initialize(_shield);
            }
            
            foreach (TurretData turretData in manualTurrets)
            {
                ManualTurret newTurret = new ManualTurret(turretData.turret, turretData.barrels, turretData.projectilePrefab, turretData.fireRate, turretData.rotationSpeed, inputReader);
                newTurret.Init();
                _manualTurrets.Add(newTurret);
            }
            
            UIManager.Instance.InitializeHealthBar(_health);
            UIManager.Instance.InitializeBoostBar(GetComponent<PlayerMovement>());
            UIManager.Instance.InitializeShieldBar(_shield);
        }

        void FixedUpdate()
        {
            _shield?.Tick(Time.fixedDeltaTime);
            
            foreach (ManualTurret turret in _manualTurrets)
            {
                turret.Tick();
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
            foreach (ManualTurret turret in _manualTurrets)
            {
                turret.OnDisable();
            }
            
            Debug.LogWarning("Game Over");
            //TODO: Game Over
        }
    }
}