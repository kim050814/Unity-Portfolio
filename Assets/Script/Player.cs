using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    Animator animator;
    Vector2 dir;


    bool IsCollisionEnter;

    [SerializeField] LayerMask EnemyLayer;
    [Header("플레이어 스탯")]
    [SerializeField] float Speed;
    [SerializeField] float Attack;
    public float Hp;
    [SerializeField] float MaxHp;

    bool FirstJump = false;
    bool IsJump = false;

    [SerializeField] Image HpFill_Img;

    Vector3 PlayerScale;
    void Start()
    {
        Hp = MaxHp;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        PlayerScale = transform.localScale;
        
    }

    

    void Update()
    {
        Move();
        Jump();
        AttackCheck();
        CollisionCheck();

        HpFill_Img.fillAmount = Hp / MaxHp;
    }

    void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");

        PlayerScale.x = hor != 0 ? hor * 1 : PlayerScale.x;
        transform.localScale = PlayerScale;

        animator.SetFloat("Hor", Mathf.Abs(hor));
        dir.x = hor * Speed;
    }




    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJump == false)
        {
            FirstJump = true;
            IsJump = true;
        }

        animator.SetBool("Jump", FirstJump);
    }

    bool GetAniState(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }




    void AttackCheck()
    {
        if (Input.GetMouseButtonDown(0) && GetAniState("Attack") == false)
        {
            //animator.ResetTrigger("Attack");
            //animator.SetTrigger("Attack");
            animator.Play("Attack");
        }
    }

    void CollisionCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector2.zero, 10, EnemyLayer);
        if (hit && IsCollisionEnter == false)
        {

            IsCollisionEnter = true;
            animator.SetTrigger("Hit");
        }
        else if(hit == false)
        {
            IsCollisionEnter = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1.5f);
    }


    public void HitPlayAni()
    {
        animator.SetTrigger("Hit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Background"))
        {
            IsJump = false;
        }
    }


    private void FixedUpdate()
    {
        dir.y = rigidbody.velocity.y;
        if (FirstJump)
        {
            FirstJump = false;
            dir.y = 5;
        }
        rigidbody.velocity = dir;

    }
}
