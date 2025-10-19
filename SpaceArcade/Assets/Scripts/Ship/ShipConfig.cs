using UnityEngine;

namespace SpaceArcade.Ship
{
    [CreateAssetMenu(fileName = "ShipConfig", menuName = "Configs/Ship Config")]
    public class ShipConfig : ScriptableObject
    {
        [Header("Health")]
        public float maxHealth = 100f;
        
        [Header("Shield")]
        public bool hasShield = true;
        public float maxShield = 50f;
        public float shieldRegenRate = 10f;
        public float shieldRegenDelay = 5f;
        
        [Header("Movement")]
        public float movementForce = 2f;
        public float brakeForce = 2f;
        public float maxSpeed = 5f;
        public float rotationForce = 0.4f;
        public float obstacleAvoidDistance = 5f;
        public float stoppingDistance = 3f;
    }
}