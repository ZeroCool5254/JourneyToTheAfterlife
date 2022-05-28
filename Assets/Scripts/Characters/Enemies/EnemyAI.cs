using UnityEngine;

using Pathfinding;

namespace Characters.Enemies
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField, Header("Base Stats")] protected int _health = 3;
        [SerializeField] private float _speed = 300;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _jumpCheckOffset = 1.2f;
        [SerializeField] private float _wallJumpCheckOffset = 1.2f;
        [SerializeField] private float _xWallJumpForce;
        [SerializeField] private float _yWallJumpForce;
        [SerializeField] private float _wallSlideSpeed;

        [SerializeField, Header("Pathfinding")] protected Transform _target;
        [SerializeField] private float _activateDistance = 10f;
        [SerializeField] private float _pathUpdateSeconds = 0.5f;
        [SerializeField] private float _nextWaypointDistance = 3;
        [Tooltip("The height the pathfinding node has to be before trying to jump")]
        [SerializeField] private float _jumpNodeHeightRequirement = 0.8f;

        [SerializeField, Header("Behaviours")] protected bool _followEnabled;
        [SerializeField] private bool _jumpEnabled;
        [SerializeField] private bool _wallJumpEnabled;
        [SerializeField] private bool _wallSlideEnabled;
        [SerializeField] private bool _directionLookEnabled;

        private Path _path;
        private Seeker _seeker;
        private Rigidbody2D _rigid;
        private int _currentWaypoint;
        
        private bool _isGrounded;
        private bool _isTouchingFront;
        private bool _wallSliding;
        private bool _wallJumping;
        protected bool _faceRight;
        protected bool _isDead;

        public virtual void Init()
        {
            _seeker = GetComponent<Seeker>();
            _rigid = GetComponent<Rigidbody2D>();
            InvokeRepeating("UpdatePath", 0f, _pathUpdateSeconds);
        }
        private void Start()
        {
            Init();
        }

        private void UpdatePath()
        {
            if (TargetInDistance() && _followEnabled && _seeker.IsDone())
            {
                _seeker.StartPath(_rigid.position, _target.position, OnPathComplete);
            }
        }

        private void OnPathComplete(Path path)
        {
            if (!path.error)
            {
                _path = path;
                _currentWaypoint = 0;
            }
        }

        public virtual void FixedUpdate()
        {
            if (TargetInDistance() && _followEnabled)
            {
                Movement();
            }
        }

        private void Movement()
        {
            if (_path == null) return;
            if (_currentWaypoint >= _path.vectorPath.Count) return;

            //direction calculation
            Vector2 direction = ((Vector2) _path.vectorPath[_currentWaypoint] - _rigid.position).normalized;
            Vector2 force = direction * (_speed * Time.deltaTime);
            
            //movement
            _rigid.AddForce(force);
            
            //wall sliding
            if (_wallSlideEnabled)
            {
                Vector2 position = _rigid.position;
                Vector2 rayPos = new Vector2(position.x - (_wallJumpCheckOffset / 2), position.y);
                _isTouchingFront = Physics2D.Raycast(rayPos, Vector2.right, _wallJumpCheckOffset, 1 << 3);
                if (_isTouchingFront && !_isGrounded) _wallSliding = true;
                else _wallSliding = false;
                if (_wallSliding)
                {
                    _rigid.velocity = new Vector2(_rigid.velocity.x, -_wallSlideSpeed);
                }
            }
            
            //jump
            if (_jumpEnabled)
            {
                //check if grounded
                _isGrounded = Physics2D.Raycast(_rigid.position, Vector2.down, _jumpCheckOffset, 1 << 3);
                //check if the next node is higher than the jump requirement
                if (direction.y > _jumpNodeHeightRequirement)
                {
                    //if the enemy is grounded then the enemy can perform a regular jump
                    if (_isGrounded)
                    {
                        _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                    }
                    //if the enemy is sliding, and is allowed to wall jump then perform a wall jump
                    else if (_wallSliding && _wallJumpEnabled)
                    {
                        float jumpDir = _rigid.velocity.x;
                        _rigid.velocity = new Vector2(_xWallJumpForce * -jumpDir, _yWallJumpForce);
                    }
                }
            }

            //direction handler
            if (_directionLookEnabled)
            {
                if (_rigid.velocity.x > 0.01f)
                {
                    _faceRight = true;
                }
                else if (_rigid.velocity.x < -0.01f)
                {
                    _faceRight = false;
                }
            }
            
            //next waypoint
            float distance = Vector2.Distance(_rigid.position, _path.vectorPath[_currentWaypoint]);
            if (distance < _nextWaypointDistance)
            {
                _currentWaypoint++;
            }
        }

        private bool TargetInDistance()
        {
            return Vector2.Distance(transform.position, _target.position) < _activateDistance;
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