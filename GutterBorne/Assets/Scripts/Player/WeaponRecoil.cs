using System.Collections;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [SerializeField] private float _kickbackDistance = 0.2f; // 얼마나 뒤로 밀릴지
    [SerializeField] private float _kickbackTime = 0.05f;    // 뒤로 가는 시간
    [SerializeField] private float _returnTime = 0.08f;      // 원위치로 돌아오는 시간

    private Vector3 _defaultLocalPos;
    private Coroutine _recoilCoroutine;

    private void Awake()
    {
        _defaultLocalPos = transform.localPosition;
    }

    public void PlayRecoil(Vector2 shotDir)
    {
        if (_recoilCoroutine != null)
            StopCoroutine(_recoilCoroutine);

        _recoilCoroutine = StartCoroutine(RecoilRoutine(shotDir));
    }

    private IEnumerator RecoilRoutine(Vector2 shotDir)
    {
        // 총 방향의 반대 방향으로 킥백
        Vector3 kickDir = - (Vector3)shotDir.normalized;
        Vector3 kickTarget = _defaultLocalPos + kickDir * _kickbackDistance;

        float t = 0f;
        // 뒤로 밀림
        while (t < _kickbackTime)
        {
            t += Time.deltaTime;
            float lerp = t / _kickbackTime;
            transform.localPosition = Vector3.Lerp(_defaultLocalPos, kickTarget, lerp);
            yield return null;
        }

        // 다시 원위치로 복귀
        t = 0f;
        while (t < _returnTime)
        {
            t += Time.deltaTime;
            float lerp = t / _returnTime;
            transform.localPosition = Vector3.Lerp(kickTarget, _defaultLocalPos, lerp);
            yield return null;
        }

        transform.localPosition = _defaultLocalPos;
        _recoilCoroutine = null;
    }
}
