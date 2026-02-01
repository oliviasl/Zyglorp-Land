using UnityEngine;
using UnityEngine.UI;

public class ShowHide : MonoBehaviour
{
    public static ShowHide instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Show(GameObject canvas)
    {
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("THIS OBJECT DOES NOT HAVE A CANVAS GROUP COMPONENT, WILL NOT SHOW PROPERLY");
        }

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void Hide(GameObject canvas)
    {
        CanvasGroup cg = canvas.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("THIS OBJECT DOES NOT HAVE A CANVAS GROUP COMPONENT, WILL NOT HIDE PROPERLY");
        }

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
