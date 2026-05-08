using UnityEngine;
using System.Collections;


public class BossMainScript : MonoBehaviour
{
    private float health = 100;
    //vida do boss
    private SpriteRenderer sr;
    //vou usar pra dar um efeito de dano
    private Rigidbody rb;
    //vamos usar o rigidbody aqui pro knockback e pro dash.
    public GameObject player;
    //vai precisar pra ele seguir o player
    private bool collisionPerformed = false;
    //variavel pra saber se ja houve colisao
    private bool dashPerformed = false;
    //variavel pra saber se o dash ja foi realizado
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        //pegando o rigidbody e o sprite renderer
    }


    void Update()
    {
        if (collisionPerformed == true && dashPerformed == true)
        {
            health -= 20;
            OnDamageEffect();
        }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            collisionPerformed = true;
        }
    }
    
}
