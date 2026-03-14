using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float Speed = 8f;
    //Velocidade do personagem
    private Rigidbody2D rb;
    //Abreviação do rigidbody pro codigo

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

        public void OnMove(InputValue inputValue)
    //Usa o void porque ele não retorna nada. OnMove pra definir pra que serve. Os Parenteses
    // são pra receber o input que o jogador usar.
    {
        Vector2 input = inputValue.Get<Vector2>();
        //O vector2 input recebe a varíavel inputValue (ex: W, A, S, D), usando .Get<Vector2>() pra ser compreendido como um valor de x e y,
        //ou seja num caso de ter apertado W, x seria 0 e y seria 1.
        rb.linearVelocity = input * Speed;
        //Atribuimos ao rb.linearVelocity (que é a velocidade do rigidbody, ou nosso personagem) 
        // o valor da tecla pressionada multiplicada pela velocidade resultando na movimentação do personagem.

if (inputValue.Get<Vector2>() != Vector2.zero)
        {
            Debug.Log("Jogador está se movendo.");
        }
        else{
            
        }

    }
}
