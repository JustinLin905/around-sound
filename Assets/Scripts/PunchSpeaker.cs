using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchSpeaker : MonoBehaviour
{
    public AudioClip pop;
    private int controllersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            controllersInside++;
            if (controllersInside >= 2)
            {
                StartCoroutine(DestroySpeaker());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            controllersInside--;
        }
    }

    private IEnumerator DestroySpeaker()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(pop);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
