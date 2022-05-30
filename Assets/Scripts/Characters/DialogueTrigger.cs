using System;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;

namespace Characters
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _visualCue;
        [SerializeField] private TextAsset _startDialogueToPlay;
        [SerializeField] private TextAsset _completedDialogueToPlay;

        [SerializeField, Header("Events")] private PlayerInteractionEvent _interactionEvent;
        [SerializeField] private TogglePlayerAbilityEvent _abilityEnabledEvent;
        [SerializeField] private StartDialogueEvent _dialogueStartEvent;
        [SerializeField] private EndDialogueEvent _dialogueEvent;

        [Tooltip("If checked then the player has to be tangible or have the ability active to interact with this object")]
        [SerializeField, Header("Behaviours")] private bool _requireAbilityEnabled;

        private bool _playerInRange;
        private bool _canInteract;
        private bool _isDialogueActive;
        private bool _isDialogueCompleted;

        private void Awake()
        {
            _visualCue.enabled = false;
        }

        private void OnEnable()
        {
            _interactionEvent.InteractionEvent.AddListener(StartDialogue);
            _dialogueEvent.CompleteDialogueEvent.AddListener(EndDialogue);
        }

        private void OnDisable()
        {
            _interactionEvent.InteractionEvent.RemoveListener(StartDialogue);
            _dialogueEvent.CompleteDialogueEvent.RemoveListener(EndDialogue);
        }

        private void Update()
        {
            if (_playerInRange && !_requireAbilityEnabled)
            {
                _visualCue.enabled = true;
                _canInteract = true;
            }
            else if (_playerInRange && _requireAbilityEnabled && _abilityEnabledEvent.AbilityEnabled)
            {
                _visualCue.enabled = true;
                _canInteract = true;
            }
            else
            {
                _visualCue.enabled = false;
                _canInteract = false;
            }
        }

        private void StartDialogue()
        {
            if (_playerInRange && _canInteract)
            {
                if (!_isDialogueCompleted)
                {
                    _dialogueStartEvent.ActivateDialogueEvent.Invoke(_startDialogueToPlay);
                    _isDialogueActive = true;
                }
                else if (_isDialogueCompleted)
                {
                    _dialogueStartEvent.ActivateDialogueEvent.Invoke(_completedDialogueToPlay);
                    _isDialogueActive = true;
                }
            }
        }

        private void EndDialogue()
        {
            if (_isDialogueActive)
            {
                _isDialogueCompleted = true;
                _isDialogueActive = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
            }
        }
    }
}