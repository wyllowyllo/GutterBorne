using System;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour
{
    [Header("샷건 성능 변수")]
    [SerializeField] private float _shotDamage = 10f; 
    [SerializeField] private float _shotRange = 5f; // 사거리
    [SerializeField] private float _fireCoolTime = 0.4f;
    [SerializeField] private int _pelletCount = 8; // 산탄 개수
    [SerializeField] private float _spreadAngle = 15f; // 산탄 정도
    [SerializeField] private float _knockbackForce = 5f;

    [Header("샷건 오브젝트 참조")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Animator _fireAnim;
    
    
    [Header("특수 효과")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private GameObject hitEffectPrefab;   //  히트 이펙트 프리팹

    Camera cam;

    private float _shotTimer = 0f;
    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        _shotTimer += Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (_shotTimer < _fireCoolTime)
            return;
        
        Shoot();
        _shotTimer = 0f;
    }
    private void Shoot()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = (mousePos - muzzle.position).normalized;

        for (int i = 0; i < _pelletCount; i++)
        {
            FirePellet(aimDir);
        }
        
        // 이펙트 효과 
        // TODO : 사격 사운드 추가하기
        _fireAnim.SetTrigger("Shot");
        _weaponRecoil.PlayRecoil(aimDir);
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulse(-aimDir); // 사격 반대 방향으로 카메라 흔들기
        }
    }

    private void FirePellet(Vector2 baseDirection)
    {
        float randomAngle = Random.Range(-_spreadAngle, _spreadAngle);
        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * baseDirection;

        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, dir, _shotRange);

        if (hit.collider != null && hit.transform.CompareTag("Enemy"))
        {
            // 🔸 히트 이펙트 생성
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
            }

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(_shotDamage);

                Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
                if (rigid)
                {
                    rigid.AddForce(dir * _knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        Debug.DrawRay(muzzle.position, dir * _shotRange, Color.red, 0.05f);
    }
}
