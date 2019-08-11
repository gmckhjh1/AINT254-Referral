using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] Material materialBlur;
    [SerializeField] GameObject eyelids;
    [SerializeField] Animator blink;
    private bool effectTriggered = false;

    /// <summary>
    /// Public call to shake coroutine. 
    /// Passes required variables.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="magnitude"></param>
    public void InititiateEffects(float time, float magnitude)
    {
        if (!effectTriggered)
        {
            StartCoroutine(Effects(time, magnitude));//Start camera shake  
            effectTriggered = true;
        }
        else return;
    }

    /// <summary>
    /// Stores original cam posiiton. 
    /// Shakes cam through a random range by input magnitude.
    /// Reset cam to original position when time has elapsed.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    private IEnumerator Effects(float time, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;
        float shakeTime = time * 0.2f;

        //Move camera by random range based on provided magnitude
        //until percentage of time has elapsed.
        while (elapsed < shakeTime)
        {
            transform.position = originalPosition + Random.insideUnitSphere * magnitude;

            elapsed += Time.deltaTime;
        }
        transform.position = originalPosition;//Move camera back to normal position

        //Continue the blur effects until full time has elapsed
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            yield return 0;
        }

        Blink();
        yield return new WaitForSeconds(.3f);

        Player.Instance.currState = Player.PlayerStates.Normal;//Set player state back to normal
        Player.Instance.CamSwitchNormal();//Switch back to normal view

        effectTriggered = false;

        yield return new WaitForSeconds(.2f);
        StopBlink();

    }

    /// <summary>
    /// Apply render texture to camera
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, materialBlur);
    }

    private void Blink()
    {
        eyelids.SetActive(true);
        blink.enabled = true;
    }

    private void StopBlink()
    {
        eyelids.SetActive(false);
        blink.enabled = false;
    }
}
