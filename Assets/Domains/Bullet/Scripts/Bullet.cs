using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Domain.Bullet
{
    public interface IBulletState
    {
        void Collision(Bullet bullet);
        void Update(Bullet bullet);
    }

    public class EnableState : IBulletState
    {
        private float _disableTimer = 0f;

        public void Collision(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        public void Update(Bullet bullet)
        {
            bullet.transform.position += bullet.Direction * bullet.Velocity * Time.deltaTime;

            _disableTimer += Time.deltaTime;
            if (_disableTimer >= bullet.LifeTime)
            {
                bullet.gameObject.SetActive(false);
            }
        }
    }

    public class DisableState : IBulletState
    {
        public void Collision(Bullet bullet)
        {
            // Do nothing
        }

        public void Update(Bullet bullet)
        {
            // Do nothing
        }
    }

    public class Bullet : MonoBehaviour
    {
        public Vector3 Direction;
        public float Velocity = 10f;
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        public float LifeTime = 5f;

        private IBulletState _state;

        void Awake()
        {
            gameObject.SetActive(false);
        }

        void OnEnable()
        {
            _state = new EnableState();
        }

        void OnDisable()
        {
            _state = new DisableState();
            BulletManager.Instance.ReturnToPool(this);
        }

        void Update()
        {
            _state.Update(this);
        }

        void OnTriggerEnter(Collider other)
        {
            _state.Collision(this);
        }
    }
}