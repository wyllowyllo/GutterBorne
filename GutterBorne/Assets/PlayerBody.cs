using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerBody : MonoBehaviour
{
    public UnityEvent OnDeathEvent = new UnityEvent();
    
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damageForFrame = 3f;
    
    [SerializeField] private Image _healthBarImage;
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

    private void LateUpdate()
    {
        if (_healthBarImage != null)
        {
            _healthBarImage.fillAmount = _health / _maxHealth;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Enemy") || _isDead) return;
        
        _health -= Time.deltaTime * _damageForFrame;
        Debug.Log(_health);
        _animator.SetTrigger("Hit");

        if (_health <= 0)
        {
            Die();
        }
    }
}
