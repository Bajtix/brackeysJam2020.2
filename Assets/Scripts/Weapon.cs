using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Weapon : MonoBehaviour
{
    public float Damage;
    public float RestTime;
    public Animator animator;
    public string triggerAttack;

    private float attackDelay;

    public Transform holdR;
    public Transform holdL;
    
    public void Attack()
    {
        if (attackDelay <= 0)
        {
            animator.SetTrigger(triggerAttack);
            attackDelay = RestTime;
        }
    }

    private void Update()
    {
        if(attackDelay > 0)
            attackDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Player.instance.HP -= Damage;
    }
}
