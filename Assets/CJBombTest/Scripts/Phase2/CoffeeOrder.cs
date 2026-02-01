using UnityEngine;
using System.Collections;
using TMPro;

public class CoffeeOrder : Puzzle
{
    [SerializeField] float ragebaitTimer = 10f;
    [SerializeField] float appearTime = 0.1f;
    [SerializeField] GameObject phone1;
    [SerializeField] GameObject phone2;
    [SerializeField] GameObject screen1;
    [SerializeField] GameObject screen2;
    [SerializeField] CoffeeWindow[] windows;
    [SerializeField] TMP_Text[] phone1TextBoxes;
    [SerializeField] TMP_Text[] phone2TextBoxes;
    [SerializeField] string correctString;
    bool phone1Active = true;
    bool phone2Active = false;
    string playerString = "";
    int currentScreen = 0;
    Coroutine myCoroutine;

    //size
    //type of coffee
    //ice amount
    //sugar amount
    //syrup
    //type of milk
    //add-ons

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //base.Solve();
        
        Reset();
        phone1Active = true;
        phone2Active = false;
        SetPhones();
        myCoroutine = StartCoroutine(SwitchPhones());
        
    }
    
    public void MakeChoice(string choice)
    {
        playerString += choice;
        BombManager.instance.GetBAM().BeepSFX();
        Debug.Log(playerString);
        currentScreen++;
        SetScreens();
    }

    IEnumerator SwitchPhones()
    {
        while (!base.GetSolved())
        {
            yield return new WaitForSeconds(ragebaitTimer);
            phone1Active = !phone1Active;
            phone2Active = !phone2Active;
            SetPhones();
        }
    }

    public void Continue()
    {
        if (playerString.Equals(correctString))
        {
            base.Solve();
            StopCoroutine(myCoroutine);
        }
        else
        {
            BombManager.instance.GetBAM().FailureSFX();
            Reset();
        }
    }

    public void Reset()
    { 
        playerString = "";
        currentScreen = 0;
        SetScreens();
    }

    void SetPhones()
    {
        phone1.SetActive(false);
        phone2.SetActive(false);

        StartCoroutine(ScreenShrinker());
    }

    IEnumerator ScreenShrinker()
    {
        float scale = 0;
        while (scale < appearTime)
        {
            scale += Time.deltaTime * appearTime;
            float scale1 = Mathf.Clamp01(scale / appearTime);
            float scale2 = Mathf.Clamp01((appearTime - scale) / appearTime);

            if (phone1Active)
            {
                screen1.transform.localScale = new Vector3(scale1, scale1, scale1);
                screen2.transform.localScale = new Vector3(scale2, scale2, scale2);
            }
            else
            {
                screen2.transform.localScale = new Vector3(scale1, scale1, scale1);
                screen1.transform.localScale = new Vector3(scale2, scale2, scale2);
            }
            yield return null;
        }
        phone1.SetActive(phone1Active);
        phone2.SetActive(phone2Active);
    }

    void SetScreens()
    {
        if(currentScreen >= windows.Length)
        {
            Continue();
        }
        else
        {
            phone1TextBoxes[0].text = windows[currentScreen].prompt;
            phone2TextBoxes[0].text = windows[currentScreen].prompt;
            for (int i = 1; i <= windows[currentScreen].responses.Length; i++)
            {
                phone1TextBoxes[i].text = windows[currentScreen].responses[i - 1];
                phone2TextBoxes[i].text = windows[currentScreen].responses[i - 1];
            }
        }
    }
}
