using System;
using UnityEngine;

public class Aim : MonoBehaviour
{
    
    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
        
    }

    private void Update()
    {
        AimToMouse();
    }
    
    private void AimToMouse()
    {
      
        // 1. 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);

        // 2. 플레이어 → 마우스 방향 벡터
        Vector3 dir = mouseWorldPos - transform.position;
        dir.z = 0f; // 2D 평면에서만 회전

        // 3. 방향 벡터 → 각도(도 단위)로 변환
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 4. 해당 각도로 Z축 회전
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
