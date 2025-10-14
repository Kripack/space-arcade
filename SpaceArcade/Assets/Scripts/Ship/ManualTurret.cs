using SpaceArcade.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceArcade.Ship
{
    public class ManualTurret : BaseTurret
    {
        InputReader _inputReader;
        Camera _mainCamera;

        public ManualTurret(Transform turret, Transform[] barrels, Projectile projectilePrefab, float fireRate, float rotationSpeed, InputReader inputReader)
            : base(turret, barrels, projectilePrefab, fireRate, rotationSpeed)
        {
            _inputReader = inputReader;
        }

        public void Init()
        {
            _inputReader.Attack += HandleAttackInput;
            _mainCamera = Camera.main;
        }

        public void OnDisable()
        {
            _inputReader.Attack -= HandleAttackInput;
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

        public override void Tick()
        {
            var turretScreenPos = _mainCamera.WorldToScreenPoint(Turret.position);
            AimAtTarget(CustomPointer.pointerPosition, turretScreenPos);
            base.Tick();
        }
    }
}