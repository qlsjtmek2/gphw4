using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

namespace Domain.Enemy
{
    public interface IEnemyState
    {
        void Collision(Enemy enemy, Collider other);
        void Update(Enemy enemy);
    }

    public class EnableState : IEnemyState
    {
        private float _disableTimer = 0f;

        public void Collision(Enemy enemy, Collider other)
        {
            enemy.Stop();

            if (other.tag == "Bullet")
            {
                enemy.gameObject.SetActive(false);
            }
        }

        public void Update(Enemy enemy)
        {
            enemy.Move();

            _disableTimer += Time.deltaTime;
            if (_disableTimer >= enemy.LifeTime)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public class DisableState : IEnemyState
    {
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
    public class Enemy : MonoBehaviour
    {
        public float LifeTime = 60f;
        public Vector3 TargetPosition;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        private IEnemyState _state;
        private NavMeshAgent _agent;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            _state = new EnableState();
        }

        void OnDisable()
        {
            _state = new DisableState();
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

        public void Move()
        {
            _agent.isStopped = false;
            _agent.SetDestination(TargetPosition);
        }

        public void Stop()
        {
            _agent.isStopped = false;
        }
    }
}
