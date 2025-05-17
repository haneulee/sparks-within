using UnityEngine;

public class RingMover : MonoBehaviour
{
    private Transform target;    // 플레이어 카메라
    private float speed = 1f;
    private float destroyDistance = 0.2f;  // 사라지는 거리

    public void Initialize(Transform targetTransform, float moveSpeed)
    {
        target = targetTransform;
        speed = moveSpeed;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // 타겟(카메라)과 충분히 가까워지면 제거
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < destroyDistance)
        {
            Destroy(gameObject);  // 링 제거
        }
    }
}
