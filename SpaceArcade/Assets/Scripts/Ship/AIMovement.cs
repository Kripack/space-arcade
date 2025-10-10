using System.Collections;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public class AIMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] protected float movementForce = 2f;
        [SerializeField] float brakeForce = 2f;
        [SerializeField] protected float maxSpeed = 5f;
        [SerializeField] protected float rotationForce = 0.4f;
        
        [Header("AI Settings")]
        [SerializeField] float obstacleAvoidDistance = 5f;
        [SerializeField] float stoppingDistance = 3f;
        [SerializeField] LayerMask obstacleLayer;
        [SerializeField] string targetTag;
        
        Transform _target;
        bool _isAvoiding;
        Rigidbody2D _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        void FixedUpdate()
        {
            Vector2 targetDirection = GetMovementDirection();
            float distanceToPlayer = Vector2.Distance(transform.position, _target.position);
            
            HandleRotation(targetDirection);
            
            if (distanceToPlayer > stoppingDistance)
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
            Vector2 directionToPlayer = (_target.position - transform.position).normalized;
            
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationForce * Time.fixedDeltaTime);
        }
        
        void HandleThrust()
        {
            Vector2 force = transform.up * movementForce;
            _rb.AddForce(force, ForceMode2D.Force);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, maxSpeed);
        }

        bool CheckForObstacles(out RaycastHit2D hit)
        {
            hit = Physics2D.Raycast(transform.position, transform.up, obstacleAvoidDistance, obstacleLayer);
            return hit.collider != null && !hit.collider.CompareTag(targetTag);
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
                _rb.AddForce(-_rb.linearVelocity * brakeForce, ForceMode2D.Force);
            }
            else _rb.linearVelocity = Vector2.zero;
        }
    }
}