using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class vignetteDegats : MonoBehaviour
{
    public AnimationCurve courbeIntensite;
    public PostProcessProfile ppProfile;

    public float targetTime = 1.0f;
    
    public float startTime = .0f;

    Vignette vignette = null;
    float elapsedTime = .0f;

    // Start is called before the first frame update
    void Start()
    {
        ppProfile.TryGetSettings(out vignette);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void launchTakeHit()
    {
        StartCoroutine("TakeHit");
    }

    IEnumerator TakeHit()
    {
        startTime = Time.time;
        while (elapsedTime <= 1.0f)
        {
            vignette.intensity.value = courbeIntensite.Evaluate(elapsedTime);
            elapsedTime = Time.time - startTime;
            yield return new WaitForSeconds(.01f);
        }
        elapsedTime = .0f;
    }
}
