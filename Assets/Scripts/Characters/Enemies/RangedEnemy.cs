using System.Collections;
using UnityEngine;

namespace Characters.Enemies
{
    public class RangedEnemy : EnemyAI
    {
        [SerializeField, Header("Weapon")] private GameObject _projectile;
        [SerializeField] private float _firingDistance;
        [SerializeField] private float _fireRate;
        private bool _isFiring;

        public void Update()
        {
            if (TargetInFiringDistance() && !_isFiring)
            {
                FireProjectile();
                StartCoroutine(FireCooldownRoutine());
            }
        }

        private void FireProjectile()
        {
            GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            projectile.transform.rotation = Quaternion.RotateTowards(projectile.transform.rotation, _target.rotation, 3 * Time.deltaTime);
            projectile.GetComponent<EnemyProjectile>().FaceRight = _faceRight;
        }

        private bool TargetInFiringDistance()
        {
            return Vector2.Distance(transform.position, _target.position) < _firingDistance;
        }

        private IEnumerator FireCooldownRoutine()
        {
            _isFiring = true;
            yield return new WaitForSeconds(_fireRate);
            _isFiring = false;
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