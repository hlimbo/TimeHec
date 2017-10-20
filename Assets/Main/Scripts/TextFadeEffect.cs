using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

//generic class used to manipulate the fading effect of various UI text objects
public class TextFadeEffect : MonoBehaviour {

    //All durations / delays are measured in seconds
    public float fadeInDuration;
    public float displayDelay;
    public float fadeOutDuration;
    [Tooltip("If ticked, fade effect ignores Unity's time scale")]
    public bool ignoreTimeScale;
    [SerializeField]
    private float effectDuration;

    private Text gText;
    //invisible text ~ http://answers.unity3d.com/questions/881620/uigraphiccrossfadealpha-only-works-for-decrementin.html
    private const float invisibleAlpha = 0.00392156862f;

    void Awake()
    {
        effectDuration = fadeInDuration + displayDelay + fadeOutDuration;
        gText = GetComponent<Text>();
        Assert.IsTrue(gText != null, gameObject.name + " does not have a Text Component attached");
        Assert.IsTrue(displayDelay != 0.0f, "Warning: " + gameObject.name + " display delay must be a NON-zero value");
        //invisible text
        gText.color = new Color(gText.color.r, gText.color.g, gText.color.b, invisibleAlpha);
    }

    void Start()
    {
        //useful if we want text to apply the fade effect when the game is paused.
        if (ignoreTimeScale == true)
            StartCoroutine(FadeEffect_RealTime());
        else
            StartCoroutine(FadeEffect_ScaledTime());
    }

    IEnumerator FadeEffect_RealTime()
    {
        //alpha values range from [1,255]
        gText.CrossFadeAlpha(255f, fadeInDuration, ignoreTimeScale);
        yield return new WaitForSecondsRealtime(displayDelay);
        gText.CrossFadeAlpha(1.0f, fadeOutDuration, ignoreTimeScale);
        yield return null;
    }

    IEnumerator FadeEffect_ScaledTime()
    {
        gText.CrossFadeAlpha(255f, fadeInDuration, ignoreTimeScale);
        yield return new WaitForSeconds(displayDelay);
        gText.CrossFadeAlpha(1.0f, fadeOutDuration, ignoreTimeScale);
        yield return null;
    }
}
