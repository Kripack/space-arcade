using UnityEngine;

namespace SpaceArcade.Ship
{
    public class AIMovement
    {
        float _movementForce;
        float _brakeForce;
        float _maxSpeed;
        float _rotationForce;
        float _obstacleAvoidDistance;
        float _stoppingDistance;
        LayerMask _obstacleLayer;
        string _targetTag;
        
        Transform _target;
        Transform _transform;
        bool _isAvoiding;
        Rigidbody2D _rb;

        public AIMovement(Transform target, Transform transform, Rigidbody2D rb, ShipConfig config, LayerMask obstacleLayer)
        {
            _rb = rb;
            _target = target;
            _transform = transform;
            _obstacleLayer = obstacleLayer;
            _targetTag = target.tag;
            _movementForce = config.movementForce;
            _brakeForce = config.brakeForce;
            _maxSpeed = config.maxSpeed;
            _rotationForce = config.rotationForce;
            _obstacleAvoidDistance = config.obstacleAvoidDistance;
            _stoppingDistance = config.stoppingDistance;
        }
        
        public void Tick()
        {
            Vector2 targetDirection = GetMovementDirection();
            float distanceToPlayer = Vector2.Distance(_transform.position, _target.position);
            
            HandleRotation(targetDirection);
            
            if (distanceToPlayer > _stoppingDistance)
            {
                HandleThrust();
            }
            else
            {
                SmoothStop();
            }
        }
        
        Vector2 GetMovementDirection()
        {
            Vector2 directionToPlayer = (_target.position - _transform.position).normalized;
            
            if (CheckForObstacles(out RaycastHit2D hit))
            {
                _isAvoiding = true;
                return CalculateAvoidanceDirection(hit);
            }
            _isAvoiding = false;
            return directionToPlayer;
        }
        
        void HandleRotation(Vector2 direction)
        {
            if (direction == Vector2.zero) return;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _rotationForce * Time.fixedDeltaTime);
        }
        
        void HandleThrust()
        {
            Vector2 force = _transform.up * _movementForce;
            _rb.AddForce(force, ForceMode2D.Force);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxSpeed);
        }

        bool CheckForObstacles(out RaycastHit2D hit)
        {
            hit = Physics2D.Raycast(_transform.position, _transform.up, _obstacleAvoidDistance, _obstacleLayer);
            return hit.collider != null && !hit.collider.CompareTag(_targetTag);
        }

        Vector2 CalculateAvoidanceDirection(RaycastHit2D hit)
        {
            Vector2 hitNormal = hit.normal;
            return new Vector2(-hitNormal.y, hitNormal.x).normalized;
        }

        void SmoothStop()
        {
            if (_rb.linearVelocity.magnitude > 0.1f)
            {
                _rb.AddForce(-_rb.linearVelocity * _brakeForce, ForceMode2D.Force);
            }
            else _rb.linearVelocity = Vector2.zero;
        }
    }
}