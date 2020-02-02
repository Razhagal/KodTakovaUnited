using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class StartGameController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public Image imgBackgroundStars;
    public BetweenAlpha imgBackgroundBetweenAlpha;

    public Button btnStart;
    public Button btnQuit;

    public BetweenTransform betweenTransform;
    public BetweenAlpha betweenAlpha;

    public BetweenTransform betweenTransformAnimatedPlanet;
    public BetweenAlpha betweenAlphaAnimatedPlanet;

    public void OnClickStartGame()
    {
        videoPlayer.Prepare();

        btnStart.gameObject.SetActive(false);
        btnQuit.gameObject.SetActive(false);

        betweenTransform.ResetToBeginning();
        betweenTransform.PlayForward();

        betweenAlpha.ResetToBeginning();
        betweenAlpha.PlayForward();

        betweenTransformAnimatedPlanet.gameObject.SetActive(true);
        betweenTransformAnimatedPlanet.ResetToBeginning();
        betweenTransformAnimatedPlanet.PlayForward();

        betweenAlphaAnimatedPlanet.ResetToBeginning();
        betweenAlphaAnimatedPlanet.PlayForward();
    }

    public void OnBetweenTransformFinish()
    {
        StartCoroutine(WaitAndRun());
    }

    public IEnumerator WaitAndRun()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(0.2f);

        imgBackgroundBetweenAlpha.gameObject.SetActive(false);
        betweenTransformAnimatedPlanet.gameObject.SetActive(false);
        yield return new WaitForSeconds(4.5f);

        SceneManager.LoadScene("Main");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
