using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// IA simple : patrouille entre des waypoints, poursuit le joueur si détecté.
/// À placer sur le GameObject "Enemy" avec un NavMeshAgent et un Collider.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Patrouille")]
    [SerializeField] private Transform[] waypoints;   // Points de patrouille
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Détection")]
    [SerializeField] private float detectionRange = 8f;   // Distance de détection
    [SerializeField] private float chaseSpeed = 4f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentWaypointIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Trouve le joueur par son tag
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
            Chase();
        else
            Patrol();
    }

    /// <summary>Patrouille entre les waypoints dans l'ordre.</summary>
    private void Patrol()
    {
        agent.speed = patrolSpeed;

        // Si l'ennemi est arrivé au waypoint, on passe au suivant
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    /// <summary>Poursuit le joueur.</summary>
    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    /// <summary>Si l'ennemi touche le joueur, il est attrapé.</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MiniGamesManager.Instance.OnPlayerCaught();
        }
    }

    // Dessine la zone de détection dans l'éditeur (pratique pour régler la valeur)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
