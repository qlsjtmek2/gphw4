using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float InitValue;
    
    private float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = Math.Max(value, 0);
        }
    }

    void Start()
    {
        _value = InitValue;
    }

    public void Damage(float damage)
    {
        Value -= damage;
    }
}