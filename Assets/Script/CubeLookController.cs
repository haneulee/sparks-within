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

        foreach (var cube in cubes)
        {
            if (cube == null) continue;

            Vector3 dirToCube = (cube.transform.position - playerCamera.position).normalized;
            float dot = Vector3.Dot(playerCamera.forward, dirToCube);

            if (dot > lookThreshold && dot > bestDot)
            {
                bestDot = dot;
                bestCandidate = cube;
            }
        }

        if (bestCandidate == currentTarget)
        {
            if (currentTarget != null)
            {
                lookTimer += Time.deltaTime;
                if (lookTimer >= requiredLookTime && !currentTarget.effectTriggered)
                {
                    currentTarget.TriggerRingEffect();
                    currentTarget.effectTriggered = true;
                }
            }
        }
        else
        {
            currentTarget = bestCandidate;
            lookTimer = 0f;
        }
    }

}
