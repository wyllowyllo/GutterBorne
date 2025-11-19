using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 속도")]
    [SerializeField] private float _moveSpeed;
    
    [Header("대쉬")]
    [SerializeField] private float _dashSpeed = 26f;
    [SerializeField] private float _dashDuration = 0.10f;
    [SerializeField] private float _dashCoolTime = 0.8f;
    
    [Header("Hand")]
    [SerializeField] private GameObject _playerHand;

    // 플레이어 컴포넌트
    private PlayerBody _playerBody;
    private SpriteRenderer _renderer;
    private Rigidbody2D _rigid;
    private Animator _animator;

    // 카메라
    private Camera _camera;
    
    // 입력
    private float _moveHorizontal;
    private float _moveVertical;
    private bool _dashInput;
    
   
    // 플래그 변수
    private bool _isDashing;
    private bool _isDead;

    private float _dashTimer;
    private void Awake()
    {
        _playerBody = GetComponent<PlayerBody>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        
        _camera = Camera.main;
    }

    private void Start()
    {
        _playerBody.OnDeathEvent.AddListener(PlayerDeath);
    }

    private void Update()
    {
        if (_isDead) return;
        
        
        Timer();
        
       GetInput();
       Dash();
       Move();
      
    }

    private void LateUpdate()
    {
        if (_isDead) return;
        
        Turn();
    }
    

    private void GetInput()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
        _dashInput = Input.GetKeyDown(KeyCode.Space);
       
        
        if (_moveHorizontal >= 0) _renderer.flipX = false;
        else _renderer.flipX = true;
    }
    private void Move()
    {
        if (_isDashing) return;
        
        Vector2 _moveDirection = new Vector2(_moveHorizontal, _moveVertical).normalized;
        Vector2 curPosition = transform.position;
        Vector2 nextPosition = curPosition + _moveDirection * _moveSpeed * Time.deltaTime;
        
        transform.position = nextPosition;
        _animator.SetFloat("moveSpeed", _moveDirection.magnitude);
        _rigid.linearVelocity = Vector2.zero;
    }

    private void Timer()
    {
        _dashTimer += Time.deltaTime;
    }

    private void Dash()
    {
        if (!_dashInput) return;
        if (_isDashing) return;
        if (_dashTimer < _dashCoolTime) return;
        
        Vector2 _dashDirection = new Vector2(_moveHorizontal, _moveVertical).normalized;
        if (_dashDirection != Vector2.zero)
        {
            StartCoroutine(DashRoutine(_dashDirection));
        }
    }

    private void Turn()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPos - transform.position;

        // 오른쪽 
        bool facingRight = dir.x >= 0f;

        _renderer.flipX = !facingRight;
        
    }

    private IEnumerator DashRoutine(Vector2 dashDirection)
    {
        _isDashing = true;
        int originalLayer = gameObject.layer;
        Color originalColor = _renderer.color;
        
      
        gameObject.layer = LayerMask.NameToLayer("PlayerDash");
        _animator.SetTrigger("Dash");
        
        float timer = 0f;

     
        while (timer < _dashDuration)
        {
            _rigid.linearVelocity = dashDirection * _dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

      
        gameObject.layer = originalLayer;
        
        // 대시 종료
        _isDashing = false;
    }

    private void PlayerDeath()
    {
        _isDead = true;
        _rigid.linearVelocity = Vector2.zero;
    }

    
}
