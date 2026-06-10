using UnityEngine;
using System.Collections;


public class BossMainScript : MonoBehaviour
{
    public float health = 100f;
    //vida do boss
    public float speed = 6f;
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
    public int damage = 2;
    private PlayerController playerController;
    private bool damagedPlayer = false;

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
        
        if (isDashing == false && damagedPlayer == false)
        //coloquei essa condição pro linearVelocity nao sobrepor o AddForce, senao ele n funciona 
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

    void OnDamage()
    {
        if (collisionPerformed == true && dashPerformed == true)
        //caso o boss tenha batido na parede e por causa do dash, toma dano.
        {
            health -= 5;
            
            StartCoroutine(OnDamageEffect());
        }

        if (damagedPlayer == true)
        {
            rb.AddForce(-direction * 0.3f, ForceMode2D.Impulse);
            //coloco um knockback aq quando ele bater no player, pra dar tempo de escapar
            StartCoroutine(DamagedPlayerReturn());
        }
    }

    void OnDeath()
    {
        if (health <= 0)
        {
            StartCoroutine(DeathEffect());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    //Queremos que o boss colida com isso fisicamente, ou seja, seja barrado nisso.
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    //vou usar o trigger pra detectar a bala, pq se eu colocar junto com o OnCollisionEnter2D, o boss n vai detectar a bala, pq a bala tem um collider trigger, entao o OnCollisionEnter2D
    // ele fica pensando q a bala n existe, mesmo separando por tag, alem disso ativo o IsTrigger do bc da bala.
    {
        if (collider.CompareTag("Bala"))
        {
            health -= 1f;
        }

            if (collider.gameObject.CompareTag("Player"))
        {
            damagedPlayer = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = false;
            // resetamos a variavel de colisao pra evitar q o dano fique repetindo
        }
    }

    IEnumerator OnDamageEffect()
    //coroutine pra dar o efeito de dano
    {
        collisionPerformed = false;
        dashPerformed = false;
        rb.AddForce(-direction * 10f, ForceMode2D.Impulse);
        //knockback do boss, com o direction negativo pra ele justamente voltar        
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.red;
    }   
    
    IEnumerator TimerForDash()
    //coroutine de timer pro dash pra dizer pro boss dar dash a cada 18 segundos
    {
        yield return new WaitForSeconds(5f);
        canDash = true;
    }

    IEnumerator DashWarningEffect()
    //coroutine q antecipa o dash, fazendo o boss piscar pra avisar o player
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
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


    IEnumerator DamagedPlayerReturn()
    //coroutine pra resetar a variavel damagedPlayer, dando tempo pós dano. tive q colocar uma coroutine
    //porque nao da pra só colocar o damagedPlayer = false, seria o equivalente a piscar a variavel.
    {
        yield return new WaitForSeconds(0.5f);
        damagedPlayer = false;
    }


    IEnumerator DeathEffect()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.4f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.4f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        sr.color = Color.red;
        //o boss aumenta e diminui antes de morrer

        Destroy(gameObject);
        //o boss morre.
    }

    

}
