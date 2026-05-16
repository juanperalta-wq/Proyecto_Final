using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight")]
    public Light flashlight;
    public float flashlightRange = 15f;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float currentBattery;
    public float batteryDrain = 10f;

    [Header("Enemy Slow")]
    public float slowResetTime = 0.2f;

    private bool isOn;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        currentBattery = maxBattery;

        if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }

    void Update()
    {
        HandleFlashlight();
        DetectEnemy();
    }

    void HandleFlashlight()
    {
        bool flashlightPressed = inputActions.Player.Flashlight.IsPressed();

        if (flashlightPressed && currentBattery > 0)
        {
            isOn = true;

            flashlight.enabled = true;

            currentBattery -= batteryDrain * Time.deltaTime;
        }
        else
        {
            isOn = false;

            flashlight.enabled = false;
        }

        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
    }

    void DetectEnemy()
    {
        if (!isOn) return;

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, flashlightRange))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.SlowEnemy(slowResetTime);
            }
        }
    }
}