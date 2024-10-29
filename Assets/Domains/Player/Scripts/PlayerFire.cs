using UnityEngine.InputSystem;
using UnityEngine;
using Domain.Bullet;
using UnityEngine.Events;

namespace Movement.Adapter
{
    public class PlayerFire : MonoBehaviour
    {
        public Transform BulletSpawnPoint;
        public UnityEvent<Vector3> OnFired;

        public void OnFire(InputValue value)
        {
            Bullet bullet = BulletManager.Instance.GetFromPool();

            if (bullet == null) return;
            
            bullet.Position = BulletSpawnPoint.position;
            bullet.Direction = Camera.main.transform.forward;
            bullet.transform.rotation = Quaternion.LookRotation(bullet.Direction);
            OnFired?.Invoke(bullet.Position);
        }
    }
}