using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public interface IEnemyState
{
    void Start(Enemy enemy);
    void Collision(Enemy enemy, Collider other);
    void Update(Enemy enemy);
}

public class EnemyIdleState : IEnemyState
{
    private float _disableTimer = 0f;

    public void Start(Enemy enemy)
    {
        enemy.OnIdle.Invoke();
        enemy.SetOpacity(1f, 0f);
    }

    public void Collision(Enemy enemy, Collider other)
    {
        enemy.Stop();

        IAttackable attackable = other.gameObject.GetComponent<IAttackable>();
        if (attackable != null)
        {
            enemy.OnDamage(attackable, attackable.GetDamage());
        }
    }

    public void Update(Enemy enemy)
    {
        enemy.FollowTarget();

        _disableTimer += Time.deltaTime;
        if (_disableTimer >= enemy.LifeTime)
        {
            enemy.Die();
        }
    }
}

public class EnemyAttackState : IEnemyState
{
    public void Start(Enemy enemy)
    {
        enemy.OnAttacked.Invoke();
        enemy.Stop();
    }

    public void Collision(Enemy enemy, Collider other)
    {
        // Do nothing
    }
    
    public void Update(Enemy enemy)
    {
        enemy.transform.LookAt(enemy.Target);
    }
}

public class EnemyDieState : IEnemyState
{
    public void Start(Enemy enemy)
    {
        enemy.OnDied.Invoke();
        enemy.Stop();
        enemy.StartCoroutine(DisableAfterDelay(enemy, 1f));
    }

    public void Collision(Enemy enemy, Collider other)
    {
        // Do nothing
    }
    
    public void Update(Enemy enemy)
    {
        // Do nothing
    }

    private IEnumerator DisableAfterDelay(Enemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        float duration = 1f;
        enemy.SetOpacity(0f, duration);
        yield return new WaitForSeconds(duration);

        enemy.gameObject.SetActive(false);
    }
}

public class EnemyDisableState : IEnemyState
{
    public void Start(Enemy enemy)
    {
        enemy.OnDisabled.Invoke();
    }

    public void Collision(Enemy enemy, Collider other)
    {
        // Do nothing
    }
    
    public void Update(Enemy enemy)
    {
        // Do nothing
    }
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IDamageable
{
    public float LifeTime = 60f;
    public Transform Target;
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public UnityEvent OnIdle;
    public UnityEvent OnDied;
    public UnityEvent OnAttacked;
    public UnityEvent OnDisabled;

    private IEnemyState _state;
    private NavMeshAgent _agent;
    private Transform _savedTarget;
    private OpacityController _opacityController;
    private Health _health;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _opacityController = GetComponentInChildren<OpacityController>();
        _health = GetComponent<Health>();
        _savedTarget = Target;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        ChangeState(new EnemyIdleState());
    }

    void OnDisable()
    {
        ChangeState(new EnemyDisableState());
        EnemyManager.Instance.ReturnToPool(this);
    }

    void Update()
    {
        _state.Update(this);
    }

    void OnTriggerEnter(Collider other)
    {
        _state.Collision(this, other);
    }

    public void FollowTarget()
    {
        _agent.isStopped = false;
        _agent.SetDestination(Target.position);
    }

    public void Stop()
    {
        _agent.isStopped = true;
    }

    public void SwitchTarget(Collider collider)
    {
        Target = collider.transform;
    }

    public void RestoreTarget()
    {
        Target = _savedTarget;
    }

    public void ChangeState(IEnemyState state)
    {
        _state = state;
        _state.Start(this);
    }

    public void SetOpacity(float opacity, float duration = 1f)
    {
        _opacityController.FadeTo(opacity, duration);
    }

    public void StartAttack()
    {
        ChangeState(new EnemyAttackState());
    }

    public void StopAttack()
    {
        ChangeState(new EnemyIdleState());
    }
    
    public void OnDamage(IAttackable attacker, float damage)
    {
        _health.Damage(damage);

        if (_health.Value <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ChangeState(new EnemyDieState());
    }
}