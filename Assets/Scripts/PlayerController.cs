using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 15f;
    //Velocidade do personagem
    private Rigidbody2D rb;
    //Variável que armazena o rigidbody
    private Vector2 input;
    //Pra lembrar o input do OnMove e usar no FixedUpdate
    private ShotMechanic shotMechanic;
    //variavel pra referenciar o script shotmechanic e usar ele pro isShot
    public int health = 6;
    //variavel de vida
    private BossMainScript boss1;
    public bool wasDamaged = false;
    private Vector2 direction;
    private bool knockbackPerforming = false;
    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    //Atribuindo rigidbody pro rb
        sr = GetComponent<SpriteRenderer>();
        //atribuindo o sprite renderer pra sr 
        shotMechanic = GetComponentInChildren<ShotMechanic>();
    //atribuindo o script shotmechanic pra variavel, pegando ele do objeto bullet.
        boss1 = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossMainScript>();
    }
    // Update is called once per frame
    void Update()
    {
        OnDamage();
        OnDeath();
    }

    void OnMove(InputValue inputValue)
    //Os Parenteses são pra receber o input que o jogador usar.
    {
        input = inputValue.Get<Vector2>();
        //O input recebe a varíavel inputValue (ex: W, A, S, D), e usamos o .Get<Vector2>() pra ser compreendido como um valor de x
        // e y, ou seja num caso de ter apertado W, x seria 0 e y seria 1.
    }

    void OnShot(InputValue inputValue)
    {
        if (inputValue.isPressed)
        //se a tecla de tiro for pressionada,
        {
            shotMechanic.isShot = true;
            //usamos o isShot do shotmechanic porque ele é responsável por controlar as mecanicas do tiro, que estao no script do shotmechanic.
            shotMechanic.targetPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            //usamos o targetPos do shotmechanic pra definir a posição do mouse como fixa e nova toda vez que clicarmos.

        }
    }

    void FixedUpdate()
    //Usamos o FixedUpdate porque é melhor lidar com a física do jogo com ele.
    {
        if (input != Vector2.zero && knockbackPerforming == false)
        //Caso o input seja diferente de 0,
        {
            rb.linearVelocity = input * Speed;
        //Atribuimos ao rb.linearVelocity (que é a velocidade do RigidBody) o valor da tecla pressionada multiplicada pela
        // velocidade resultando na movimentação do personagem.
        }
        else
        //Caso o input seja 0, então, a tecla foi solta,
        {
            rb.linearVelocity = new Vector2(Mathf.MoveTowards(rb.linearVelocity.x, 0f, 25f * Time.fixedDeltaTime), 
            Mathf.MoveTowards(rb.linearVelocity.y, 0f, 25f * Time.fixedDeltaTime));
            //Atribuimos ao rb.linearVelocity um novo valor/vector2, no entanto, com o Mathf.MoveTowards, que será responsável pela
            // desaceleração, com a estrutura de seus parenteses (velocidade atual, velocidade alvo, aceleração multiplicada pelo 
            // Time.fixedDeltaTime pro movimento ser consistente).

            //Mais detalhadamente, o que acontece no Mathf.MoveTowards é que ele pega a velocidade atual e vai subtraindo a
            // aceleração até chegar ao 0.

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss")){
            wasDamaged = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss") && gameObject.tag == "Player"){
            wasDamaged = false;

        }
    }

    void OnDamage(){
        direction = (transform.position - boss1.transform.position).normalized;
        //é o mesmo direction usado no BossScript pra ele mirar no player, só q vou usar ele negativo pra ser knockback

        if (wasDamaged == true){
            knockbackPerforming = true;
            health -= boss1.damage;
            rb.AddForce(direction * 30f, ForceMode2D.Impulse);
            wasDamaged = false;
            knockbackPerforming = false;
        }
    }

    IEnumerator DeathEffect()
    {
        transform.localScale = new Vector2(1f, 1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector2(1.5f, 1.5f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector2(1f, 1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector2(1.5f, 1.5f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector2(1f, 1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = new Vector2(1.5f, 1.5f);
        sr.color = Color.red;

        Destroy(gameObject);
    }    

    void OnDeath(){
        if (health <= 0){
            boss1.speed = 0f;
            StartCoroutine(DeathEffect());
        }
    }
}

