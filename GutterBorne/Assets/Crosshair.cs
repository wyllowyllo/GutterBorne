using UnityEngine;

public class Crosshair : MonoBehaviour
{
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
        
        transform.position = mouseWorldPos;
    }
}
