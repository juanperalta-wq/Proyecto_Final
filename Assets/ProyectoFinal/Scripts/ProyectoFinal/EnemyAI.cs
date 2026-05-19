using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private EnemyBase enemyBase;

    private bool canMove = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyBase = GetComponent<EnemyBase>();
    }

    void Update()
    {
        if (canMove)
        {
            agent.SetDestination(player.position);
        }
    }

    public void StopMovement(float duration)
    {
        StartCoroutine(StopCoroutine(duration));
    }

    System.Collections.IEnumerator StopCoroutine(float duration)
    {
        canMove = false;

        agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        agent.isStopped = false;

        canMove = true;
    }
}