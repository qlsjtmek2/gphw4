public interface IAttackable
{
    void OnAttack(IDamageable target);
    float GetDamage();
}