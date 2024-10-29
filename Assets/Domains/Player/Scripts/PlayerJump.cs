using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [Tooltip("점프의 힘을 설정합니다.")]
    public float jumpForce = 5.0f;

    [Tooltip("땅 레이어")]
    public LayerMask GroundMask;

    public UnityEvent OnJumped;
    public UnityEvent OnLanded;

    private Rigidbody _rigid;
    private bool _isGrounded;
    private bool _isJumping;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public void OnJump(InputValue value)
    {
        if (_isGrounded && !_isJumping)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _isJumping = true;
        _rigid.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

        OnJumped?.Invoke();
    }

    private void Update()
    {
        if (_isGrounded && _isJumping)
        {
            _isJumping = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, GroundMask))
        {
            Land();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, GroundMask))
        {
            _isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!_isGrounded)
        {
            if (IsInLayerMask(collision.gameObject, GroundMask))
            {
                Land();
            }
        }
    }

    private void Land()
    {
        _isGrounded = true;
        OnLanded?.Invoke();
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}
