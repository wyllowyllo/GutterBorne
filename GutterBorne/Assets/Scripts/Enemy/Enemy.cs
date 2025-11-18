using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("스탯")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _stopDistance = 0.5f; // 플레이어와의 최소 간격 (너무 붙지 않도록)

    [Header("넉백 시간")]
    [SerializeField] private float _knockbackDuration = 0.15f;

    [Header(("사망 vfx"))]
    [SerializeField] private GameObject _deathVFX;
    
    private SpriteRenderer _renderer;
    private Animator _animator;

    private Rigidbody2D _rigid;
    private Transform _player;

    private int _currentHealth;
   
    private bool _isActive = false; // 방 트리거 전까지는 비활성
    private bool _isDead = false;

    // 넉백 관련
    private bool _isKnockback = false;
    private Coroutine _knockbackRoutine = null;

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
        if (!_isActive) return;

        if (_isDead)
        {
            _rigid.linearVelocity = Vector2.zero;
            return;
        }
       

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        if (_isKnockback) return;

        Vector2 curPos = transform.position;
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
        //_rigid.MovePosition(nextPos);
        transform.position = nextPos;
      
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
        else
        {
            _animator.SetTrigger("hit");
        }
       
       
    }

    public void Knockback(Vector2 dir, float power)
    {
        if (_isDead) return;

       
        if (_knockbackRoutine != null)
        {
            StopCoroutine(_knockbackRoutine);
        }
        _knockbackRoutine = StartCoroutine(KnockbackRoutine(dir, power));
    }

    private IEnumerator KnockbackRoutine(Vector2 dir, float power)
    {
        _isKnockback = true;

        // 방향 정규화 후 속도 적용
        Vector2 knockDir = dir.normalized;
        _rigid.linearVelocity = knockDir * power;

        float elapsed = 0f;

        while (elapsed < _knockbackDuration)
        {
            elapsed += Time.deltaTime;
            // 필요하면 여기서 서서히 감쇠도 가능
            yield return null;
        }

        // 넉백 끝
        _rigid.linearVelocity = Vector2.zero;
        _isKnockback = false;
        _knockbackRoutine = null;
    }

    private void Die()
    {
        _isDead = true;
        _isActive = false;

        Instantiate(_deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
       /* if (_animator != null)
            _animator.SetTrigger("die");*/

    }

    
}
