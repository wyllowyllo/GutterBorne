using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBody : MonoBehaviour
{
    public UnityEvent OnDeathEvent = new UnityEvent();
    
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damageForFrame = 3f;
    private Animator _animator;
    private Rigidbody2D _rigid;
    
    private bool _isDead;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
    }

    private void Die()
    {
        _isDead = true;
        OnDeathEvent.Invoke();
        
        _animator.SetTrigger("Death");
        
       
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy") || _isDead) return;
        
        _health -= Time.deltaTime * _damageForFrame;

        if (_health <= 0)
        {
            Die();
        }
    }
}
