using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Systems")]
    public FlashlightSystem flashlightSystem;
    public CameraSystem cameraSystem;

    [Header("Databases")]
    public MusicDatabase musicDatabase;

    private void Awake()
    {
        // Evita duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}