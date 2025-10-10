using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceArcade.Input
{
    [CreateAssetMenu(menuName = "ScriptableObjects/InputReader")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions
    {
        public bool IsShiftPressed { get; private set; }

        #region Events
    
        public event Action<InputAction.CallbackContext> Attack;
        public event Action Brake;
        public event Action StopBrake;
        public event Action<Vector2> Move;
        public event Action<Vector2> PointerDeltaUpdate;

        #endregion
    
        InputSystem_Actions _controls;
    
        void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }
    
        public void OnDisable()
        {
            _controls.Player.Disable();
        }
    
        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }
    
        public void OnBrake(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) Brake?.Invoke();
            if (context.phase == InputActionPhase.Canceled) StopBrake?.Invoke();
        }
    
        public void OnAttack(InputAction.CallbackContext context)
        {
            Attack?.Invoke(context);
        }
    
        public void OnShift(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                IsShiftPressed = true;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                IsShiftPressed = false;
            }
        }
    
        public void OnLook(InputAction.CallbackContext context)
        {
            PointerDeltaUpdate?.Invoke(context.ReadValue<Vector2>());
        }
    
        public void OnInteract(InputAction.CallbackContext context)
        {
        
        }

        public void OnJump(InputAction.CallbackContext context)
        {
        
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        
        }
        
        public void OnNavigate(InputAction.CallbackContext context)
        {
        
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        
        }
    }
}
