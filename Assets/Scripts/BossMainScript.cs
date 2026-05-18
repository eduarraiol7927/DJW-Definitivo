using UnityEngine;
using System.Collections;


public class BossMainScript : MonoBehaviour
{
    public float health = 100f;
    //vida do boss
    private float speed = 3f;
    //velocidade do boss
    private Vector2 direction;
    private Vector2 dashDirection;
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
    private bool isDashing = false;
    // variavel pra saber se o boss ta dando o dash

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //pegando o rigidbody e o sprite renderer
        StartCoroutine(TimerForDash());
        //iniciando a coroutine/ nosso timer pro dash
    }

    void FixedUpdate()
    {
        
        if (isDashing == false)
        {
            direction = (player.transform.position - transform.position).normalized;
            // atribuimos ao direction o vetor diferença, q é entre a posição do player e a do boss, fazendo um vetor
            // que aponta do boss pro player, o normalized é pra garantir que seja 1 o valor.
            rb.linearVelocity = direction * speed;
            //faz o boss seguir o player
        }
        
        OnDash();
        OnDamage();
        OnDeath();
    }

    IEnumerator OnDamageEffect()
    //coroutine pra dar o efeito de dano
    {
        collisionPerformed = false;
        dashPerformed = false;
        rb.AddForce(-direction * 10f, ForceMode2D.Impulse);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.red;
        // resetamos a variavel de colisao pra evitar q o dano fique repetindo
    }   
    
    IEnumerator TimerForDash()
    //coroutine de timer pro dash pra dizer pro boss dar dash a cada 18 segundos
    {
        yield return new WaitForSeconds(18f);
        canDash = true;
    }

    IEnumerator DashWarningEffect()
    //coroutine q antecipa o dash, fazendo o boss piscar pra avisar o player
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = true;
        }
        if (collision.gameObject.CompareTag("Bala")) ;
        {
            health -= 1f;
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
        //deu os 18 segundos, o boss pode dar dash                    
            canDash = false;
            //resetamos a variavel pra ele n ficar dando dash infinitamente
            StartCoroutine(DashSequence());
            //iniciamos a coroutine do dash, eu decidi botar numa coroutine pra poder usar
            // o yield return pra esperar o dashWarningEffect terminar antes de partir pro dash
        }
    }

    IEnumerator DashSequence()
    {
        isDashing = true;
        //o isDashing vira true pra ele parar de seguir o player.
        yield return StartCoroutine(DashWarningEffect());
        //o yield return é pra esperar a coroutine terminar antes de seguir.
        rb.AddForce(direction * 30f, ForceMode2D.Impulse);
        //ação do dash realmente
        dashPerformed = true;
        //dash foi performado
        
        yield return new WaitForSeconds(1f);
        //tempo do boss voltar ao comportamento normal.
        isDashing = false;
        //isDashing retorna ao false pro boss voltar ao seu comportamento normal

        StartCoroutine(TimerForDash());
        //iniciamos o timer pro dash, pra acontecer isso tudo de novo.
    }

    void OnDamage()
    {
        if (collisionPerformed == true && dashPerformed == true)
        //caso o boss tenha batido na parede e por causa do dash, perde dano.
        {
            health -= 10;
            StartCoroutine(OnDamageEffect());
        }
    }

    void OnDeath()
    {
        if (health <= 0)
        {
            StartCoroutine(DeathEffect());
        }
    }

    IEnumerator DeathEffect()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        sr.color = Color.red;
        //o boss aumenta e diminui antes de morrer

        Destroy(gameObject);
        //o boss morre.
    }

}
