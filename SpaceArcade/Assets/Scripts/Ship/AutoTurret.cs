using UnityEngine;

namespace SpaceArcade.Ship
{
    public class AutoTurret : BaseTurret
    {
        [SerializeField] float attackRange = 15f;
        [SerializeField] string targetTag = "Player";
        
        Transform _target;

        void Start()
        {
            _target = GameObject.FindGameObjectWithTag(targetTag).transform;
        }
        void FixedUpdate()
        {
            if (ShouldShoot())
            {
                AimAtTarget(_target.position, transform.position);
                StartShooting();
            }
            else EndShooting();
        }
        bool ShouldShoot()
        {
            float distance = Vector2.Distance(transform.position, _target.position);
            if (distance > attackRange) return false;
            return true;
        }
    }
}