using System;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    
    private Camera _camera;
    private float _currentAngle;
    SpriteRenderer _renderer;
    
    private PlayerBody _playerBody;
    private bool _isDead;
    private void Awake()
    {
        _camera = Camera.main;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _playerBody = GetComponentInParent<PlayerBody>();
        
    }

    private void Start()
    {
        _playerBody.OnDeathEvent.AddListener(PlayerDeath);
    }

    private void Update()
    {
        if (_isDead) return;
        
        AimToMouse();
       
    }

    private void LateUpdate()
    {
        HandleFlip();
    }

    private void AimToMouse()
    {
        
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 dir = mouseWorldPos - transform.position;
        dir.z = 0f; // 2D 평면에서만 회전

        // 방향 벡터 → 각도
        _currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 해당 각도로 Z축 회전
        transform.rotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
    }
    
    private void HandleFlip()
    {
        // 오른쪽을 보고 있을 때(-90~90)
        bool facingRight = _currentAngle > -90f && _currentAngle <= 90f;

        // 캐릭터 본체 스프라이트
        _renderer.flipY = !facingRight;

    }

    private void PlayerDeath()
    {
        _isDead = true;
    }
}
