using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator transition;
    public float transitionTime = 1f;
    public string spawnPointName = "";
    public GameObject bossIntroCanvas;
    public TextMeshProUGUI bossIntroText;
    public GameObject creditsCanvas;
    public TextMeshProUGUI creditsText;
    

    public bool isBossIntro = false;
    public bool isCredits = false;

    private void Start()
    {
        if (isBossIntro) bossIntroCanvas.SetActive(false);
        if (isCredits) creditsCanvas.SetActive(false);
    }

    private string[] introLines = new string[]
    {
        "After a long journey through the cursed forest...",
        "The knight arrives at the ruined battlefield...",
        "A fallen knight awaits in the silence..."
    };

    

    private string[] creditsLines = new string[]
    {
        
        "Game Design & Programming",
        "José Luis Blanco",

        "",
        "Assets",
        "Pixel Frog (itch.io)",

        "",
        "Music",
        "bobjt (OpenGameArt.org)",
        "The Cynic Project / Pixelsphere (cynicmusic.com)",
        "Kistol (OpenGameArt.org)",
        "Symphony (SoundCloud / OpenGameArt.org)",

        "",
        "Sound Effects",
        "cabled_mess (Freesound.org)",

        "",
        "Thank you for playing"
    };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerMovement>().enabled = false;
            transition.Play("FadeToBlack");
            SaveManager.spawnPointName = spawnPointName;
            StartCoroutine(DelayFade());
        }
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(transitionTime);

        if (isBossIntro)
        {
            yield return StartCoroutine(PlayBossIntro());
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator PlayBossIntro()
    {
        bossIntroCanvas.SetActive(true);

        foreach (string line in introLines)
        {
            bossIntroText.text = line;
            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(1f);

        bossIntroCanvas.SetActive(false);
    }

    public void StartEndGameSequence()
    {
        StartCoroutine(EndGameSequence());
    }

    IEnumerator EndGameSequence()
    {
        MusicManager.Instance.PlayMusic("Victory");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
        }
        transition.Play("FadeToBlack");

        yield return new WaitForSeconds(transitionTime);

        if (isCredits)
        {
            yield return StartCoroutine(PlayCredits());
        }

        SaveManager.Instance.DeleteSaveData();

        SceneManager.LoadScene("Menu");
    }

    IEnumerator PlayCredits()
    {
        creditsCanvas.SetActive(true);    
        creditsText.gameObject.SetActive(true);

        foreach (string line in creditsLines)
        {
            creditsText.text = line;
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(1f);

        creditsCanvas.SetActive(false);
    }
}
 