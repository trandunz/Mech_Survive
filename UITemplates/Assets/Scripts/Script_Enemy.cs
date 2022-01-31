using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_Enemy : MonoBehaviour
{
    enum AIState
    {
        IDLE,
        WANDER,
        SEEK
    }

    AIState state;

    NavMeshAgent agent;
    Transform player;
    ParticleSystem deathParticle;

    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;

    public float MovementSpeed;
    private float timer;
    float DistanceToPlayer;

    bool m_isActive = true;
    bool m_isDead = false;

    [SerializeField] float m_Health;
    public float m_MaxHealth = 100;

    IEnumerator OnDeathCoroutine()
    {
        m_isActive = false;
        state = AIState.IDLE;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        deathParticle.Play();
        m_isDead = true;
        yield return new WaitUntil(() => deathParticle.isPlaying == false);
    }

    private void Start()
    {
        deathParticle = GetComponentInChildren<ParticleSystem>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = AIState.IDLE;
    }
    private void Awake()
    {
        m_Health = m_MaxHealth;
    }
    private void Update()
    {
        if (m_Health <= 0 && !m_isDead)
        {
            StartCoroutine(OnDeathCoroutine());
        }

        if (m_isActive)
        {
            CalculateDistanceToPlayer();
            if (DistanceToPlayer <= wanderRadius)
            {
                state = AIState.SEEK;
            }
            else if (DistanceToPlayer > (2 * wanderRadius))
            {
                state = AIState.IDLE;
            }
            else if (DistanceToPlayer > wanderRadius)
            {
                state = AIState.WANDER;
            }
        }

    }
    private void LateUpdate()
    {
        if (state == AIState.WANDER)
        {
            agent.speed = 1.5f * MovementSpeed;
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }
        else if (state == AIState.SEEK)
        {
            agent.speed = MovementSpeed;
            agent.SetDestination(player.position);
        }
        else if (state == AIState.IDLE)
        {
            agent.ResetPath();
        }

        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    void CalculateDistanceToPlayer()
    {
        DistanceToPlayer = Vector3.Magnitude(transform.position - player.position);
    }

    public void TakeDamage(float _amount)
    {
        for (float i = _amount; i > 0; i--)
        {
            if (m_Health >= 0)
            {
                m_Health--;
            }
            else
            {
                break;
            }
        }
    }
    public void Heal(float _amount)
    {
        for (float i = 0; i < _amount; i++)
        {
            if (m_Health < 100)
            {
                m_Health++;
            }
            else
            {
                break;
            }
        }
    }
    
}
