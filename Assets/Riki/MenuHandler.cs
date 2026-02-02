using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    public void StartGame()
    {
        StartCoroutine(Intro());
    }
    
    private IEnumerator Intro()
    {
        image.color = Color.black;
        startButton.SetActive(false);
        yield return new WaitForSeconds(1);
        text.enabled = true;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
