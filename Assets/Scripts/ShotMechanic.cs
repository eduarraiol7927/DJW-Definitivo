using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotMechanic : MonoBehaviour
{

    public GameObject Bullet;
    //declaramos o nosso gameobject em uma variavel pra usar ele, detalhe, precisa botar o prefab na 
    // unity
    public bool isShot = false;
    //declaramos uma variavel bool pra decidir se a bala foi atirada, começando como falsa para a bala 
    // nao aparecer e também nao começar atirando
    private float speed = 20f;
    public Vector2 mousePos;
    //declaramos a variável mouseposition pra usar posteriormente no codigo, pra receber a posiçao do 
    // mouse, cuja é vector2
    private Coroutine returnCoroutine;
    //declaramos essa variável de tipo coroutine pra verificar se a coroutine returnbulletlocalposition
    // está rodando antes de iniciar outra no meio, pra evitar bugs.
    private Transform playerTransform;
    //variavel pra pegar a posiçao do player
    public Vector2 targetPos;
    void Start()
    {
        playerTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //a variavel mousepos recebe o valor da posição do mouse.
        
        if (isShot == false){
            Bullet.transform.localScale = new Vector2(0f,0f);
            //a bala desaparece.
            Bullet.transform.position = playerTransform.position;
            //a bala retorna pra posição do player, que é o pai do gameobject bullet.
            Bullet.transform.SetParent(playerTransform);
            //a bala é reparentada pro player, pra ficar presa a ele e se mover junto com ele. 
        }
        else{
        //caso isShot seja true, ou, a bala foi atirada:
            Bullet.transform.localScale = new Vector2(0.3f, 0.17f);
            //a bala aparece,
            Bullet.transform.position = Vector2.MoveTowards(Bullet.transform.position, targetPos, speed * Time.deltaTime);
            //a bala se move para a posição do mouse(usando o targetPos para ser fixo) partindo da posição do player, com a 
            // velocidade definida.
            

            if (returnCoroutine == null)
            //eu planejava botar o nome igual o da coroutine, mas dá problema. enfim, se a coroutine for null,
            //ou seja, não estiver rodando,
            {
                returnCoroutine = StartCoroutine(ReturnBulletLocalPosition());
                //pode iniciar mais uma vez a coroutine.
            }
        
        }


        if (Vector2.Distance(Bullet.transform.position, targetPos) < 0.1f)
        //se a distancia, EM VECTOR2, entre a posiçao da bala e do alvo for 
        // menor que 0.1, a bala ja chegou no alvo.
        {
            ResetShot();
        }          
    
        }       
    
    IEnumerator ReturnBulletLocalPosition()
    //declaramos a coroutine returnbulletlocalposition fora do void update porque nao da pra usar o yield return
    //se for la. 
    {
        yield return new WaitForSeconds(5f);
        //basicamente, espere por 5 segundos.
        ResetShot();
    } 

    void OnCollisionEnter2D()
    //isso é mt importante, porque só o box collider sozinho fica bugando a bala, fazendo ela ficar presa
    // tentando passar a parede, e o player fica indo pra tras.
    {
        //casa a bala colida com algo, com a parede ou com um inimigo,
        ResetShot();
    }
    

    private void ResetShot()
    //criei esse metodo pra resumir essas duas coisas, já que se repetiam varias vezes
    // no codigo.
    {
        isShot = false;
        //retorna o isShot pra false, fazendo com q a bala retorne posição e desapareça.
        returnCoroutine = null;
        //returnCoroutine volta a ser null e permite que outra seja iniciada, como explicado anteriormente.
        Bullet.transform.SetParent(playerTransform);
        //desparentamos a bala do player, pra ela nao ficar presa a ele e poder se mover sozinha.
    }
        
}
