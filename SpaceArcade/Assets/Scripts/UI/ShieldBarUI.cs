using SpaceArcade.Ship;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceArcade.UI
{
    public class ShieldBarUI : MonoBehaviour
    {
        [SerializeField] Image fillImage;
        [SerializeField] TextMeshProUGUI shieldText;
        [SerializeField] Color fullShieldColor = Color.cyan;
        [SerializeField] Color lowShieldColor = Color.red;

        Shield _shield;

        public void Initialize(Shield shield)
        {
            _shield = shield;

            _shield.OnShieldChanged += UpdateBar;
            _shield.OnShieldDepleted += OnShieldDepleted;
            _shield.OnShieldRestored += OnShieldRestored;

            UpdateBar(_shield.CurrentShield);
        }

        void UpdateBar(float currentShield)
        {
            fillImage.fillAmount = _shield.ShieldPercent;
            fillImage.color = Color.Lerp(lowShieldColor, fullShieldColor, _shield.ShieldPercent);
            
            shieldText.text = $"{Mathf.CeilToInt(currentShield)}/{Mathf.CeilToInt(_shield.MaxShield)}";
        }

        void OnShieldDepleted()
        {
            // if (shieldIcon != null)
            //     shieldIcon.SetActive(false);
        }

        void OnShieldRestored()
        {
            // if (shieldIcon != null)
            //     shieldIcon.SetActive(true);
        }

        void OnDestroy()
        {
            if (_shield != null)
            {
                _shield.OnShieldChanged -= UpdateBar;
                _shield.OnShieldDepleted -= OnShieldDepleted;
                _shield.OnShieldRestored -= OnShieldRestored;
            }
        }
    }
}