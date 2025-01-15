using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    public float runRange = 10f;

    HeroKnight player;
    Rigidbody2D rb;
    BossScript boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<HeroKnight>();
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossScript>();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.m_body2d.position, rb.position) <= runRange)
        {
            animator.SetTrigger("Run");
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
    }
}
