using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] persistents; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MakePersistents();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            CleanUpAndDestroy();
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (instance != this) return;
        if (scene.name == "Menu" || scene.buildIndex == 0)
        {
             CleanUpAndDestroy();
        }
    }
    private void MakePersistents()
    {
        foreach (GameObject obj in persistents)
        {
            if(obj != null) DontDestroyOnLoad(obj);
        }
    }

    public void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistents)
        {
            if (obj != null) Destroy(obj);
        }
        Destroy(gameObject);
    }
}
