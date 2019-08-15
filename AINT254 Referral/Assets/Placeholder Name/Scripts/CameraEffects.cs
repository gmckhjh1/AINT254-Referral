using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private Material m_materialBlur;
    [SerializeField] private GameObject m_eyelids;
    [SerializeField] Animator m_blink;
    private bool effectTriggered = false;

    /// <summary>
    /// Public call to shake coroutine. 
    /// Passes required variables.
    /// </summary>
    /// <param name="_time"></param>
    /// <param name="m_magnitude"></param>
    public void InititiateEffects(float _time, float m_magnitude)
    {
        if (!effectTriggered)
        {
            StartCoroutine(Effects(_time, m_magnitude));//Start camera shake  
            effectTriggered = true;
        }
        else return;
    }

    /// <summary>
    /// Stores original cam posiiton. 
    /// Shakes cam through a random range by input magnitude.
    /// Reset cam to original position when time has elapsed.
    /// </summary>
    /// <param name="_time"></param>
    /// <param name="_magnitude"></param>
    /// <returns></returns>
    private IEnumerator Effects(float _time, float _magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        float shakeTime = _time * 0.2f;

        //Move camera by random range based on provided magnitude
        //until percentage of time has elapsed.
        while (elapsed < shakeTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * _magnitude;

            elapsed += Time.deltaTime;
        }
        transform.position = originalPosition;//Move camera back to normal position

        //Continue the blur effects until full time has elapsed
        while (elapsed < _time)
        {
            elapsed += Time.deltaTime;
            yield return 0;
        }

        if (m_blink)
        {
            Blink();

            yield return new WaitForSeconds(.3f);
        }   
        Player.Instance.CamSwitchNormal();//Switch back to normal view

        effectTriggered = false;

        if (m_blink)
        {
            yield return new WaitForSeconds(.2f);
            StopBlink();
        }

    }

    /// <summary>
    /// Apply render texture to camera
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, m_materialBlur);
    }

    private void Blink()
    {
        m_eyelids.SetActive(true);
        m_blink.enabled = true;
    }

    private void StopBlink()
    {
        m_eyelids.SetActive(false);
        m_blink.enabled = false;
    }
}
