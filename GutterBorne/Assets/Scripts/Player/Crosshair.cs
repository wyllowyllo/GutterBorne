using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("최대 사거리 설정")]
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _maxDistance = 5f; 
    
    private Camera _camera;

    private void Awake()
    {
       Init();
    }

    private void Init()
    {
        _camera = Camera.main;
        
        // 시스템 커서 숨기기 + 게임 창 안에 가두기
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        if (_camera == null) return;
        
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;
        
        Vector3 centerPos = _muzzle.position;
        Vector3 distance = mouseWorldPos - centerPos;
        
        
        // 사거리 제한 
        float sqrMax = _maxDistance * _maxDistance;
        if (distance.sqrMagnitude > sqrMax)
        {
            distance = distance.normalized * _maxDistance;
        }

        // 4. 크로스헤어 위치 = 플레이어 위치 + (제한된 오프셋)
        transform.position = centerPos + distance;
    }
}
