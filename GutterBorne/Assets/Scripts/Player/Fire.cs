using System;
using System.Collections;
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

    [Header("탄약 / 재장전")]
    [SerializeField] private int _magazineSize = 6;     // 탄창 크기
    [SerializeField] private float _reloadTime = 2.0f;  // 재장전 시간(초)
    [SerializeField] private ReloaderBar _reloadBar;
    private int _currentAmmo;                           
    private bool _isReloading = false;     
    
    [Header("샷건 오브젝트 참조")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private WeaponRecoil _weaponRecoil;
    [SerializeField] private Animator _fireAnim;
    
    
    [Header("특수 효과")]
    [SerializeField] private float _cameraShakeStrength = 0.5f;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private GameObject hitEffectPrefab;   //  히트 이펙트 프리팹

    Camera cam;

    private float _shotTimer = 0f;

    public int CurrentAmmo => _currentAmmo;

    public int MagazineSize => _magazineSize;
    


    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        _currentAmmo = MagazineSize;
    }

    private void Update()
    {
        _shotTimer += Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
        
        // 수동 재장전 (R 키)
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurrentAmmo < MagazineSize && !_isReloading)
            {
                StartCoroutine(ReloadRoutine());
            }
        }
    }

    private void TryShoot()
    {
        if (_shotTimer < _fireCoolTime || _isReloading)
            return;

        if (CurrentAmmo <= 0)
        {
            StartCoroutine(ReloadRoutine());
            return;
        }
        
        Shoot();
        _shotTimer = 0f;
        
       
    }
    private void Shoot()
    {
        _currentAmmo--;
        
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
        _impulseSource.GenerateImpulse(-aimDir * _cameraShakeStrength); // 사격 반대 방향으로 카메라 흔들기
        
    }

    private void FirePellet(Vector2 baseDirection)
    {
        float randomAngle = Random.Range(-_spreadAngle, _spreadAngle);
        Vector2 dir = Quaternion.Euler(0, 0, randomAngle) * baseDirection;

        int enemyMask = LayerMask.GetMask("Enemy");
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, dir, _shotRange, enemyMask);

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
                enemy.Knockback(dir, _knockbackForce);
            }
        }

        Debug.DrawRay(muzzle.position, dir * _shotRange, Color.red, 0.05f);
    }
    
    private IEnumerator ReloadRoutine()
    {
       
        _isReloading = true;

        // TODO: 재장전 사운드
        
        _reloadBar.Show(); // 재장전 UI 표시

        float elapsed = 0f;
        Debug.Log("Reloading..");
        
        while (elapsed < _reloadTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _reloadTime);

            if (_reloadBar != null)
            {
                _reloadBar.SetProgress(t);
            }

            yield return null;
        }

        Debug.Log("Reloading Complete!");
        
        _reloadBar.Hide();
        
        _currentAmmo = MagazineSize;
        _isReloading = false;
    }
}
