using System;
using UnityEngine;

namespace SpaceArcade
{
    public class Health : MonoBehaviour, IDamageable
    {
        public event Action OnHealthDecrease;
        public event Action OnHealthIncrease;
        public float CurrentHp { get; private set; }
        [field: SerializeField] public float MaxHp { get; private set; }

        void Start()
        {
            CurrentHp = MaxHp;
        }

        public void TakeDamage(float amount)
        {
            if (amount > CurrentHp) CurrentHp = 0f;
            else CurrentHp -= amount;

            OnHealthDecrease?.Invoke();

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

            OnHealthIncrease?.Invoke();
        }

        void HealthIsOver()
        {
            Destroy(gameObject);
            //GlobalEventBus.Instance.TriggerGameLose();
        }
        
        // private IEnumerator FlashDamage()
        // {
        //     _spriteRenderer.color = Color.red;
        //     yield return new WaitForSeconds(0.1f);
        //     _spriteRenderer.color = Color.white;
        // }
    }
}
