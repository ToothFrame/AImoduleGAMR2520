using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioSource audioBegin;
    public AudioSource audioChomp;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AudioPlay());
    }

IEnumerator AudioPlay()
    {
        audioBegin.Play();
        while (audioBegin.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);

        audioChomp.Play();
    }
}
