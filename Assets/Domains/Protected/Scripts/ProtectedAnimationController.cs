using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedAnimationController : MonoBehaviour
{
    private readonly string DEATH_PARAM_NAME = "Death";

    [SerializeField] private Animator _animator;

    void Start()
    {
        
    }

    public void OnDead(IProtectedState prevState, IProtectedState newState)
    {
        if (newState is DieState)
        {
            _animator.SetBool(DEATH_PARAM_NAME, true);
        }
    }
}
