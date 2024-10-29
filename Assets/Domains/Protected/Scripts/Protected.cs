using UnityEngine;
using UnityEngine.Events;

public interface IProtectedState
{
    void Start(Protected prot);
    void Collision(Protected prot, Collision other);
}

public class ProtectedIdleState : IProtectedState
{
    public void Start(Protected prot)
    {
        // Do nothing
    }

    public void Collision(Protected prot, Collision other)
    {
        IAttackable attacker = other.gameObject.GetComponent<IAttackable>();
        if (attacker != null)
        {
            prot.OnDamage(attacker, attacker.GetDamage());
        }
    }
}

public class ProtectedDieState : IProtectedState
{
    public void Start(Protected prot)
    {
        prot.OnDie?.Invoke();
    }

    public void Collision(Protected prot, Collision other)
    {
        // Do nothing
    }
}

[RequireComponent(typeof(Health))]
public class Protected : MonoBehaviour, IDamageable
{
    public UnityEvent OnDie;

    private IProtectedState _state;
    private Health _health;

    void Start()
    {
        _health = GetComponent<Health>();
        ChangeState(new ProtectedIdleState());
    }

    void OnCollisionEnter(Collision other)
    {
        _state.Collision(this, other);
    }

    public IProtectedState ChangeState(IProtectedState newState)
    {
        IProtectedState prevState = _state;
        _state = newState;

        return prevState;
    }

    public void OnDamage(IAttackable attacker, float damage)
    {
        _health.Damage(damage);

        if (_health.Value <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ChangeState(new ProtectedDieState());
    }
}
