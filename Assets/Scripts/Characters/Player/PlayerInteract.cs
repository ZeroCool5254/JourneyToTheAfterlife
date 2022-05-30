using System;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField, Header("Events")] private TogglePlayerInputEvent _playerInputEvent;
        [SerializeField] private PlayerInteractionEvent _interactionEvent;
        private InputManager _inputManager;

        private void OnEnable()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Interact.performed += Interact;
            _playerInputEvent.InputChangedEvent.AddListener(EnableInput);
        }

        private void OnDisable()
        {
            _playerInputEvent.InputChangedEvent.RemoveListener(EnableInput);
        }

        private void EnableInput(bool state)
        {
            if (state) _inputManager.Player.Enable();
            else _inputManager.Player.Disable();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            _interactionEvent.InteractionEvent.Invoke();
        }
    }
}