using UnityEngine;
using UnityEngine.Events;

public interface IProtectedState
    {
        void Collision(Protected obj, Collision other);
    }

    public class AliveState : IProtectedState
    {
        public void Collision(Protected obj, Collision other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                obj.ChangeState(new DieState());
            }
        }
    }

    public class DieState : IProtectedState
    {
        public void Collision(Protected obj, Collision other)
        {
            // Do nothing
        }
    }

public class Protected : MonoBehaviour
{
    public UnityEvent<IProtectedState, IProtectedState> OnStateChange;
    private IProtectedState _state;

    void Start()
    {
        ChangeState(new AliveState());
    }

    void OnCollisionEnter(Collision other)
    {
        _state.Collision(this, other);
    }

    public IProtectedState ChangeState(IProtectedState newState)
    {
        IProtectedState prevState = _state;
        _state = newState;

        OnStateChange?.Invoke(prevState, newState);

        return prevState;
    }
}
