using UnityEngine;

public class AnimationController : MonoBehaviour
{   
    private Animator animator;
    //para utilizar o animator e o animation
    private Rigidbody2D rb;
    //para verificar se esta parado ou se movendo
    private Vector2 direction;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = rb.linearVelocity.normalized;

        if (rb.linearVelocity == Vector2.up || direction == Vector2.down)
        //se estiver se movendo pra cima ou pra baixo
        {
            animator.SetBool("IsWalking", true);
        }
        else
        //se estiver parado
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsWalkingRight", false);
            animator.SetBool("IsWalkingLeft", false);
            animator.SetBool("Idle", true);
        }

        if (direction == Vector2.right){
            //se estiver se movendo pra direita
            animator.SetBool("IsWalkingRight", true);
            animator.SetBool("IsWalkingLeft", false);
        }
        if (direction == Vector2.left){
            //se estiver se movendo pra esquerda
            animator.SetBool("IsWalkingLeft", true);
            animator.SetBool("IsWalkingRight", false);
        }
    }
}
