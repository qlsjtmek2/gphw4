using UnityEngine;
using UnityEngine.Events;

public interface IPlayerState
{
    void Start(Player player);
    void Collision(Player player, Collision other);
}

public class PlayerIdleState : IPlayerState
{
    public void Start(Player player)
    {
        // Do nothing
    }

    public void Collision(Player player, Collision other)
    {
        IAttackable attacker = other.gameObject.GetComponent<IAttackable>();
        if (attacker != null)
        {
            player.OnDamage(attacker, attacker.GetDamage());
        }
    }
}

public class PlayerDieState : IPlayerState
{
    public void Start(Player player)
    {
        player.OnDie?.Invoke();
    }

    public void Collision(Player player, Collision other)
    {
        // Do nothing
    }
}

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour, IDamageable
{
    public UnityEvent OnDie;

    private Health _health;
    private IPlayerState _state;

    private void Start()
    {
        _health = GetComponent<Health>();
        ChangeState(new PlayerIdleState());
    }

    public void OnCollisionEnter(Collision other)
    {
        _state.Collision(this, other);
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
        ChangeState(new PlayerDieState());
    }
    
    public void ChangeState(IPlayerState state)
    {
        _state = state;
        _state.Start(this);
    }
}