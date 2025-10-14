using UnityEngine;

namespace SpaceArcade.Ship
{
    [System.Serializable]
    public class TurretData
    {
        public Transform turret;
        public Transform[] barrels;
        public Projectile projectilePrefab;
        public float fireRate;
        public float rotationSpeed;
        public float attackRange;
    }
}