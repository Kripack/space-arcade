using SpaceArcade.UI;
using UnityEngine;

namespace SpaceArcade.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
    
        [SerializeField] HealthBarUI healthBarUI;
    
        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
    
        public void InitializeHealthBar(Health health)
        {
            healthBarUI.Initialize(health);
        }
    }
}
