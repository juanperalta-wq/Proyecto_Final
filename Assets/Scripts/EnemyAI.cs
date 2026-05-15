using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Movement")]
    public float normalSpeed = 5f;
    public float slowedSpeed = 2f;

    [Header("States")]
    public bool isStunned;

    private NavMeshAgent agent;

    private Coroutine slowCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = normalSpeed;
    }

    void Update()
    {
        if (player == null) return;

        // Si está stun
        if (isStunned)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;

        // Perseguir jugador
        agent.SetDestination(player.position);
    }

    // LINTERNA
    public void SlowEnemy(float duration)
    {
        if (isStunned) return;

        agent.speed = slowedSpeed;

        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }

        slowCoroutine = StartCoroutine(ResetSpeed(duration));
    }

    IEnumerator ResetSpeed(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (!isStunned)
        {
            agent.speed = normalSpeed;
        }
    }

    // CÁMARA
    public void StunEnemy(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;

        agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        isStunned = false;

        agent.isStopped = false;

        agent.speed = normalSpeed;
    }
}