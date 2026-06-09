using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public string trackName;

    void Start()
    {
        MusicManager.Instance.PlayMusic(trackName);
    }
}