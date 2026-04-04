using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10f;
    //Velocidade do personagem
    private Rigidbody2D rb;
    //Variável que armazena o rigidbody
    private Vector2 input;
    //Pra lembrar o input do OnMove e usar no FixedUpdate

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    //Atribuindo rigidbody pro rb
    }
    // Update is called once per frame
    void Update()
    {
    }

    void OnMove(InputValue inputValue)
    //Usa o void porque ele não retorna nada. OnMove pra definir pra que serve. Os Parenteses são pra receber o input que o jogador
    // usar.
    {
        input = inputValue.Get<Vector2>();
        //O input recebe a varíavel inputValue (ex: W, A, S, D), e usamos o .Get<Vector2>() pra ser compreendido como um valor de x
        // e y, ou seja num caso de ter apertado W, x seria 0 e y seria 1.
    }

    void FixedUpdate()
    //Usamos o FixedUpdate porque é melhor lidar com a física do jogo com ele.
    {
        if (input != Vector2.zero)
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
}

