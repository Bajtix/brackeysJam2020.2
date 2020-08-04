using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyBehaviour : MonoBehaviour
{
    public Vector2 distanceFromPlayer;
    public float activationDelay;
    
    public Animator animator;

    private NavMeshAgent agent;
    public Weapon weapon;

    public Transform handTargetR;
    public Transform handTargetL;

    public float RWEIGHT,LWEIGHT;

    public TwoBoneIKConstraint iKConstraintR;
    public TwoBoneIKConstraint iKConstraintL;

    public GameObject deathEffect;

    public float stunTime;

    public int hp;

    public TimeEntity timeEntity;

    private bool runToPlayer = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        iKConstraintR.weight = 0.2f;
        iKConstraintL.weight = 0.2f;
        weapon.gameObject.SetActive(false);
    }

    private void Update()
    {
        activationDelay -= Time.deltaTime;

        if (TimeController.instance.playbackMode)
            hp = int.Parse(timeEntity.metadata);
        else
        {
            timeEntity.metadata = hp.ToString();
            if (activationDelay <= 0)
            {
                weapon.gameObject.SetActive(true);
                DoAI();
                handTargetR.position = weapon.holdR.position;
                handTargetR.rotation = weapon.holdR.rotation;
                iKConstraintR.weight = RWEIGHT;

                handTargetL.position = weapon.holdL.position;
                handTargetL.rotation = weapon.holdL.rotation;
                iKConstraintL.weight = LWEIGHT;
            }
        }


    }

    public void DoAI()
    {
        if (runToPlayer)
        {
            if (Vector3.Distance(transform.position, Player.instance.transform.position) >= distanceFromPlayer.y)
            {
                agent.isStopped = false;
                agent.SetDestination(Player.instance.transform.position);


            }
            else if (Vector3.Distance(transform.position, Player.instance.transform.position) < distanceFromPlayer.x)
            {
                agent.isStopped = true;
            }
            else
            {
                weapon.Attack();
            }
        }

        if (agent.isStopped)
            animator.SetBool("Walking", false);
        else
            animator.SetBool("Walking", true);
    }

    public void Hit()
    {
        animator.SetTrigger("Fall");
        //Stun();
        hp--;
        if (hp <= 0)
            Die();
    }

    public void Stun()
    {
        activationDelay = stunTime;
    }

    public void BackOut()
    {
        agent.updateRotation = false;
        runToPlayer = false;
        agent.SetDestination(transform.position + (transform.position - Player.instance.transform.position) * 2);
        LeanTween.delayedCall(1, () => { runToPlayer = true; agent.updateRotation = true; });
    }

    public void Die()
    {
        weapon.gameObject.SetActive(false);
        activationDelay = stunTime * 1.5f;
        animator.SetTrigger("Die");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        LeanTween.delayedCall(stunTime,()=>gameObject.SetActive(false));
    }

}
