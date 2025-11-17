using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _stopDistance = 0.5f; // 플레이어와의 최소 간격 (너무 붙지 않도록)

    [Header("참조")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rigid;
    private Transform _player;

    private int _currentHealth;
    private bool _isActive = false; // 방 트리거 전까지는 비활성
    private bool _isDead = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
       _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _currentHealth = _maxHealth;
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        _player = playerObj.transform;
       
    }

    private void Update()
    {
        if (!_isActive || _isDead) return;
       

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector2 curPos = _rigid.position;
        Vector2 targetPos = _player.position;
        Vector2 dir = (targetPos - curPos);
        float distance = dir.magnitude;

        if (distance <= _stopDistance)
        {
            // 너무 가까우면 멈춤
            _rigid.linearVelocity = Vector2.zero;
            
            _animator.SetFloat("moveSpeed", 0f);
            return;
        }

        dir = dir.normalized;
        Vector2 nextPos = curPos + dir * _moveSpeed * Time.deltaTime;
        _rigid.MovePosition(nextPos);

      
        _animator.SetFloat("moveSpeed", _moveSpeed);
         _renderer.flipX = (dir.x > 0);
        
    }

    // 방 트리거에서 호출할 메서드
    public void Activate()
    {
        if (_isDead) return;
        _isActive = true;
    }

   

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        Debug.Log("Hit!");

        _currentHealth -= Mathf.RoundToInt(damage);
        if (_currentHealth <= 0)
        {
            Die();
        }

        _animator.SetTrigger("hit");
       
    }

    private void Die()
    {
        _isDead = true;
        _isActive = false;

       /* if (_animator != null)
            _animator.SetTrigger("die");*/

    }

    
}
