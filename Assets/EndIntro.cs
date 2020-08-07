using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EndIntro : MonoBehaviour
{
    public VideoPlayer p1, p2;
    public GameObject panel;
    public int nextScene = 2;
    private void Start()
    {
        panel.SetActive(true);
        p1.Play();
        p2.Play();
        AsyncOperation o = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
        o.allowSceneActivation = false;
        LeanTween.delayedCall(3, () =>
        {
            p1.Stop();
            p2.Stop();
            o.allowSceneActivation = true;
        });
    }
}
