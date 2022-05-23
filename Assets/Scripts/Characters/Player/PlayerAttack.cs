using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private ManaManagerSO _manaManager;
        [SerializeField] private float _projectileCost;
        [SerializeField] private float _attackCooldown;

        private bool _isAttacking;
        
        private InputManager _inputManager;

        private void Start()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Attack.performed += Attack;
            _inputManager.Player.Enable();
        }

        private void Attack(InputAction.CallbackContext context)
        {
            if (!_isAttacking && _manaManager.Mana >= _projectileCost)
            {
                //attack logic here maybe a pool?
                Debug.Log("PlayerAttack::Is shooting projectiles");
                _manaManager.DecreaseMana(_projectileCost);
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