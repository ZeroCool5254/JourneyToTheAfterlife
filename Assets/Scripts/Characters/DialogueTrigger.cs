using System;
using ScriptableObjects;
using UnityEngine;

namespace Characters
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _visualCue;
        [SerializeField] private TextAsset _inkJson;

        [SerializeField, Header("Managers")] private PlayerInteractionManagerSO _playerInteractionManager;
        [SerializeField] private DialogueManagerSO _dialogueManager;

        private bool _playerInRange;

        private void Awake()
        {
            _visualCue.enabled = false;
        }

        private void OnEnable()
        {
            _playerInteractionManager.InteractionEvent.AddListener(Interact);
        }

        private void OnDisable()
        {
            _playerInteractionManager.InteractionEvent.RemoveListener(Interact);
        }

        private void Interact()
        {
            if (_playerInRange)
            {
                //call dialogue
                _dialogueManager.ActivateDialogueEvent.Invoke(_inkJson);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                _visualCue.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
                _visualCue.enabled = false;
            }
        }
    }
}