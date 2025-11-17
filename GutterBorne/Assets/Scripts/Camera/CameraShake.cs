using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float _defaultDuration = 0.1f;
    [SerializeField] private float _defaultMagnitude = 0.2f;

    private Vector3 _originCamPos;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
      
        _originCamPos = transform.localPosition;
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        if (duration <= 0f) duration = _defaultDuration;
        if (magnitude <= 0f) magnitude = _defaultMagnitude;

        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            transform.localPosition = _originCamPos;
        }

        _shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = _originCamPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _originCamPos;
        _shakeCoroutine = null;
    }
}
