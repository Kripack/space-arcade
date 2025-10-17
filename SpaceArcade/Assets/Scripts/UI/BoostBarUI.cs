using UnityEngine;
using UnityEngine.UI;

namespace SpaceArcade.UI
{
    public class BoostBarUI : MonoBehaviour
    {
        [SerializeField] Image boostBarFill;

        Ship.PlayerMovement _playerMovement;

        public void Initialize(Ship.PlayerMovement playerMovement)
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnBoostChanged -= UpdateBoostDisplay;
            }

            _playerMovement = playerMovement;
            _playerMovement.OnBoostChanged += UpdateBoostDisplay;
            
            UpdateBoostDisplay();
        }

        void UpdateBoostDisplay()
        {
            float boostPercentage = _playerMovement.CurrentBoostEnergy / _playerMovement.MaxBoostEnergy;
            
            if (boostBarFill != null)
            {
                boostBarFill.fillAmount = boostPercentage;
            }
        }

        void OnDestroy()
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnBoostChanged -= UpdateBoostDisplay;
            }
        }
    }
}
