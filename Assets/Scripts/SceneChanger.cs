using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator transition;
    public float transitionTime = 1f;
    public bool saveOnTransition = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transition.Play("FadeToBlack");
            StartCoroutine(DelayFade());
            if (saveOnTransition)
            {
                SaveManager.Instance.SaveData();
            }
        }
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
 