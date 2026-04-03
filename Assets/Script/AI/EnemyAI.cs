using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// IA simple : patrouille entre des waypoints, poursuit le joueur si détecté.
/// Après MaxChaseDuration secondes de poursuite, l'ennemi s'épuise et reprend sa patrouille.
/// Pilote l'Animator avec les paramètres Speed (float) et Attack (trigger).
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Patrouille")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Détection")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Attaque")]
    [SerializeField] private float attackRange = 1.5f;

    [Header("Épuisement")]
    [SerializeField] private float maxChaseDuration = 5f;
    [SerializeField] private float exhaustionDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioClip zombieIdleSound;
    [SerializeField] private AudioClip zombieAlertSound;
    [SerializeField] private float idleSoundInterval = 2f;
    [SerializeField][Range(0f, 1f)] private float zombieSFXVolume = 0.8f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private int currentWaypointIndex = 0;

    private const float StartDelay = 1.5f;
    private float elapsedTime = 0f;
    private float chaseTimer = 0f;
    private float exhaustionTimer = 0f;
    private float idleSoundTimer = 0f;
    private bool isExhausted = false;
    private bool isAttacking = false;
    private bool isChasing = false;

    private int _speedParam;
    private int _attackParam;

    private void Awake()
    {
        _speedParam = Animator.StringToHash("Speed");
        _attackParam = Animator.StringToHash("Attack");

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        // Source audio 3D spatiale propre à chaque zombie
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;   // full 3D
        audioSource.maxDistance = 20f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.playOnAwake = false;
        audioSource.volume = zombieSFXVolume;
    }

    private void Update()
    {
        if (!agent.isOnNavMesh) return;

        elapsedTime += Time.deltaTime;
        if (elapsedTime < StartDelay) return;

        if (isExhausted)
        {
            Recover();
            animator.SetFloat(_speedParam, agent.velocity.magnitude);
            TickIdleSound();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            TriggerAttack();
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            Chase();
        }
        else if (!isAttacking)
        {
            ResetChase();
            TickIdleSound();
        }

        if (!isAttacking)
            animator.SetFloat(_speedParam, agent.velocity.magnitude);
    }

    /// <summary>Joue le grognement d'idle toutes les idleSoundInterval secondes.</summary>
    private void TickIdleSound()
    {
        idleSoundTimer += Time.deltaTime;
        if (idleSoundTimer >= idleSoundInterval)
        {
            idleSoundTimer = 0f;
            if (zombieIdleSound != null)
                audioSource.PlayOneShot(zombieIdleSound);
        }
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

    /// <summary>Poursuit le joueur. Déclenche le son d'alerte à la première détection.</summary>
    private void Chase()
    {
        if (!isChasing)
        {
            isChasing = true;
            idleSoundTimer = 0f;
            if (zombieAlertSound != null)
                audioSource.PlayOneShot(zombieAlertSound);
        }

        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        chaseTimer += Time.deltaTime;

        if (chaseTimer >= maxChaseDuration)
        {
            isExhausted = true;
            chaseTimer = 0f;
            exhaustionTimer = 0f;
            agent.SetDestination(transform.position);
        }
    }

    /// <summary>Déclenche l'animation d'attaque et stoppe l'agent temporairement.</summary>
    private void TriggerAttack()
    {
        isAttacking = true;
        agent.ResetPath();
        animator.SetFloat(_speedParam, 0f);
        animator.SetTrigger(_attackParam);

        Invoke(nameof(EndAttack), GetAttackClipLength());
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private float GetAttackClipLength()
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Contains("Attack") || clip.name.Contains("Melee"))
                return clip.length;
        }
        return 1f;
    }

    /// <summary>Récupération : l'ennemi attend puis reprend sa patrouille.</summary>
    private void Recover()
    {
        exhaustionTimer += Time.deltaTime;
        if (exhaustionTimer >= exhaustionDuration)
            isExhausted = false;

        Patrol();
    }

    /// <summary>Remet les timers à zéro quand le joueur échappe.</summary>
    private void ResetChase()
    {
        if (isChasing)
            isChasing = false;

        chaseTimer = 0f;
        Patrol();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            MiniGamesManager.Instance.OnPlayerCaught();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
