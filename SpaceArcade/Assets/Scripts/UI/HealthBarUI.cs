using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SpaceArcade.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] Image healthBarFill;
        [SerializeField] TextMeshProUGUI healthText;

        Health _health;

        public void Initialize(Health health)
        {
            if (_health != null)
            {
                _health.OnChange -= UpdateHealthDisplay;
            }

            _health = health;
            _health.OnChange += UpdateHealthDisplay;
            
            UpdateHealthDisplay();
        }

        void UpdateHealthDisplay()
        {
            float healthPercentage = _health.CurrentHp / _health.MaxHp;
            
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = healthPercentage;
            }

            healthText.text = $"{_health.CurrentHp:F0} / {_health.MaxHp:F0}";
        }

        void OnDestroy()
        {
            if (_health != null)
            {
                _health.OnChange -= UpdateHealthDisplay;
            }
        }
    }
}
