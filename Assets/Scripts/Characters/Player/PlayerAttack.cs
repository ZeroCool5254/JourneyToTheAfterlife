using System;
using System.Collections;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Characters.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField, Header("Weapon")] private GameObject _projectile;
        [SerializeField] private float _projectileCost;
        [SerializeField] private float _attackCooldown;

        [SerializeField] private AudioClip[] _attackClips;
        
        [SerializeField, Header("Events")] private UpdateManaEvent _manaChangedEvent;
        [SerializeField] private TogglePlayerInputEvent _playerInputEvent;
        [SerializeField] private PlayAudioEvent _playAudioEvent;

        private bool _isAttacking;
        
        private InputManager _inputManager;

        private void OnEnable()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Attack.performed += Attack;
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

        private void Attack(InputAction.CallbackContext context)
        {
            if (!_isAttacking && _manaChangedEvent.Mana >= _projectileCost)
            {
                int selectedAttackClip = Random.Range(0, _attackClips.Length);
                _playAudioEvent.PlaySelectedClip(_attackClips[selectedAttackClip]);
                GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
                projectile.GetComponent<PlayerProjectile>().FaceRight = transform.GetComponent<PlayerController>().FaceRight;
                _manaChangedEvent.DecreaseMana(_projectileCost);
                StartCoroutine(AttackCooldownRoutine());
            }
        }

        private IEnumerator AttackCooldownRoutine()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(_attackCooldown);
            _isAttacking = false;
        }
    }
}