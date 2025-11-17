using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fire : MonoBehaviour
{
    [Header("샷건 성능 변수")]
    [SerializeField] private int _pelletCount = 8;
    [SerializeField] private float _shotRange = 5f;
    [SerializeField] private float _spreadAngle = 15f;
    [SerializeField] private float _shotDamage = 10f;
    [SerializeField] private float _knockbackForce = 5f;

    [Header("샷건 오브젝트 참조")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Animator _fireAnim;
    
    [Header("VFX")]
    [SerializeField] private GameObject hitEffectPrefab;   //  히트 이펙트 프리팹

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
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
        _fireAnim.SetTrigger("Shot");
        _weaponRecoil.PlayRecoil(aimDir);
        CameraShake.Instance.Shake(0.08f, 0.15f); // 카메라 흔들기
    }

    private void FirePellet(Vector2 baseDirection)
    {
        float randomAngle = Random.Range(-_spreadAngle, _spreadAngle);
        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * baseDirection;

        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, dir, _shotRange);

        if (hit.collider != null)
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
