using UnityEngine;
using UnityEngine.Events;

public class Sight : MonoBehaviour
{
    // 감지 범위 및 시야각 설정
    [Header("Sensor Settings")]
    public float detectionRadius = 10f;     // 감지 거리
    public float viewAngle = 90f;           // 시야각
    public LayerMask targetLayer;           // 감지할 타겟의 레이어 마스크
    public LayerMask obstacleLayer;         // 장애물 레이어 마스크 (폐색 필터)

    [Tooltip("감지할 방향을 설정합니다. 기본값은 전방입니다.")]
    public Vector3 detectionDirection = Vector3.forward;

    [Header("Events")]
    public UnityEvent<Collider> OnTargetEnter;  // 타겟이 감지 범위에 들어왔을 때 발생
    public UnityEvent<Collider> OnTargetStay;   // 타겟이 감지 범위 내에 있을 때 발생
    public UnityEvent<Collider> OnTargetExit;   // 타겟이 감지 범위를 벗어날 때 발생

    private Collider detectedTarget;            // 현재 감지된 타겟
    private Collider previousTarget;            // 이전에 감지된 타겟 (Exit 이벤트 처리용)

    private void Update()
    {
        DetectTargets();
    }

    void DetectTargets()
    {
        // 감지 거리 내의 모든 Collider를 가져옴 (OverlapSphere 사용)
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);
        detectedTarget = null;  // 감지된 타겟을 초기화

        // 감지된 모든 타겟을 순회하며 시야각과 폐색 필터 적용
        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            
            // 감지 방향을 오브젝트의 회전 방향에 따라 설정
            Vector3 detectionDir = transform.rotation * detectionDirection;
            float angleToTarget = Vector3.Angle(detectionDir.normalized, directionToTarget);

            // 시야각 내에 타겟이 있는지 확인
            if (angleToTarget < viewAngle / 2)
            {
                // Linecast로 장애물이 없는지 확인
                if (!Physics.Linecast(transform.position, target.transform.position, obstacleLayer))
                {
                    detectedTarget = target;  // 타겟 감지

                    if (previousTarget == null)  // 새로운 타겟이 감지된 경우 (Enter)
                    {
                        OnTargetEnter.Invoke(detectedTarget);
                    }
                    else  // 타겟이 계속 감지되고 있는 경우 (Stay)
                    {
                        OnTargetStay.Invoke(detectedTarget);
                    }
                    break;
                }
            }
        }

        // 감지 범위에서 벗어날 경우 (Exit)
        if (previousTarget != null && detectedTarget == null)
        {
            OnTargetExit.Invoke(previousTarget);
        }

        // 현재 타겟을 이전 타겟으로 업데이트
        previousTarget = detectedTarget;
    }

    // Gizmos를 사용해 시각적으로 감지 영역 및 시야각 표시
    private void OnDrawGizmos()
    {
        // 감지 반경 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // 오브젝트의 회전에 따라 시야각 방향을 조정
        Vector3 forwardDirection = transform.rotation * detectionDirection.normalized;

        // 시야각 표시 (오른쪽 방향)
        Vector3 rightDirection = Quaternion.Euler(0, viewAngle / 2, 0) * forwardDirection;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rightDirection * detectionRadius);

        // 시야각 표시 (왼쪽 방향)
        Vector3 leftDirection = Quaternion.Euler(0, -viewAngle / 2, 0) * forwardDirection;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftDirection * detectionRadius);
    }
}