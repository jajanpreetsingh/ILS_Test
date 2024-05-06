using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navAgent;

    [SerializeField]
    private float _maxHealth = 100;

    private Rigidbody _playerBody;
    private Animator _animator;

    private float _currentForwardSpeed;
    private float _currentStrafeSpeed;

    private float _minDistanceToAttack = 0.3f;

    private float _currentHealth;

    private bool _attacking = false;
    private bool _initialized = false;

    public bool Attacking => _attacking;

    private PlayerMovement _target;

    #region Monobehavior
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerBody = GetComponent<Rigidbody>();

        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_currentHealth == 0 || !_initialized)
        {
            return;
        }

        float distance = Vector3.Distance(_target.transform.position, transform.position);

        _currentForwardSpeed = 2;

        if (distance > 50)
        {
            _currentForwardSpeed = 6;
        }
        else if (distance > 30)
        {
            _currentForwardSpeed = 4;
        }

        if (_target != null)
        {
            _navAgent.SetDestination(_target.transform.position);
            _navAgent.velocity = transform.forward * _currentForwardSpeed;
            _animator.SetFloat("SpeedZ", _currentForwardSpeed);
        }
        else
        {
            _attacking = false;
            _currentForwardSpeed = 0;
            _navAgent.SetDestination(transform.position);
            _navAgent.velocity = Vector2.zero;
            _animator.SetFloat("SpeedZ", _currentForwardSpeed);
        }

        if (Vector3.Distance(_target.transform.position, transform.position) <= _minDistanceToAttack)
        {
            OnAttack();
        }
        else
        {
            _attacking = false;
        }
    }
    #endregion


    #region Private Methods
    private void OnAttack()
    {
        _attacking = true;
        _navAgent.SetDestination(_target.transform.position);
        _navAgent.velocity = transform.forward * _currentForwardSpeed;
        transform.LookAt(_target.transform.position);
        _animator.SetTrigger("Attack");
    }

    private void OnDamage()
    {
        _currentHealth -= 10;

        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth < 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        _animator.SetTrigger("Death");
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    #endregion

    public void SetPlayerTarget(PlayerMovement target)
    {
        _target = target;
    }

    public void Init()
    {
        _initialized = true;
    }
}
