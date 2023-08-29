using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] Player player;
    [SerializeField] LayerMask PlayerLayer;
    public Rigidbody2D rigidbody;
    public Animator animator;
    SpriteRenderer renderer;

    Vector2 dir;
    public bool IsKnockback;


    public int Hp;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Attack());

        Hp = 10;
    }



    public void AttackHitCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.zero, 10, PlayerLayer);
        if(hit)
        {
            player.Hp--;
            player.HitPlayAni();
        }
    }

    public void ObjectDisable()
    {
        gameObject.SetActive(false);
    }
    bool GetAniState(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    IEnumerator Attack()
    {
        while(true)
        {
            yield return new WaitForSeconds(4.0f);
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Walk");
            animator.SetTrigger("Attack");
        }
    }

    //private void OnDrawGizmos()
    //{
     //   Gizmos.DrawSphere(transform.position, 1.5f);
    //}

    
    private void FixedUpdate()
    {
        if (IsKnockback == false && GetAniState("Attack") == false)
        {
            if(Vector2.Distance(player.rigidbody.position, rigidbody.position) > 1.0f &&
                GetAniState("Death") == false)
            {
                dir.x = player.rigidbody.position.x - rigidbody.position.x >= 0 ? 1 : -1;
                renderer.flipX = dir.x > 0 ? false : true;
                animator.SetTrigger("Walk");
            }
            else
            {
                dir.x = 0;
                animator.SetTrigger("Idle");
            }
            
            dir.y = rigidbody.velocity.y;
            rigidbody.velocity = dir;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.HitPlayAni();
        }
        if(collision.gameObject.CompareTag("Background"))
        {
            IsKnockback = false;
        }
    }
    
}
