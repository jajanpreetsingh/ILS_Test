using CustomUtilityScripts;
using UnityEngine;
using Logger = CustomUtilityScripts.Logger;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputMap _inputMap;

    [SerializeField]
    private Range _forwardSpeedRange;

    [SerializeField]
    private float _maxStrafeSpeed;

    [SerializeField]
    private float _maxHealth = 100;

    [SerializeField]
    private float _jumpForce;

    private Rigidbody _playerBody;
    private Animator _animator;

    private float _currentForwardSpeed;
    private float _currentStrafeSpeed;

    private float _currentHealth;

    private bool _attacking = false;

    public bool Attacking => _attacking;

    #region Monobehavior
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerBody = GetComponent<Rigidbody>();

        _currentHealth = _maxHealth;

        _inputMap.Sanitize();
    }

    private void Update()
    {
        SetForwardMotion();

        if(_currentForwardSpeed == 0)
        {
            SetSideWaysMotion();
        }

        OnPlayerJump();

        OnAttack();
    }
    #endregion

    #region Private Methods
    private void SetForwardMotion()
    {
        InputAction context = InputAction.Move;

        if (!_inputMap.InputActionMap.ContainsKey(context))
        {
            return;
        }

        InputStatus previousState = _inputMap.GetInputStatus(context);

        InputStatus newState = _inputMap.RefreshInputStatus(context);

        bool resetSpeed = (previousState.ReverseMotion != newState.ReverseMotion);

        if (resetSpeed || !newState.Status)
        {
            _currentForwardSpeed = 0f;
        }
        else
        {
            _currentForwardSpeed += _forwardSpeedRange.Max * Time.deltaTime * 0.25f;
            _forwardSpeedRange.Clamp(ref _currentForwardSpeed);
        }

        int direction = newState.ReverseMotion ? -1 : 1;
        _playerBody.velocity = transform.forward * (direction * _currentForwardSpeed);
        _animator.SetFloat("SpeedZ", _currentForwardSpeed * direction);
    }

    private void OnPlayerJump()
    {
        InputAction context = InputAction.Jump;

        if (!_inputMap.InputActionMap.ContainsKey(context))
        {
            return;
        }

        InputStatus newState = _inputMap.RefreshInputStatus(context);

        if (newState.Status)
        {
            _playerBody.AddForce(_jumpForce *
                (_playerBody.velocity.normalized + transform.up).normalized);

            _animator.SetTrigger("Jump");
        }
    }

    private void SetSideWaysMotion()
    {
        InputAction context = InputAction.Strafe;

        if (!_inputMap.InputActionMap.ContainsKey(context))
        {
            return;
        }

        InputStatus previousState = _inputMap.GetInputStatus(context);

        InputStatus newState = _inputMap.RefreshInputStatus(context);

        bool resetSpeed = (previousState.ReverseMotion != newState.ReverseMotion);

        if (resetSpeed || !newState.Status)
        {
            _currentStrafeSpeed = 0f;
        }
        else
        {
            _currentStrafeSpeed = _maxStrafeSpeed;
        }

        int direction = newState.ReverseMotion ? -1 : 1;

        _playerBody.velocity = transform.right * (direction * _currentStrafeSpeed);        
        _animator.SetFloat("SpeedX", _currentStrafeSpeed * direction);
    }

    private void OnAttack()
    {
        InputAction context = InputAction.Attack;

        if (!_inputMap.InputActionMap.ContainsKey(context))
        {
            return;
        }

        InputStatus newState = _inputMap.RefreshInputStatus(context);

        if (newState.Status)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void OnDamage()
    {
        _animator.SetTrigger("Hit");
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
        EnemyController enemy = collision.collider.gameObject.GetComponent<EnemyController>();

        if (enemy== null || !enemy.Attacking)
        {
            return;
        }

        OnDamage();
    }
    #endregion
}
