using UnityEngine;

public class CamearFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;  // 따라갈 대상 (플레이어)
    [SerializeField] private float _smoothSpeed = 10f; // 부드럽게 따라가는 정도

    private void LateUpdate()
    {
        if (_target == null) return;

        // 현재 카메라 위치
        Vector3 currentPos = transform.position;

        // 목표 위치 (플레이어 위치 + z는 카메라 고정)
        Vector3 targetPos = new Vector3(
            _target.position.x,
            _target.position.y,
            currentPos.z
        );

        // Lerp로 부드럽게 이동
        transform.position = Vector3.Lerp(currentPos, targetPos, _smoothSpeed * Time.deltaTime);
    }
}
