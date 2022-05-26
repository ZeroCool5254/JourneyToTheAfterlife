using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField, Header("Managers")] private PlayerInputManagerSO _playerInputManager;
        [SerializeField] private PlayerInteractionManagerSO _playerInteractionManager;
        private InputManager _inputManager;

        private void OnEnable()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Interact.performed += Interact;
            _playerInputManager.InputChangedEvent.AddListener(EnableInput);
        }

        private void OnDisable()
        {
            _playerInputManager.InputChangedEvent.RemoveListener(EnableInput);
        }

        private void EnableInput(bool state)
        {
            if (state) _inputManager.Player.Enable();
            else _inputManager.Player.Disable();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            _playerInteractionManager.InteractionEvent.Invoke();
        }
    }
}