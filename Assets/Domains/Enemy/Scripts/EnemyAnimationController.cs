using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private readonly string IS_DIE_PARAM_NAME = "IsDie";
    private readonly string IS_ATTACK_PARAM_NAME = "IsAttack";

    [SerializeField] private Animator _animator;

    private Vector2 _smoothInputVector;

    public void OnDie()
    {
        _animator.SetBool(IS_DIE_PARAM_NAME, true);
    }

    public void OnRevive()
    {
        _animator.SetBool(IS_DIE_PARAM_NAME, false);
    }

    public void StartAttack()
    {
        _animator.SetBool(IS_ATTACK_PARAM_NAME, true);
    }

    public void StopAttack()
    {
        _animator.SetBool(IS_ATTACK_PARAM_NAME, false);
    }
}
