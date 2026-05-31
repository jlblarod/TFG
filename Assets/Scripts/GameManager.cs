using UnityEngine;

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
        }
        else
        {
            CleanUpAndDestroy();
            return;
        }
    }

    private void MakePersistents()
    {
        foreach (GameObject obj in persistents)
        {
            if(obj != null) DontDestroyOnLoad(obj);
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistents)
        {
            if (obj != null) Destroy(obj);
        }
        Destroy(gameObject);
    }
}
