using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RopeEscapeTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject rope;
    public Animator helicopterAnimator;
    public string leaveTriggerName = "Leave";
    public GameObject playerCamera;
    public GameObject cutsceneCamera;

    [Header("Credits")]
    public string creditsSceneName = "Credits";
    public float delayBeforeCredits = 5f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            TriggerEscape();
        }
    }

    void TriggerEscape()
    {
        if (rope != null)
        {
            rope.SetActive(false);
        }

        if (helicopterAnimator != null)
        {
            helicopterAnimator.SetTrigger(leaveTriggerName);
        }
        playerCamera.SetActive(false);
        cutsceneCamera.SetActive(true);

        // Run the coroutine on the cutscene camera instead of this object,
        // since this object (rope) is about to be/already inactive.
        CreditsLoader.Instance.LoadCreditsAfterDelay(creditsSceneName, delayBeforeCredits);
    }
}