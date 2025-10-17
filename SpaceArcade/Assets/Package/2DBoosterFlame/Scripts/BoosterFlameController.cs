using SpaceArcade.Ship;
using UnityEngine;

namespace RockingProjects
{
    public class BoosterFlameController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] float maxBend;
        [SerializeField] float maxAngularVelocityForBending;

        [SerializeField] Smooth smoothBending;

        [SerializeField, Range(0f, 1f)] float minSquash;
        [SerializeField, Range(0f, 6f)] float maxSquash;
        [SerializeField] Smooth smoothSquashing;
        [SerializeField, Tooltip("The velocity where the flame is at maximum")]
        float maxVelocity = 20f;

        [SerializeField, Range(0f, 2f)] float minIntensity;
        [SerializeField, Range(0f, 2f)] float maxIntensity;
        [SerializeField] Smooth smoothIntensity;

        [Header("Boost Scale Settings")]
        [SerializeField] float boostScaleIncrease = 0.3f;
        [SerializeField] float boostScaleSpeed = 8f;

        Velocity _velocity;
        SpriteRenderer _spriteRenderer;
        PlayerMovement _playerMovement;
        
        float _baseScaleY;
        float _targetScaleY;
        float _currentScaleY;
        bool _wasBoostActive;
    
        public void Initialize(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _velocity = new Velocity(gameObject);
            _baseScaleY = transform.localScale.y;
            _currentScaleY = _baseScaleY;
            _targetScaleY = _baseScaleY;
        }

        void FixedUpdate() 
        {
            _velocity.step();
            
            UpdateBoostScale();
            
            float bend = ComputeBend();
            float squash = ComputeSquash();
            float intensity = ComputeIntensity();
            _spriteRenderer.material.SetFloat("_bend", bend);
            _spriteRenderer.material.SetFloat("_squash", squash);
            _spriteRenderer.material.SetFloat("_intensity", intensity);
        }

        void UpdateBoostScale()
        {
            if (_playerMovement == null) return;

            bool isBoostActive = _playerMovement.IsBoostActive;
            
            if (isBoostActive != _wasBoostActive)
            {
                _targetScaleY = isBoostActive ? _baseScaleY + boostScaleIncrease : _baseScaleY;
                _wasBoostActive = isBoostActive;
            }

            if (_currentScaleY != _targetScaleY)
            {
                _currentScaleY = Mathf.MoveTowards(_currentScaleY, _targetScaleY, boostScaleSpeed * Time.fixedDeltaTime);
                
                Vector3 scale = transform.localScale;
                scale.y = _currentScaleY;
                transform.localScale = scale;
            }
        }

        float ComputeIntensity()
        {
            float intensity = Helper.mapValue(0f, this.maxVelocity, Mathf.Abs(_velocity.velocity.magnitude), this.minIntensity, this.maxIntensity);
            return this.smoothIntensity.smooth(intensity);
        }

        float ComputeSquash()
        {
            float squash = Helper.mapValue(-maxVelocity, maxVelocity, _velocity.magnitudeRelativeToTargetDirection, maxSquash, minSquash);
            return this.smoothSquashing.smooth(squash);
        }

        float ComputeBend() {
            float bend = Helper.mapValue(-maxAngularVelocityForBending, maxAngularVelocityForBending, _velocity.angularVelocity, -maxBend, maxBend);
            float angle = Vector2.SignedAngle(_velocity.velocity, transform.up);
            float bend2 = -Helper.mapValue(-1f, 1f, (float)System.Math.Sin((double) Helper.mapValue(-180f, 180f, angle, 0f, 2f * (float)System.Math.PI)), -maxBend, maxBend);
            float normalizedVelocity = Helper.mapValue(0f, maxVelocity, _velocity.velocity.magnitude, 0f, 1f);
            bend2 *= normalizedVelocity;
            bend = bend + bend2;
            bend = Mathf.Clamp(bend, -maxBend, maxBend);
            return this.smoothBending.smooth(bend);
        }


    }
    
}
