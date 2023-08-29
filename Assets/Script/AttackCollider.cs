using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyHit"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            enemy.rigidbody.velocity = (Vector2.up + (player.transform.position.x -
                collision.transform.position.x >= 0 ? Vector2.left : Vector2.right)) * 3;
            enemy.IsKnockback = true;
            enemy.animator.ResetTrigger("Walk");
            enemy.animator.SetTrigger("Hit");
            enemy.Hp --;

            if (enemy.Hp <= 0)
                enemy.animator.Play("Death");
        }
        
    }
}
