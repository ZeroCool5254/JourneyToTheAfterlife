﻿using UnityEngine;

using Characters.Player;

namespace Characters.Enemies
{
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeDuration;
        public bool FaceRight { get; set; }
        private int _direction;
        private Rigidbody2D _rigid;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Destroy(gameObject, _lifeDuration);
        }

        private void FixedUpdate()
        {
            _direction = FaceRight ? 1 : -1;
            _rigid.velocity = new Vector2(_direction *(_speed * Time.deltaTime), _rigid.velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Damage();
                Damage();
            }

            if (other.CompareTag("Ground"))
            {
                Damage();
            }
        }

        public void Damage()
        {
            Destroy(gameObject);
        }
    }
}