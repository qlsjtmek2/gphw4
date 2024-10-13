using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCircle : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Circle의 중심점")]
    public GameObject MiddlePoint;

    [Tooltip("물체에 가할 힘의 크기")]
    public float forceStrength = 10.0f;

    private void OnTriggerExit(Collider other)
    {
        // 충돌한 물체가 Rigidbody를 가지고 있는지 확인
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // MiddlePoint 방향 계산
            Vector3 directionToMiddle = (MiddlePoint.transform.position - other.transform.position).normalized;

            // MiddlePoint 방향으로 힘을 가함
            rb.AddForce(directionToMiddle * forceStrength, ForceMode.Impulse);
        }
    }
}
