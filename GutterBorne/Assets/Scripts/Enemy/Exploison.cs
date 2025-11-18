using Unity.Cinemachine;
using UnityEngine;

public class Exploison : MonoBehaviour
{
    private CinemachineImpulseSource _impulse;

    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        // 파티클이 재생될 때 한 번만 흔들기
        if (_ps != null && _impulse != null)
        {
            _impulse.GenerateImpulse();
        }
    }
}
