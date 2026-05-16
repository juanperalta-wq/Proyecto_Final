using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraWeapon : MonoBehaviour
{
    [Header("Camera Settings")]
    public float cameraRange = 10f;
    public float stunDuration = 3f;

    [Header("Photo System")]
    public int maxPhotos = 5;
    public int currentPhotos;

    [Header("Cooldown")]
    public float cooldown = 2f;

    private bool canShoot = true;

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
        currentPhotos = maxPhotos;
    }

    void Update()
    {
        bool photoPressed = inputActions.Player.TakePhoto.triggered;

        if (photoPressed && canShoot)
        {
            TakePhoto();
        }
    }

    void TakePhoto()
    {
        if (currentPhotos <= 0)
        {
            Debug.Log("NO PHOTOS LEFT!");
            return;
        }

        currentPhotos--;

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, cameraRange))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.StunEnemy(stunDuration);

                Debug.Log("ENEMY STUNNED!");
            }
        }

        StartCoroutine(PhotoCooldown());
    }

    IEnumerator PhotoCooldown()
    {
        canShoot = false;

        yield return new WaitForSeconds(cooldown);

        canShoot = true;
    }
}