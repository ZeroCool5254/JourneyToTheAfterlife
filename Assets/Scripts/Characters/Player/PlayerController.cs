using System;
using System.Collections;
using ScriptableObjects;
using ScriptableObjects.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField, Header("Base Stats")] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpCheckOffset;
        [SerializeField] private float _wallJumpCheckOffset;
        [SerializeField] private float _xWallJumpForce;
        [SerializeField] private float _yWallJumpForce;
        [SerializeField] private float _wallSlideSpeed;
        
        public bool FaceRight { get; private set; }
        
        [SerializeField, Header("Events")] private UpdateHealthEvent _healthChangedEvent;
        [SerializeField] private TogglePlayerInputEvent _playerInputEvent;

        private bool _isGrounded;
        private bool _isTouchingFront;
        private bool _wallSliding;
        private bool _wallJumping;
        private bool _isDead;

        private Rigidbody2D _rigid;
        private PlayerAbility _playerAbility;
        
        private InputManager _inputManager;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            //enable Player input event
            //this will have to be removed once a working GameManager is in place
            _playerInputEvent.InputChangedEvent.Invoke(true);
            //delayed UI update event
            _healthChangedEvent.HealthChangedEvent.Invoke(_healthChangedEvent.Health);
        }

        private void OnEnable()
        {
            _inputManager = new InputManager();
            _inputManager.Player.Jump.performed += Jump;
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

        private void FixedUpdate()
        {
            Movement();
            Vector2 position = _rigid.position;
            _isGrounded = Physics2D.Raycast(position, Vector2.down, _jumpCheckOffset, 1 << 3);
            Vector2 rayPos = new Vector2(position.x - (_wallJumpCheckOffset / 2), position.y);
            _isTouchingFront = Physics2D.Raycast(rayPos, Vector2.right, _wallJumpCheckOffset, 1 << 3);
        }
        
        private void Movement()
        {
            float xPos = _inputManager.Player.Movement.ReadValue<float>();
            _rigid.velocity = new Vector2(xPos * _speed, _rigid.velocity.y);
            //show movement animations
            if (xPos > 0)
            {
                FaceRight = true;
            }
            else if (xPos < 0)
            {
                FaceRight = false;
            }

            if (_isTouchingFront && xPos != 0) _wallSliding = true;
            else _wallSliding = false;
            
            if (_wallSliding)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, -_wallSlideSpeed);
            }

            if (_wallJumping)
            {
                _rigid.velocity = new Vector2(_xWallJumpForce * -xPos, _yWallJumpForce);
            }
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (_isGrounded && !_wallSliding)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                //show jump animations
            }

            if (_wallSliding)
            {
                StartCoroutine(WallJumpCooldownRoutine());
            }
        }

        private IEnumerator WallJumpCooldownRoutine()
        {
            _wallJumping = true;
            yield return new WaitForSeconds(0.2f);
            _wallJumping = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("EnemyWeapon") && !_isDead)
            {
                Damage();
            }
        }

        public void Damage()
        {
            //play damage animation
            _healthChangedEvent.DecreaseHealth();

            if (_healthChangedEvent.Health <= 0)
            {
                _isDead = true;
                //play death animation
                //Game over
            }
        }
    }
}
