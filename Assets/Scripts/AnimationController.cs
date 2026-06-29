using UnityEngine;

public class AnimationController : MonoBehaviour
{   
    private Animator animator;
    //para utilizar o animator e o animation
    private Rigidbody2D rb;
    //para verificar se esta parado ou se movendo
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        //se estiver se movendo
        {
            animator.SetBool("IsWalking", true);
        }
        else
        //se estiver parado
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("Idle", true);
        }
    }
}
