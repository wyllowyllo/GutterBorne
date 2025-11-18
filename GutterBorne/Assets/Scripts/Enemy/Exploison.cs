using Unity.Cinemachine;
using UnityEngine;

public class Exploison : MonoBehaviour
{
    private AudioSource _audioSource; 
    private CinemachineImpulseSource _impulse;

    private ParticleSystem _ps;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _ps = GetComponent<ParticleSystem>();
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        //사운드 재생
        _audioSource?.Play();
        
        // 파티클이 재생될 때 한 번만 흔들기
        if (_ps != null && _impulse != null)
        {
            _impulse.GenerateImpulse();
        }
    }
}
