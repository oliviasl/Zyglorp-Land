using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using StarterAssets;

public class HelmetHandler : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator maskAnim;
    [SerializeField] private string triggerToPutOn;
    [SerializeField] private string triggerToPutOff;
    [SerializeField] private GameObject audioSourceObj;

    [Header("Audio JBL")]
    [SerializeField] private Transform closeTransform;
    [SerializeField] private Transform farTransform;
    

    [SerializeField] private GameObject helmetUICanvas;
    private ShowHide handler;

    private FirstPersonController controller;
    private PlayerInput _playerInput;
    private bool _bIsHelmetOn;

    // camera stuff
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _cameraRoot;
    readonly float rotationTime = 0.5f;
    float degrees;
    private bool cameraRotating = false;
    public bool abduction = false;

    [SerializeField] private Animator _bombAnim;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput != null)
        {
            InputAction helmetAction = _playerInput.actions.FindAction("MoveHelmet");
            helmetAction.performed += ToggleHelmetUI;
        }
    }

    private void Start()
    {
        handler = ShowHide.instance;

        _bIsHelmetOn = true;
        handler.Show(helmetUICanvas);

        controller = FindFirstObjectByType<FirstPersonController>(FindObjectsInactive.Include);
    }

    private void ToggleHelmetUI(InputAction.CallbackContext obj)
    {
        if (cameraRotating) { return; }
        if(abduction)  { return; }
        
        _bIsHelmetOn = !_bIsHelmetOn;

        // camera stuff
        if (_bIsHelmetOn) //taking mask on
        {
            controller.EnableMovement(true);
            controller.EnableLook(true);
            Cursor.lockState = CursorLockMode.Locked;
            handler.Show(helmetUICanvas);

            StartCoroutine(RotateBack());
            _bombAnim.SetBool("up", false);

            audioSourceObj.transform.position = farTransform.position;

            Debug.Log("helmet on");

            if (!AudioManager.instance.bombMusic.isPlaying)
            {
                Debug.Log("no music playing");
                if (AlienManager.Instance.state == AlienManager.ManagerState.Chase)
                {
                    //play low with manager chasing
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.lowBombMangMusic;
                    AudioManager.instance.bombMusic.Play();
                }
                else
                {
                    //play just low music
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.lowBombMusic;
                    AudioManager.instance.bombMusic.Play();
                }
                
            }
            else if(AlienManager.Instance.state == AlienManager.ManagerState.Chase)
            {
                Debug.Log("chasing");
                if (AudioManager.instance.bombMusic.clip == AudioManager.instance.highBombMangMusic)
                {
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.lowBombMangMusic;
                    AudioManager.instance.bombMusic.Play();
                }
            }
            else
            {
                Debug.Log("manager regular");

                if (AudioManager.instance.bombMusic.clip == AudioManager.instance.highBombMusic)
                {
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.lowBombMusic;
                    Debug.Log("Playing low music");
                    AudioManager.instance.bombMusic.Play();
                }
             
            }
            // do this for all the states, low, low mang, high, high mang

            //maskAnim.SetTrigger(triggerToPutOn);

        }
        else //putting mask off
        {
            Debug.Log("helmet off");
            controller.EnableMovement(false);
            controller.EnableLook(false);
            Cursor.lockState = CursorLockMode.None;
            handler.Hide(helmetUICanvas);

            StartCoroutine(RotateToBomb());
            _bombAnim.SetBool("up", true);
            maskAnim.SetTrigger(triggerToPutOff);

            audioSourceObj.transform.position = closeTransform.position;

            if (!AudioManager.instance.bombMusic.isPlaying)
            {
                Debug.Log("no music");
                if (AlienManager.Instance.state == AlienManager.ManagerState.Chase)
                {
                    //play low with manager chasing
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.highBombMangMusic;
                    AudioManager.instance.bombMusic.Play();
                }
                else
                {
                    //play just high music
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.highBombMusic;
                    AudioManager.instance.bombMusic.Play();
                }

            }
            else if (AlienManager.Instance.state == AlienManager.ManagerState.Chase)
            {
                Debug.Log("chasing");
                if (AudioManager.instance.bombMusic.clip == AudioManager.instance.lowBombMangMusic)
                {
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.highBombMangMusic;
                    AudioManager.instance.bombMusic.Play();
                }
            }
            else
            {
                Debug.Log("regular patrol");
                if (AudioManager.instance.bombMusic.clip == AudioManager.instance.lowBombMusic)
                {
                    AudioManager.instance.bombMusic.clip = AudioManager.instance.highBombMusic;
                    Debug.Log("Playing high music");
                    AudioManager.instance.bombMusic.Play();
                }
            }


        }
    }

    public void HideMaskScreen()
    {
        handler.Hide(helmetUICanvas);
        _bombAnim.SetBool("up", false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowMaskScreen()
    {
        controller.EnableMovement(true);
        controller.EnableLook(true);
        Cursor.lockState = CursorLockMode.Locked;
        handler.Show(helmetUICanvas);

        StartCoroutine(RotateBack());
        _bombAnim.SetBool("up", false);
    }

    private IEnumerator RotateToBomb()
    {
        cameraRotating = true;
        float totalDegrees = 0;
        float targetDegrees = 25f;
        if (_cameraRoot.transform.localEulerAngles.x > 180)
        {
            degrees = targetDegrees - (_cameraRoot.transform.localEulerAngles.x - 360);
        }
        else
        {
            degrees = targetDegrees - _cameraRoot.transform.localEulerAngles.x;
        }
        Debug.Log("trying to rotate from: " + _cameraRoot.transform.localEulerAngles.x + " with degrees: " + degrees);

        while (Mathf.Abs(totalDegrees) < Mathf.Abs(degrees))
        {
            float rotateStep = degrees * Time.deltaTime / rotationTime;
            totalDegrees += rotateStep;
            _cameraRoot.transform.localEulerAngles += new Vector3(rotateStep, 0, 0);
            yield return null;
        }

        Vector3 angles = _cameraRoot.transform.localEulerAngles;
        _cameraRoot.transform.localEulerAngles = new Vector3(targetDegrees, angles.y, angles.z);
        cameraRotating = false;
    }

    private IEnumerator RotateBack()
    {
        cameraRotating = true;
        float totalDegrees = 0;

        while (Mathf.Abs(totalDegrees) < Mathf.Abs(degrees))
        {
            float rotateStep = degrees * Time.deltaTime / rotationTime;
            totalDegrees += rotateStep;
            _cameraRoot.transform.localEulerAngles -= new Vector3(rotateStep, 0, 0);
            yield return null;
        }

        cameraRotating = false;
    }

    public bool GetIsHelmetOn()
    {
        return _bIsHelmetOn;
    }
}
