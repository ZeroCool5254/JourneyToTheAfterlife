using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerAbility : MonoBehaviour
    {
        [SerializeField, Header("Ability")] private float _abilityCost;
        [SerializeField] private float _lerpDuration;
        [SerializeField] private Color _activeColor;
        [SerializeField] private float _activeIntensity;
        [SerializeField] private Light2D _light;
        
        [SerializeField, Header("Managers")] private ManaManagerSO _manaManager;
        
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
            _inputManager = new InputManager();
            _inputManager.Player.Ability.performed += PerformAbility;
            _inputManager.Player.Enable();
            _inactiveColor = _spriteRenderer.color;
            _inactiveIntensity = _light.intensity;
            //delayed UI update event
            _manaManager.ManaChangedEvent.Invoke(_manaManager.Mana);
        }

        public void PerformAbility(InputAction.CallbackContext context)
        {
            Debug.Log("PlayerAbility::Should toggle ability");
            if (!_isAbilityActive && _manaManager.Mana >= _abilityCost)
            {
                _isAbilityActive = true;
                _manaManager.DecreaseMana(_abilityCost);
                //the player is becoming visible to the living world
                StartCoroutine(PerformAbilityRoutine(_activeColor, _activeIntensity));
            }
            else if (_isAbilityActive)
            {
                _isAbilityActive = false;
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