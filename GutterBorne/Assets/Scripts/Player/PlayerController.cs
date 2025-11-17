using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _playerHand;

    private SpriteRenderer _renderer;
    private Rigidbody2D _rigid;
    private Animator _animator;

    private Camera _camera;
    
    private float moveHorizontal;
    private float moveVertical;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        
        _camera = Camera.main;
    }

    private void Start()
    {
        _health = _maxHealth;
    }

    private void Update()
    {
       GetInput();
       Move();
      
    }

    private void LateUpdate()
    {
        Turn();
    }
    

    private void GetInput()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (moveHorizontal >= 0) _renderer.flipX = false;
        else _renderer.flipX = true;
    }
    private void Move()
    {
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;
        Vector2 curPosition = transform.position;
        Vector2 nextPosition = curPosition + direction * _moveSpeed * Time.deltaTime;
        
        transform.position = nextPosition;
        _animator.SetFloat("moveSpeed", direction.magnitude);
        _rigid.linearVelocity = Vector2.zero;
    }

    private void Turn()
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPos - transform.position;

        // 오른쪽 
        bool facingRight = dir.x >= 0f;

        _renderer.flipX = !facingRight;
        
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
    }
}
