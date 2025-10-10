/*using SpaceArcade.Input;
using UnityEngine;

namespace SpaceArcade.Ship
{
    public class PlayerMovementDEL : MonoBehaviour
    {
        [SerializeField] InputReader inputReader;
        Vector2 _inputVector;
        
        void OnEnable()
        {
            inputReader.Move += HandlePlayerInput;
            inputReader.Brake += SmoothStop;
            inputReader.StopBrake += OnStopBrake;
        }

        void OnDisable()
        {
            inputReader.Move -= HandlePlayerInput;
            inputReader.Brake -= SmoothStop;
            inputReader.StopBrake -= OnStopBrake;
        }

        void HandlePlayerInput(Vector2 inputVector)
        {
            _inputVector = inputVector;
        }

        protected Vector2 GetMovementDirection()
        {
            return _inputVector;
        }
        
        void HandleStrafe()
        {
            if (inputReader.IsShiftPressed)
            {
                isStrafe = true;
                if (_inputVector.x > 0.1f) StrafeRight();
                else if (_inputVector.x < -0.1f) StrafeLeft();
            }
            else isStrafe = false;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            HandleStrafe();
        }
    }
}*/