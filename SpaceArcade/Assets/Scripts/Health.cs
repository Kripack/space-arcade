using System;

namespace SpaceArcade
{
    public class Health
    {
        public event Action OnDeath;
        
        public float CurrentHp;
        public float MaxHp;

        public Health(float maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void TakeDamage(float amount)
        {
            if (amount > CurrentHp) CurrentHp = 0f;
            else CurrentHp -= amount;

            if (CurrentHp <= 0f)
            {
                HealthIsOver();
            }
        }

        public void Heal(float amount)
        {
            if (amount <= 0) return;

            if (amount >= MaxHp) CurrentHp = MaxHp;
            else CurrentHp += amount;

            if (CurrentHp > MaxHp) CurrentHp = MaxHp;
        }

        void HealthIsOver()
        {
            OnDeath?.Invoke();
        }
    }
}
