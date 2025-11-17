using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private int _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _playerHand;

    private Rigidbody2D _rigid;
    private Animator _animator;

    private float moveHorizontal;
    private float moveVertical;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
       GetInput();
       Move();
    }

    private void GetInput()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }
    private void Move()
    {
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;
        Vector2 curPosition = transform.position;
        Vector2 nextPosition = curPosition + direction * _moveSpeed * Time.deltaTime;
        
        transform.position = nextPosition;
        _animator.SetFloat("moveSpeed", direction.magnitude);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
    }
}
