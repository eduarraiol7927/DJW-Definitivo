using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotMechanic : MonoBehaviour
{

    public GameObject Bullet;
    public bool isShot = false;
    private float speed = 10f;
    private Vector2 mousePos;
    private Coroutine returnCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        
        if (isShot == false){
            Bullet.transform.localScale = new Vector2(0f,0f);
            Bullet.transform.position = transform.parent.position;
        }
        else{
            Bullet.transform.localScale = new Vector2(0.5f, 0.5f);
            Bullet.transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);
            
            if (returnCoroutine == null){
                returnCoroutine = StartCoroutine(ReturnBulletLocalPosition());
            }
        
        }    

            
        }       
    
    IEnumerator ReturnBulletLocalPosition(){
        yield return new WaitForSeconds(2f);
        isShot = false;
        returnCoroutine = null;
    } 

    void OnCollisionEnter2D(){
        isShot = false;            
        returnCoroutine = null;
    }
        
}
