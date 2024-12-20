using UnityEngine.InputSystem;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Speed))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("가속도")]
    public float Acceleration = 10.0f;
    
    [Tooltip("기울일 정도 (각도)")]
    public float TiltAmount = 15f;

    [Tooltip("기울임 회전 속도")]
    public float RotationSpeed = 5f;

    public float Speed
    {
        get
        {
            return _speedComp.Value;
        }
        set
        {
            _speedComp.Value = value;
        }
    }

    public Vector3 Velocity
    {
        get { return _rigid.velocity; }
    }

    public bool IsMoving
    {
        get 
        {
            return _moveInput.sqrMagnitude > 0.1f;
        }
    }

    public Vector2 LocalDirection
    {
        get 
        {
            return _moveInput;
        }
    }

    /* privite fields */
    private Rigidbody _rigid;
    private Speed _speedComp;
    private Vector2 _moveInput;
    
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _speedComp = GetComponent<Speed>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // 2D 입력을 3D로 변환 (z축으로 전진, x축으로 좌우 이동)
        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        // moveDirection을 로컬 좌표계 기준으로 변환 (플레이어가 바라보는 방향으로 이동)
        Vector3 forceDirection = transform.TransformDirection(moveDirection);
        forceDirection.y = 0;

        // AddForce로 힘을 가해 이동 적용
        _rigid.AddForce(forceDirection * Acceleration, ForceMode.Acceleration);

        // 최대 속도 제한
        ClampMaxSpeed();
    }
    
    private void ClampMaxSpeed()
    {
        // 수평 속도 (x, z 축)만 고려하여 최대 속도를 제한
        Vector3 horizontalVelocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z);

        if (horizontalVelocity.magnitude > Speed)
        {
            // 수평 속도의 방향을 유지하면서 최대 속도로 제한
            horizontalVelocity = horizontalVelocity.normalized * Speed;
            
            // y축 속도는 유지하고, x와 z 축 속도만 조정
            _rigid.velocity = new Vector3(horizontalVelocity.x, _rigid.velocity.y, horizontalVelocity.z);
        }
    }
}
