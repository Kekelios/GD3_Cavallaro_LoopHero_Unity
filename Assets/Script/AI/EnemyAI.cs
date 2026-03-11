using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// IA simple : patrouille entre des waypoints, poursuit le joueur si détecté.
/// Après MaxChaseDuration secondes de poursuite, l'ennemi s'épuise et reprend sa patrouille.
/// À placer sur le GameObject "Enemy" avec un NavMeshAgent et un Collider.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Patrouille")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Détection")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Épuisement")]
    [SerializeField] private float maxChaseDuration = 5f;   // Durée max de poursuite en secondes
    [SerializeField] private float exhaustionDuration = 3f; // Durée de récupération avant de reprendre

    private NavMeshAgent agent;
    private Transform player;
    private int currentWaypointIndex = 0;

    private const float StartDelay = 1.5f;
    private float elapsedTime = 0f;

    // Durée de poursuite en cours
    private float chaseTimer = 0f;

    // Durée depuis que l'ennemi est épuisé
    private float exhaustionTimer = 0f;

    // True si l'ennemi est épuisé et ne peut pas poursuivre
    private bool isExhausted = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (!agent.isOnNavMesh) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime < StartDelay) return;

        // Si épuisé, on attend la fin de la récupération
        if (isExhausted)
        {
            Recover();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
            Chase();
        else
            ResetChase();
    }

    /// <summary>Patrouille entre les waypoints dans l'ordre.</summary>
    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    /// <summary>Poursuit le joueur. Déclenche l'épuisement après maxChaseDuration secondes.</summary>
    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        chaseTimer += Time.deltaTime;

        if (chaseTimer >= maxChaseDuration)
        {
            // L'ennemi s'épuise et s'arrête sur place
            isExhausted = true;
            chaseTimer = 0f;
            exhaustionTimer = 0f;
            agent.SetDestination(transform.position);
        }
    }

    /// <summary>Récupération : l'ennemi attend puis reprend sa patrouille.</summary>
    private void Recover()
    {
        exhaustionTimer += Time.deltaTime;

        if (exhaustionTimer >= exhaustionDuration)
            isExhausted = false;

        Patrol();
    }

    /// <summary>Remet le timer de poursuite à zéro quand le joueur échappe.</summary>
    private void ResetChase()
    {
        chaseTimer = 0f;
        Patrol();
    }

    /// <summary>Si l'ennemi touche le joueur, il est attrapé.</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MiniGamesManager.Instance.OnPlayerCaught();
        }
    }

    // Dessine la zone de détection dans l'éditeur
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
