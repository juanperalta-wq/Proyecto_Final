using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class BarneyController : MonoBehaviour
{
    #region VariablesPublicas
    [FoldoutGroup("Variables")]
    public Transform Player;
    [FoldoutGroup("Variables")]
    public float NormalSpeed = 5f;
    [FoldoutGroup("Variables")]
    public float SlowedSpeed = 2f;
    [FoldoutGroup("Variables")]
    public float RangeVision = 10f;
    [FoldoutGroup("Variables")]
    private NavMeshAgent agent;
    [FoldoutGroup("Variables")]
    private Animator anim;
    [FoldoutGroup("Variables")]
    private bool isWalking = false;
    [FoldoutGroup("Variables")]
    public bool point = false;
    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        agent.speed = NormalSpeed;
    }

    void Update()
    {
        Detection();
        Animations();
    }

    public void Animations()
    {
        // Animacion basada en velocidad real
        bool moving = agent.velocity.magnitude > 0.2f;

        if (moving && !isWalking)
        {
            isWalking = true;
            anim.ResetTrigger("Idle");
            anim.SetTrigger("Walk");
        }
        else if (!moving && isWalking)
        {
            isWalking = false;
            anim.ResetTrigger("Walk");
            anim.SetTrigger("Idle");
        }
    }

    public void Detection()
    {
        if (Player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (distanceToPlayer <= RangeVision)
        {
            // Dentro del rango: perseguir
            agent.isStopped = false;
            agent.SetDestination(Player.position);
        }
        else
        {
            // Fuera del rango: dejar que frene solo
            agent.isStopped = false;
            agent.SetDestination(transform.position);
        }
    }
}