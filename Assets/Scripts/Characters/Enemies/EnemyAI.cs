using System;
using UnityEngine;

using Pathfinding;

namespace Characters.Enemies
{
    public abstract class EnemyAI : MonoBehaviour
    {
        [SerializeField, Header("Health")] protected int _health;
        
        [SerializeField, Header("Pathfinding")] protected Transform _target;
        [SerializeField] protected float _activateDistance = 10f;
        [SerializeField] protected float _pathUpdateSeconds = 0.5f;
        [SerializeField] protected float _nextWaypointDistance = 3;
        [SerializeField] protected float _jumpNodeHeightRequirement = 0.8f;

        [SerializeField, Header("Physics")] protected float _speed = 300;
        [SerializeField] protected float _jumpForce = 5f;
        [SerializeField] protected float _jumpCheckOffset = 0.5f;

        [SerializeField, Header("Behaviours")] protected bool _followEnabled;
        [SerializeField] private bool _jumpEnabled;
        [SerializeField] private bool _directionLookEnabled;

        private Path _path;
        private Seeker _seeker;
        protected Rigidbody2D _rigid;
        private int _currentWaypoint;
        private bool _isGrounded;
        protected bool _faceRight;

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
                PathFollow();
            }
        }

        private void PathFollow()
        {
            if (_path == null) return;
            if (_currentWaypoint >= _path.vectorPath.Count) return;

            //direction calculation
            Vector2 direction = ((Vector2) _path.vectorPath[_currentWaypoint] - _rigid.position).normalized;
            Vector2 force = direction * (_speed * Time.deltaTime);
            
            //movement
            _rigid.AddForce(force);
            
            //jump
            if (_jumpEnabled)
            {
                //check if grounded
                _isGrounded = Physics2D.Raycast(_rigid.position, Vector2.down, _jumpCheckOffset, 1 << 3);
                if (direction.y > _jumpNodeHeightRequirement && _isGrounded)
                {
                    _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpForce);
                }
            }

            //next waypoint
            float distance = Vector2.Distance(_rigid.position, _path.vectorPath[_currentWaypoint]);
            if (distance < _nextWaypointDistance)
            {
                _currentWaypoint++;
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
        }

        private bool TargetInDistance()
        {
            return Vector2.Distance(transform.position, _target.position) < _activateDistance;
        }
    }
}