using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public void Awake()
    {
        instance = this;
    }
    public CharacterController charCon;
    public float moveSpeed;
    public float gravityModifier = 4f;

    public InputActionReference moveAction;
    Vector3 currentMovement;
    public InputActionReference lookAction;
    public InputActionReference jumpAction;
    public float jumpPower;
    private Vector2 rotStore;
    public float lookSpeed;

    public InputActionReference shootAction;
    public InputActionReference sprintAction;
    public InputActionReference reloadAction;
    public float runSpeed;
    public Camera theCam;
    public float camZoomNormal,
        camZoomOut,
        camZoomSpeed;
    public float minViewAngle,
        maxViewAngle;
    public WeaponController weaponCon;

    public bool isDead;

    void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        shootAction.action.Enable();
        reloadAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        shootAction.action.Disable();
        reloadAction.action.Disable();
    }

    void Start()
    {
        if (Time.timeScale == 1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {

        if (isDead || Time.timeScale == 0f)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

        float yStore = currentMovement.y;
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 moveForward = transform.forward * moveInput.y;
        Vector3 moveSideways = transform.right * moveInput.x;

        if (sprintAction.action.IsPressed())
        {
            currentMovement = (moveForward + moveSideways) * runSpeed;
            if (currentMovement != Vector3.zero)
            {
                theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, camZoomOut, camZoomSpeed);
            }
        }
        else
        {
            currentMovement = (moveForward + moveSideways) * moveSpeed;
            theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, camZoomNormal, camZoomSpeed);
        }

        if (charCon.isGrounded)
        {
            yStore = 0f;
        }
        currentMovement.y = yStore + (Physics.gravity.y * Time.deltaTime * gravityModifier);

        if (jumpAction.action.WasPressedThisFrame() && charCon.isGrounded)
        {
            currentMovement.y = jumpPower;
        }

        charCon.Move(currentMovement * Time.deltaTime);

        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        lookInput.y = -lookInput.y;
        rotStore = rotStore + (lookInput * lookSpeed * Time.deltaTime);
        rotStore.y = Mathf.Clamp(rotStore.y, minViewAngle, maxViewAngle);
        transform.rotation = Quaternion.Euler(0f, rotStore.x, 0f);
        theCam.transform.localRotation = Quaternion.Euler(rotStore.y, 0f, 0f);

        if (shootAction.action.WasPressedThisFrame())
        {
            weaponCon.Shoot();
        }
        if (shootAction.action.IsPressed())
        {
            weaponCon.ShootHeld();
        }
        if (reloadAction.action.WasPressedThisFrame())
        {
            weaponCon.Reload();
        }
    }
}