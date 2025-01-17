using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Run : StateMachineBehaviour
{
    public float speed = .2f;
    public float attackRange = 5f;
    public float Range = 10f;
    public int AttackComboMax = 3;
    public float runRange = 10f;

    HeroKnight player;
    Rigidbody2D rb;
    BossScript boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<HeroKnight>();
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        boss.LookAtPlayer();

        Vector2 target = new Vector2(player.m_body2d.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        if (Vector2.Distance(player.m_body2d.position, rb.position) <= runRange)
        {
            rb.position = newPos;
        }
        else animator.SetTrigger("Idle");


        if (Vector2.Distance(player.m_body2d.position, rb.position) <= attackRange)
        {
            if (!player.m_isDead)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
