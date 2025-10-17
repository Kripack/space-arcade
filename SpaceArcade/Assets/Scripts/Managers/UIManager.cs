using SpaceArcade.Ship;
using SpaceArcade.UI;
using UnityEngine;

namespace SpaceArcade.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
    
        [SerializeField] HealthBarUI healthBarUI;
        [SerializeField] BoostBarUI boostBarUI;
    
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    
        public void InitializeHealthBar(Health health)
        {
            healthBarUI.Initialize(health);
        }

        public void InitializeBoostBar(PlayerMovement playerMovement)
        {
            boostBarUI.Initialize(playerMovement);
        }
    }
}
