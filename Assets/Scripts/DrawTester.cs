using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DrawTester : MonoBehaviour
{
    public GameObject nextCam;

    void Start()
    {
        CinemachineVirtualCamera cam = nextCam.GetComponent<CinemachineVirtualCamera>();
        var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StartCoroutine(ChangeAmplitudeGain(noise, 1f, 1f));
        StartCoroutine(ChangeFrequencyGain(noise, 10f, 3f));
    }
    private IEnumerator ChangeAmplitudeGain(CinemachineBasicMultiChannelPerlin noise, float targetValue, float duration)
    {
        float startValue = noise.m_AmplitudeGain;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            noise.m_AmplitudeGain = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        noise.m_AmplitudeGain = targetValue; // 최종 값 설정
    }

    private IEnumerator ChangeFrequencyGain(CinemachineBasicMultiChannelPerlin noise, float targetValue, float duration)
    {
        float startValue = noise.m_FrequencyGain;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            noise.m_FrequencyGain = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        noise.m_FrequencyGain = targetValue; // 최종 값 설정
    }
}