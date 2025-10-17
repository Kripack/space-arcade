using System.Collections.Generic;
using SpaceArcade.Input;
using SpaceArcade.Managers;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public class PlayerShipController : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealth = 100f;
        [SerializeField] List<TurretData> manualTurrets;
        [SerializeField] InputReader inputReader;
        
        Health _health;
        List<ManualTurret> _manualTurrets = new List<ManualTurret>();
        
        void Start()
        {
            _health = new Health(maxHealth);
            _health.OnDeath += Death;
            UIManager.Instance.InitializeHealthBar(_health);
            UIManager.Instance.InitializeBoostBar(GetComponent<PlayerMovement>());
            
            foreach (TurretData turretData in manualTurrets)
            {
                ManualTurret newTurret = new ManualTurret(turretData.turret, turretData.barrels, turretData.projectilePrefab, turretData.fireRate, turretData.rotationSpeed, inputReader);
                newTurret.Init();
                _manualTurrets.Add(newTurret);
            }
        }

        void FixedUpdate()
        {
            foreach (ManualTurret turret in _manualTurrets)
            {
                turret.Tick();
            }
        }

        public void TakeDamage(float amount)
        {
            _health.TakeDamage(amount);
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