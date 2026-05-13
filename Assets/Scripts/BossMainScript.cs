using UnityEngine;
using System.Collections;


public class BossMainScript : MonoBehaviour
{
    private float health = 100f;
    //vida do boss
    private float speed = 3f;
    //velocidade do boss
    private Vector2 direction;
    private SpriteRenderer sr;
    //vou usar pra dar um efeito de dano
    private Rigidbody2D rb;
    //vamos usar o rigidbody aqui pro knockback e pro dash.
    public GameObject player;
    //vai precisar pra ele seguir o player
    public bool collisionPerformed = false;
    //variavel pra saber se ja houve colisao
    public bool dashPerformed = false;
    //variavel pra saber se o dash ja foi realizado
    public bool canDash = false;
    // variavel pra decidir se o boss pode dar o dash
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //pegando o rigidbody e o sprite renderer
    }

    void FixedUpdate()
    {
        if (collisionPerformed == true && dashPerformed == true)
        {
            health -= 10;
            StartCoroutine(OnDamageEffect());
        }
        if (got)
        
        direction = (player.transform.position - transform.position).normalized;
        // atribuimos ao direction o vetor diferença, q é entre a posição do player e a do boss, fazendo um vetor
        // que aponta do boss pro player, o normalized é pra garantir que seja 1 o valor.
        rb.linearVelocity = direction * speed;
        //faz o boss seguir o player
    }

    IEnumerator OnDamageEffect()
    {
        rb.AddForce(Vector3.back * 100);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.red;
        collisionPerformed = false;
    }
    
    IEnumerator TimerForDash()
    {
        yield return new WaitForSeconds(18f);
        canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = true;
        }
        if(collision.gameObject.CompareTag("Bala"));{

        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = false;
        }
    }

    void OnDash()
    {
        if (canDash == true){
            rb.AddForce();
        }

    }

}
