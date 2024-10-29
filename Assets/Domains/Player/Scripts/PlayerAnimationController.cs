using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly string X_PARAM_NAME = "X";
    private readonly string Y_PARAM_NAME = "Y";
    private readonly string SPEED_PARAM_NAME = "Speed";
    private readonly string JUMP_PARAM_NAME = "Jump";
    private readonly string GROUNDED_PARAM_NAME = "Grounded";
    private readonly string FIRE_PARAM_NAME = "Fire";
    private readonly string IS_DIE_PARAM_NAME = "IsDie";

    [SerializeField] private Speed _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMove _playerMove;

    private Vector2 _smoothInputVector;
    
    void FixedUpdate()
    {
        _smoothInputVector = Vector2.Lerp(_smoothInputVector, _playerMove.LocalDirection, 0.15f);

        _animator.SetFloat(X_PARAM_NAME, _smoothInputVector.x);
        _animator.SetFloat(Y_PARAM_NAME, _smoothInputVector.y);

        _animator.SetFloat(SPEED_PARAM_NAME, Mathf.Max(_playerMove.Velocity.magnitude / _speed.InitValue, 1));
    }

    public void OnJumped()
    {
        _animator.SetBool(JUMP_PARAM_NAME, true);
        _animator.SetBool(GROUNDED_PARAM_NAME, false);
    }

    public void OnLanded()
    {
        _animator.SetBool(GROUNDED_PARAM_NAME, true);
        _animator.SetBool(JUMP_PARAM_NAME, false);
    }

    public void OnFired(Vector3 pos)
    {
        _animator.SetBool(FIRE_PARAM_NAME, false);
        _animator.SetBool(FIRE_PARAM_NAME, true);
    }

    public void OnDie()
    {
        _animator.SetBool(IS_DIE_PARAM_NAME, true);
    }
}
