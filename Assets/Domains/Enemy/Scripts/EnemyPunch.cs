using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Damage))]
public class EnemyHand : MonoBehaviour, IAttackable
{
    public UnityEvent OnAttackEvent;
    private Damage _damage;

    void Start()
    {
        _damage = GetComponent<Damage>();
    }

    public float GetDamage()
    {
        return _damage.Value;
    }

    public void OnAttack(IDamageable target)
    {
        OnAttackEvent?.Invoke();
    }

    public void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            OnAttack(damageable);
        }
    }
}
