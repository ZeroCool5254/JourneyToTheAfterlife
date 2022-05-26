using System.Collections;
using Contracts;
using UnityEngine;

namespace Characters.Enemies
{
    public class MeleeEnemy : EnemyAI, IDamageable
    {
        [SerializeField, Header("Weapon")] private GameObject _weapon;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _attackRate;
        private bool _isAttacking;

        private void Update()
        {
            if (TargetInFiringDistance() && !_isAttacking)
            {
                Attack();
                StartCoroutine(AttackCooldownRoutine());
            }
        }

        private void Attack()
        {
            Debug.Log("DemonicImpEnemy::Should Attack");
            //show the attack anim and set the collider to active
        }

        private bool TargetInFiringDistance()
        {
            return Vector2.Distance(transform.position, _target.position) < _attackDistance;
        }

        private IEnumerator AttackCooldownRoutine()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(_attackRate);
            _isAttacking = false;
        }

        public void Damage()
        {
            _health--;

            if (_health <= 0)
            {
                //show death animation
                Destroy(gameObject, 2f);
            }
        }
    }
}