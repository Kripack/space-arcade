using UnityEngine;
using System.Collections;
using RockingProjects;
using SpaceArcade.Input;

namespace SpaceArcade.Ship
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] BoosterFlameController[] boosterFlames;
        
        [Header("Movement Settings")] 
        [SerializeField] protected float movementForce = 5f;
        [SerializeField] float brakeForce = 2f;
        [SerializeField] protected float maxSpeed = 7f;
        [SerializeField] protected float rotationSpeed = 90f;
        [SerializeField] InputReader inputReader;

        [Header("Boost Settings")] [SerializeField]
        float boostMultiplier = 2f;

        [SerializeField] float maxBoostEnergy = 100f;
        [SerializeField] float boostDrainRate = 25f;
        [SerializeField] float boostRecoveryDelay = 2.5f;
        [SerializeField] float boostRecoveryRate = 20f;

        Vector2 _inputVector;
        Rigidbody2D _rb;
        Coroutine _brakeCoroutine;

        float _currentBoostEnergy;
        float _boostRecoveryTimer;
        
        public bool IsBoostActive { get; private set; }
        public float CurrentBoostEnergy => _currentBoostEnergy;
        public float MaxBoostEnergy => maxBoostEnergy;
        public System.Action OnBoostChanged;

        void OnEnable()
        {
            inputReader.Move += HandlePlayerInput;
            inputReader.Brake += SmoothStop;
            inputReader.StopBrake += OnStopBrake;
        }

        protected void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentBoostEnergy = maxBoostEnergy;
            
            foreach (BoosterFlameController flame in boosterFlames)
            {
                flame.Initialize(this);
            }
        }

        void Update()
        {
            HandleBoostRecovery();
        }

        protected void FixedUpdate()
        {
            HandleRotation(_inputVector);
            HandleThrust(_inputVector);
            HandleBoost();
        }

        void HandlePlayerInput(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        void HandleRotation(Vector2 direction)
        {
            if (direction.x != 0)
            {
                float rotation = -direction.x * rotationSpeed * Time.fixedDeltaTime;
                _rb.MoveRotation(_rb.rotation + rotation);
            }
        }

        void HandleThrust(Vector2 direction)
        {
            if (direction.y != 0)
            {
                float currentForce = IsBoostActive ? movementForce * boostMultiplier : movementForce;
                Vector2 force = transform.up * (direction.y * currentForce);
                _rb.AddForce(force, ForceMode2D.Force);

                float speedLimit = IsBoostActive ? maxSpeed * boostMultiplier : maxSpeed;
                _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, speedLimit);
            }
        }

        void HandleBoost()
        {
            if (inputReader.IsShiftPressed && _currentBoostEnergy > 0 && _inputVector.y > 0)
            {
                IsBoostActive = true;
                _currentBoostEnergy -= boostDrainRate * Time.fixedDeltaTime;
                _currentBoostEnergy = Mathf.Max(_currentBoostEnergy, 0);
                _boostRecoveryTimer = boostRecoveryDelay;

                OnBoostChanged?.Invoke();
            }
            else
            {
                IsBoostActive = false;
            }
        }

        void HandleBoostRecovery()
        {
            if (_currentBoostEnergy < maxBoostEnergy)
            {
                if (_boostRecoveryTimer > 0)
                {
                    _boostRecoveryTimer -= Time.deltaTime;
                }
                else
                {
                    _currentBoostEnergy += boostRecoveryRate * Time.deltaTime;
                    _currentBoostEnergy = Mathf.Min(_currentBoostEnergy, maxBoostEnergy);
                    OnBoostChanged?.Invoke();
                }
            }
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

        void OnDisable()
        {
            inputReader.Move -= HandlePlayerInput;
            inputReader.Brake -= SmoothStop;
            inputReader.StopBrake -= OnStopBrake;
        }
    }
}