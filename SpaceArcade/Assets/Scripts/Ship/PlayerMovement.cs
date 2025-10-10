using UnityEngine;
using System.Collections;
using SpaceArcade.Input;

namespace SpaceArcade.Ship
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] protected float movementForce = 5f;
        [SerializeField] float strafeForce = 3f;
        [SerializeField] float brakeForce = 2f;
        [SerializeField] protected float maxSpeed = 7f;
        [SerializeField] protected float rotationSpeed = 90f;
        [SerializeField] InputReader inputReader;
        
        Vector2 _inputVector;
        bool _isStrafe;
        Rigidbody2D _rb;
        Coroutine _brakeCoroutine;
        
        void OnEnable()
        {
            inputReader.Move += HandlePlayerInput;
            inputReader.Brake += SmoothStop;
            inputReader.StopBrake += OnStopBrake;
        }
        
        protected void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected void FixedUpdate()
        {
            HandleRotation(_inputVector);
            HandleThrust(_inputVector);
            HandleStrafe();
        }

        void HandlePlayerInput(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }
        
        void HandleRotation(Vector2 direction)
        {
            if (direction.x != 0 && !_isStrafe)
            {
                float rotation = -direction.x * rotationSpeed * Time.fixedDeltaTime;
                _rb.MoveRotation(_rb.rotation + rotation);
            }
        }

        void HandleThrust(Vector2 direction)
        {
            if (direction.y != 0)
            {
                Vector2 force = transform.up * (direction.y * movementForce);
                _rb.AddForce(force, ForceMode2D.Force);
                _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, maxSpeed);
            }
        }
        
        void HandleStrafe()
        {
            if (inputReader.IsShiftPressed)
            {
                _isStrafe = true;
                if (_inputVector.x > 0.1f) StrafeRight();
                else if (_inputVector.x < -0.1f) StrafeLeft();
            }
            else _isStrafe = false;
        }
        
        void SmoothStop()
        {
            if (_brakeCoroutine != null)
            {
                StopCoroutine(_brakeCoroutine);
            }
            _brakeCoroutine = StartCoroutine(BrakeRoutine());
        }
    
        IEnumerator BrakeRoutine()
        {
            while (_rb.linearVelocity.magnitude > 0.1f)
            {
                _rb.AddForce(-_rb.linearVelocity * brakeForce, ForceMode2D.Force);
                yield return new WaitForFixedUpdate();
            }
            _rb.linearVelocity = Vector2.zero;
            _brakeCoroutine = null;
        }
        
        void OnStopBrake()
        {
            if (_brakeCoroutine != null)
            {
                StopCoroutine(_brakeCoroutine);
                _brakeCoroutine = null;
            }
        }
        
        void StrafeLeft()
        {
            _rb.AddForce(-transform.right * strafeForce, ForceMode2D.Impulse);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, maxSpeed/2f);
        }
    
        void StrafeRight()
        {
            _rb.AddForce(transform.right * strafeForce, ForceMode2D.Impulse);
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, maxSpeed/2f);
        }
        
        void OnDisable()
        {
            inputReader.Move -= HandlePlayerInput;
            inputReader.Brake -= SmoothStop;
            inputReader.StopBrake -= OnStopBrake;
        }
    }

}
