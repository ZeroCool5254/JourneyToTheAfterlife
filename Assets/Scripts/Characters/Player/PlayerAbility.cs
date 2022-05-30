using System;
using System.Collections;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Characters.Player
{
    public class PlayerAbility : MonoBehaviour
    {
        [SerializeField, Header("Ability")] private float _abilityCost;
        [SerializeField] private float _lerpDuration;
        [SerializeField] private Color _activeColor;
        [SerializeField] private float _activeIntensity;
        [SerializeField] private Light2D _light;

        [SerializeField, Header("Audio")] private AudioClip[] _abilityActiveClips;
        [SerializeField] private AudioClip[] _abilityInactiveClips;
        
        
        [SerializeField, Header("Events")] private UpdateManaEvent _manaChangedEvent;
        [SerializeField] private TogglePlayerAbilityEvent _abilityEnabledEvent;
        [SerializeField] private TogglePlayerInputEvent _playerInputEvent;
        [SerializeField] private PlayAudioEvent _playAudioEvent;
        
        private SpriteRenderer _spriteRenderer;
        private Color _inactiveColor;
        private float _inactiveIntensity;
        private bool _isAbilityActive;
        private InputManager _inputManager;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {

            _inactiveColor = _spriteRenderer.color;
            _inactiveIntensity = _light.intensity;
            //delayed UI update event
            _manaChangedEvent.ManaChangedEvent.Invoke(_manaChangedEvent.Mana);
        }

        private void OnEnable()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Ability.performed += PerformAbility;
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

        private void PerformAbility(InputAction.CallbackContext context)
        {
            Debug.Log("PlayerAbility::Should toggle ability");
            if (!_isAbilityActive && _manaChangedEvent.Mana >= _abilityCost)
            {
                int selectedActiveClip = Random.Range(0, _abilityActiveClips.Length);
                _playAudioEvent.PlaySelectedClip(_abilityActiveClips[selectedActiveClip]);
                _isAbilityActive = true;
                _abilityEnabledEvent.EnableAbility();
                _manaChangedEvent.DecreaseMana(_abilityCost);
                //the player is becoming visible to the living world
                StartCoroutine(AbilityCooldownRoutine());
                StartCoroutine(PerformAbilityRoutine(_activeColor, _activeIntensity));
            }
            else if (_isAbilityActive)
            {
                int selectedInactiveClip = Random.Range(0, _abilityInactiveClips.Length);
                _playAudioEvent.PlaySelectedClip(_abilityInactiveClips[selectedInactiveClip]);
                _isAbilityActive = false;
                _abilityEnabledEvent.DisableAbility();
                //the player is becoming invisible to the living world
                StopCoroutine(AbilityCooldownRoutine());
                StartCoroutine(PerformAbilityRoutine(_inactiveColor, _inactiveIntensity));
            }
        }

        private IEnumerator AbilityCooldownRoutine()
        {
            if (_isAbilityActive)
            {
                yield return new WaitForSeconds(5);
                int selectedInactiveClip = Random.Range(0, _abilityInactiveClips.Length);
                _playAudioEvent.PlaySelectedClip(_abilityInactiveClips[selectedInactiveClip]);
                _isAbilityActive = false;
                _abilityEnabledEvent.DisableAbility();
                //the player is becoming invisible to the living world
                StartCoroutine(PerformAbilityRoutine(_inactiveColor, _inactiveIntensity));
            }
        }

        private IEnumerator PerformAbilityRoutine(Color targetColor, float targetIntensity)
        {
            float time = 0;
            float startIntensity = _light.intensity;
            Color startColor = _spriteRenderer.color;
            while (time < _lerpDuration)
            {
                _light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / _lerpDuration);
                _spriteRenderer.color = Color.Lerp(startColor, targetColor, time / _lerpDuration);
                time += Time.deltaTime;
                yield return null;
            }
            _spriteRenderer.color = targetColor;
            _light.intensity = targetIntensity;
        }
    }
}