using System;
using SpaceArcade.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceArcade.Ship
{
    public class ManualTurret : BaseTurret
    {
        [SerializeField] InputReader inputReader;
        Camera _mainCamera;
        void OnEnable()
        {
            inputReader.Attack += HandleAttackInput;
        }

        void OnDisable()
        {
            inputReader.Attack -= HandleAttackInput;
        }

        void Start()
        {
            _mainCamera = Camera.main;
        }

        void HandleAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartShooting();
            }
            else if (context.canceled)
            {
                EndShooting();
            }
        }

        void FixedUpdate()
        {
            var turretScreenPos = _mainCamera.WorldToScreenPoint(transform.position);
            AimAtTarget(CustomPointer.pointerPosition, turretScreenPos);
        }
    }
}