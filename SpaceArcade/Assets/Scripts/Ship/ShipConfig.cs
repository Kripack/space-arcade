using UnityEngine;

namespace SpaceArcade.Ship
{
    [CreateAssetMenu(fileName = "ShipConfig", menuName = "Configs/Ship Config")]
    public class ShipConfig : ScriptableObject
    {
        public float maxHealth = 100f;
        public float movementForce = 2f;
        public float brakeForce = 2f;
        public float maxSpeed = 5f;
        public float rotationForce = 0.4f;
        public float obstacleAvoidDistance = 5f;
        public float stoppingDistance = 3f;
    }
}