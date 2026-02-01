using System.Collections;
using TMPro;
using UnityEngine;

public class StartingPrompts : MonoBehaviour
{
    [SerializeField] private GameObject tutorialParentObj;
    [SerializeField] private GameObject taskTutorial;
    [SerializeField] private GameObject maskTutorial;
    [SerializeField] private GameObject shoveTutorial;

    [SerializeField] private GameObject timerObject;
    [SerializeField] private Collider thisCollider;


    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(WaitToShowTut());
            thisCollider.enabled = false;
        }
    }

    IEnumerator WaitToShowTut()
    {
        taskTutorial.SetActive(true);
        yield return new WaitForSeconds(8f);
        taskTutorial.SetActive(false);
        maskTutorial.SetActive(true);
        yield return new WaitForSeconds(15f);
        maskTutorial.SetActive(false);
        shoveTutorial.SetActive(true);
        yield return new WaitForSeconds(5f);
        tutorialParentObj.SetActive(false);
        timerObject.SetActive(true);
        TaskManager.instance.StartTimer();

    }
}
