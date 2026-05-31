using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TextMeshProUGUI text;
    private Vector2 normalPos;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        normalPos = text.rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData e) => 
    text.rectTransform.offsetMin = new Vector2(text.rectTransform.offsetMin.x, 2);

    public void OnPointerUp(PointerEventData e)
    {
        text.rectTransform.offsetMin = new Vector2(text.rectTransform.offsetMin.x, 7);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame() => Application.Quit();

    public void NewGame()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.data = new GameData();
            SaveManager.Instance.hasLoadedData = false;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void Continue()
    {
        SaveManager.Instance.LoadData();
    }
}