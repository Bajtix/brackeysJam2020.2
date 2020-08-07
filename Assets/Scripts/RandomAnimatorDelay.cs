using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimatorDelay : MonoBehaviour
{
    public Vector2 randomBetween;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.enabled = false;
        LeanTween.delayedCall(Random.Range(randomBetween.x, randomBetween.y),()=>{
            animator.enabled = true;
        });
    }
}
