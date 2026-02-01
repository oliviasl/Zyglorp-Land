using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using StarterAssets;

public class HelmetHandler : MonoBehaviour
{
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

        _bIsHelmetOn = !_bIsHelmetOn;

        // camera stuff
        if (_bIsHelmetOn)
        {
            controller.EnableMovement(true);
            controller.EnableLook(true);
            Cursor.lockState = CursorLockMode.Locked;
            handler.Show(helmetUICanvas);

            StartCoroutine(RotateBack());
            _bombAnim.SetBool("up", false);
        }
        else
        {
            controller.EnableMovement(false);
            controller.EnableLook(false);
            Cursor.lockState = CursorLockMode.None;
            handler.Hide(helmetUICanvas);

            StartCoroutine(RotateToBomb());
            _bombAnim.SetBool("up", true);
        }
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
