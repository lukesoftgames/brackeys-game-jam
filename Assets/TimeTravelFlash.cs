using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelFlash : MonoBehaviour
{
    CanvasGroup cv;
    public float flashtime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        cv = GetComponent<CanvasGroup>();
        GameEvents.current.onTimerEnd += Flash;
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < flashtime)
        {
            cv.alpha = t / flashtime;
            t += Time.deltaTime;
            yield return null;
        }
        GameEvents.current.Flash();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        while (t < flashtime)
        {
            cv.alpha = 1 - (t / flashtime);
            t += Time.deltaTime;
            yield return null;
        }
    }

    void Flash()
    {
        StartCoroutine(FadeIn());    
    }
}
