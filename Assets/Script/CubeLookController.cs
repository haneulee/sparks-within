using UnityEngine;
using System.Collections.Generic;

public class CubeLookController : MonoBehaviour
{
    public Transform playerCamera;
    public List<Look> cubes;
    public float lookThreshold = 0.98f; // 약 10도 이내
    public float requiredLookTime = 3f;

    private Look currentTarget;
    private float lookTimer = 0f;

    void Update()
    {
        Look bestCandidate = null;
        float bestDot = -1f;

        // 모든 큐브 중 카메라 중심과 가장 일치하는 큐브 찾기
        foreach (var cube in cubes)
        {
            Vector3 dirToCube = (cube.transform.position - playerCamera.position).normalized;
            float dot = Vector3.Dot(playerCamera.forward, dirToCube);

            if (dot > lookThreshold && dot > bestDot)
            {
                bestDot = dot;
                bestCandidate = cube;
            }
        }

        // 현재 바라보는 큐브가 이전과 같으면 타이머 증가
        if (bestCandidate == currentTarget)
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= requiredLookTime && !currentTarget.effectTriggered)
            {
                currentTarget.TriggerRingEffect();
                currentTarget.effectTriggered = true;
            }

        }
        else
        {
            currentTarget = bestCandidate;
            lookTimer = 0f;
        }
    }
}
