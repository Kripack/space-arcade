
using System;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public class Shield
    {
        public event Action<float> OnShieldChanged;
        public event Action<float> OnShieldPercentChanged;
        public event Action OnShieldDepleted;
        public event Action OnShieldRestored;
        
        readonly float _maxShield;
        readonly float _regenRate;
        readonly float _regenDelay;
        
        float _currentShield;
        float _timeSinceLastDamage;
        bool _isRegenerating;
        
        public float CurrentShield => _currentShield;
        public float MaxShield => _maxShield;
        public float ShieldPercent => _maxShield > 0 ? _currentShield / _maxShield : 0f;
        public bool IsActive => _currentShield > 0;
        
        public Shield(float maxShield, float regenRate, float regenDelay = 6f)
        {
            _maxShield = maxShield;
            _regenRate = regenRate;
            _regenDelay = regenDelay;
            _currentShield = maxShield;
        }
        
        public void Tick(float deltaTime)
        {
            if (_currentShield < _maxShield)
            {
                _timeSinceLastDamage += deltaTime;
                
                if (_timeSinceLastDamage >= _regenDelay)
                {
                    if (!_isRegenerating)
                    {
                        _isRegenerating = true;
                    }
                    
                    Regenerate(deltaTime);
                }
            }
        }
        
        public float AbsorbDamage(float damage)
        {
            if (damage <= 0) return 0f;
            
            _timeSinceLastDamage = 0f;
            _isRegenerating = false;
            
            if (_currentShield <= 0)
            {
                return damage;
            }
            
            float damageAbsorbed = Mathf.Min(damage, _currentShield);
            float remainingDamage = damage - damageAbsorbed;
            
            _currentShield -= damageAbsorbed;
            
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldPercentChanged?.Invoke(ShieldPercent);
            
            if (_currentShield <= 0)
            {
                _currentShield = 0;
                OnShieldDepleted?.Invoke();
            }
            
            return remainingDamage;
        }
        
        void Regenerate(float deltaTime)
        {
            bool wasNotFull = _currentShield < _maxShield;
            
            _currentShield = Mathf.Min(_currentShield + _regenRate * deltaTime, _maxShield);
            
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldPercentChanged?.Invoke(ShieldPercent);
            
            if (wasNotFull && _currentShield >= _maxShield)
            {
                _isRegenerating = false;
                OnShieldRestored?.Invoke();
            }
        }
        
        public void Recharge(float amount)
        {
            if (amount <= 0) return;
            
            _currentShield = Mathf.Min(_currentShield + amount, _maxShield);
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldPercentChanged?.Invoke(ShieldPercent);
        }
        
        public void Reset()
        {
            _currentShield = _maxShield;
            _timeSinceLastDamage = 0f;
            _isRegenerating = false;
            OnShieldChanged?.Invoke(_currentShield);
            OnShieldPercentChanged?.Invoke(ShieldPercent);
        }
    }
}
