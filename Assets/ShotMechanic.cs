using UnityEngine;
using UnityEngine.InputSystem;

public class ShotMechanic : MonoBehaviour
{

    public GameObject Bullet;
    private bool isShot = false;
    protected float speed = 15f;
    private Vector2 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        
        if (isShot= false){
            Bullet.transform.localScale = new Vector2(0f,0f);
        }
        else{
            Bullet.transform.localScale = new Vector2(0.5f, 0.5f);
            Vector2.MoveTowards(transform.localPosition, mousePos, speed);
            
        }

        
        
    }
}
