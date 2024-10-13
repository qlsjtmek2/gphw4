using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Bullet;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }
    public Bullet Prefab;
    public int PoolSize = 100;
    
    private Queue<Bullet> _pool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _pool = new Queue<Bullet>();
        
        for (int i = 0; i < PoolSize; i++) {
            Instantiate(Prefab.gameObject);
        }
    }

    public Bullet GetFromPool() {
        if (_pool.Count > 0) {
            Bullet obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        return null;
    }

    public void ReturnToPool(Bullet obj) {
        _pool.Enqueue(obj);
    }
}
