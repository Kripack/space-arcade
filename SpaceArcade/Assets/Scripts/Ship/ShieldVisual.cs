
using UnityEngine;

namespace SpaceArcade.Ship
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ShieldVisual : MonoBehaviour
    {
        [Header("Visual Settings")]
        [SerializeField] SpriteRenderer shieldRenderer;
        [SerializeField] Color fullShieldColor = new Color(0.2f, 0.6f, 1f, 0.4f);
        [SerializeField] Color lowShieldColor = new Color(1f, 0.3f, 0.3f, 0.4f);
        [SerializeField] float hitFlashDuration = 0.1f;
        [SerializeField] float hitFlashAlpha = 0.8f;
        
        [Header("Animation")]
        [SerializeField] float pulseSpeed = 2f;
        [SerializeField] float pulseAmount = 0.1f;
        
        Shield _shield;
        float _hitFlashTimer;
        Vector3 _originalScale;
        
        void Awake()
        {
            if (shieldRenderer == null)
                shieldRenderer = GetComponent<SpriteRenderer>();
                
            _originalScale = transform.localScale;
            shieldRenderer.enabled = false;
        }
        
        public void Initialize(Shield shield)
        {
            _shield = shield;
            
            _shield.OnShieldChanged += OnShieldChanged;
            _shield.OnShieldDepleted += OnShieldDepleted;
            _shield.OnShieldRestored += OnShieldRestored;
            
            UpdateVisual(1f);
        }
        
        void Update()
        {
            if (_shield == null || !_shield.IsActive) return;
            
            if (_hitFlashTimer > 0)
            {
                _hitFlashTimer -= Time.deltaTime;
            }
            
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = _originalScale * pulse;
        }
        
        void OnShieldChanged(float currentShield)
        {
            if (currentShield > 0)
            {
                if (!shieldRenderer.enabled)
                    shieldRenderer.enabled = true;
                    
                UpdateVisual(_shield.ShieldPercent);
                
                if (_shield.ShieldPercent < 1f)
                {
                    _hitFlashTimer = hitFlashDuration;
                }
            }
        }
        
        void UpdateVisual(float percent)
        {
            Color targetColor = Color.Lerp(lowShieldColor, fullShieldColor, percent);
            
            if (_hitFlashTimer > 0)
            {
                float flashT = _hitFlashTimer / hitFlashDuration;
                targetColor.a = Mathf.Lerp(targetColor.a, hitFlashAlpha, flashT);
            }
            
            shieldRenderer.color = targetColor;
        }
        
        void OnShieldDepleted()
        {
            shieldRenderer.enabled = false;
        }
        
        void OnShieldRestored()
        {
            shieldRenderer.enabled = true;
            UpdateVisual(1f);
        }
        
        void OnDestroy()
        {
            if (_shield != null)
            {
                _shield.OnShieldChanged -= OnShieldChanged;
                _shield.OnShieldDepleted -= OnShieldDepleted;
                _shield.OnShieldRestored -= OnShieldRestored;
            }
        }
    }
}
