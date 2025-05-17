using UnityEngine;

public class floatingobject : MonoBehaviour
{
    public float speed = 1.0f;
    public float changeDirectionTime = 2.0f;
    public float movementRadius = 2.0f;     // 플레이어 중심 반경
    public float verticalOffset = 0.3f;     // 눈높이 위아래 허용 범위
    public Transform playerCamera;          // 플레이어 참조

    public bool isFrozen = false;

    private Vector3 targetDirection;
    private float timer;

    void Start()
    {
        PickNewDirection();
    }

    void Update()
    {
        if (isFrozen || playerCamera == null) return;

        Vector3 newPos = transform.position + targetDirection * speed * Time.deltaTime;

        float targetY = Mathf.Clamp(newPos.y, playerCamera.position.y - verticalOffset, playerCamera.position.y + verticalOffset);

        transform.position = new Vector3(newPos.x, targetY, newPos.z);  // 반경 제한 제거

        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            PickNewDirection();
            timer = 0f;
        }
    }


    void PickNewDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-0.3f, 0.3f);  // 적은 Y 변경량 (실제로는 제한됨)
        float z = Random.Range(-1f, 1f);
        targetDirection = new Vector3(x, y, z).normalized;
    }
}
