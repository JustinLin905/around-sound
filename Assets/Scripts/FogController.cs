using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
    public AudioSource universalAudiosource;
    public AudioClip fogSound;
    private float targetFogDensity;

    private void Start() {
        RenderSettings.fog = false;
    }

    private void Update()
    {
        if (RenderSettings.fog) {
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensity, Time.deltaTime);
        }
    }

    IEnumerator RollFog(float density)
    {
        RenderSettings.fog = true;
        targetFogDensity = density;
        universalAudiosource.PlayOneShot(fogSound);

        yield return new WaitForSeconds(2.5f);

        targetFogDensity = 0f;

        yield return new WaitForSeconds(2.0f);

        RenderSettings.fog = false;
    }

    public void ShowFog(float density)
    {
        StartCoroutine(RollFog(density));
    }
}
