using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace Domain.Bullet
{
    public interface IBulletState
    {
        void Start(Bullet bullet);
        void Collision(Bullet bullet, Collider other);
        void Update(Bullet bullet);
    }

    public class BulletIdleState : IBulletState
    {
        private float _disableTimer = 0f;

        public void Start(Bullet bullet)
        {
            bullet.OnIdleEvent?.Invoke();
        }

        public void Collision(Bullet bullet, Collider other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                bullet.OnAttack(damageable);
            }
        }

        public void Update(Bullet bullet)
        {
            bullet.transform.position += bullet.Direction * bullet.Velocity * Time.deltaTime;

            _disableTimer += Time.deltaTime;
            if (_disableTimer >= bullet.LifeTime)
            {
                bullet.ChangeState(new BulletBrokenState());
            }
        }
    }

    public class BulletBrokenState : IBulletState
    {
        public void Start(Bullet bullet)
        {
            bullet.OnBrokenEvent?.Invoke();
            bullet.gameObject.SetActive(false);
            // bullet.StartCoroutine(DisableAfterDelay(bullet, 1f));
        }

        public void Collision(Bullet bullet, Collider other)
        {
            // Do nothing
        }

        public void Update(Bullet bullet)
        {
            // Do nothing
        }

        private IEnumerator DisableAfterDelay(Bullet bullet, float delay)
        {
            yield return new WaitForSeconds(delay);
            bullet.gameObject.SetActive(false);
        }
    }

    public class BulletDisableState : IBulletState
    {
        public void Start(Bullet bullet)
        {
            bullet.OnDisableEvent?.Invoke();
            BulletManager.Instance.ReturnToPool(bullet);
        }

        public void Collision(Bullet bullet, Collider other)
        {
            // Do nothing
        }

        public void Update(Bullet bullet)
        {
            // Do nothing
        }
    }

    [RequireComponent(typeof(Damage))]
    public class Bullet : MonoBehaviour, IAttackable
    {
        public UnityEvent OnIdleEvent;
        public UnityEvent OnBrokenEvent;
        public UnityEvent OnDisableEvent;
        public Vector3 Direction;
        public float Velocity = 10f;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public float LifeTime = 5f;

        private IBulletState _state;
        private Damage _damage;

        void Awake()
        {
            _damage = GetComponent<Damage>();
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            ChangeState(new BulletIdleState());
        }

        void OnDisable()
        {
            ChangeState(new BulletDisableState());
        }

        void Update()
        {
            _state.Update(this);
        }

        void OnTriggerEnter(Collider other)
        {
            _state.Collision(this, other);
        }
        
        public void OnAttack(IDamageable target)
        {
            ChangeState(new BulletBrokenState());
        }

        public float GetDamage()
        {
            return _damage.Value;
        }

        public void ChangeState(IBulletState newState)
        {
            _state = newState;
            _state.Start(this);
        }
    }
}