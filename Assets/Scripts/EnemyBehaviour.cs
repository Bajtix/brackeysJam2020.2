using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    public Vector2 distanceFromPlayer;
    public float activationDelay;
    
    public Animator animator;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        activationDelay -= Time.deltaTime;

        if(activationDelay <= 0)
        {
            DoAI();
        }
    }

    public void DoAI()
    {
        if(Vector3.Distance(transform.position,Player.instance.transform.position) >= distanceFromPlayer.y)
        {
            agent.isStopped = false;
            agent.SetDestination(Player.instance.transform.position);

            
        }
        else if (Vector3.Distance(transform.position, Player.instance.transform.position) < distanceFromPlayer.x)
        {
            agent.isStopped = true;
        }

        if (agent.isStopped)
            animator.SetBool("Walking", false);
        else
            animator.SetBool("Walking", true);
    }

    public void Hit()
    {

    }

    public void Die()
    {

    }

}
