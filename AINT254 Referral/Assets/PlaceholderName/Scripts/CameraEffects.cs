using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{

    [SerializeField] private Shader blur;
    [SerializeField] Material materialBlur;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    /// <summary>
    /// Public call to shake coroutine. 
    /// Passes required variables.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="magnitude"></param>
    public void InititiateEffects(float time, float magnitude)
    {
        SetRenderer();//Enable blur effect
        StartCoroutine(Shake(time, magnitude));//Start camera shake      
    }

    /// <summary>
    /// Stores original cam posiiton. 
    /// Shakes cam through a random range by input magnitude.
    /// Reset cam to original position when time has elapsed.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    private IEnumerator Shake(float time, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < time)
        {           
            transform.position = originalPosition + Random.insideUnitSphere * magnitude;

            elapsed += Time.deltaTime;
            yield return 0;
        }

        transform.position = originalPosition;
        //Call to disable renderer blur effect
        SetRenderer();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, materialBlur);
    }

    private void SetRenderer()
    {
        if(mesh.enabled == true)
        {
            mesh.enabled = false;
        }
        else
        {
            mesh.enabled = true;
        }
    } 
}
