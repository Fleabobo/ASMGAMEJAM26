using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public Light flashLight;
    public float flashDelay = 0.05f;      // seconds to wait before the flash appears
    public float flashDuration = 0.05f;
    public GameObject flashParticle;   // optional, leave empty if not using one

    public void Flash()
    {
        StopAllCoroutines(); // in case of rapid fire, restart cleanly
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        if (flashDelay > 0f)
            yield return new WaitForSeconds(flashDelay);

        if (flashLight != null)
            flashLight.enabled = true;

        if (flashParticle != null)
            flashParticle.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        if (flashLight != null)
            flashLight.enabled = false;

        if (flashParticle != null)
            flashParticle.SetActive(false);
    }
}