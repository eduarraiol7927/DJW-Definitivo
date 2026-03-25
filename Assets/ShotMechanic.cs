using UnityEngine;

public class ShotMechanic : MonoBehaviour
{

    public GameObject Bullet;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isHidden(){
            Bullet.transform.localScale = new Vector2(0,0);
        }
        bool isShowing(){
            Bullet.transform.localScale = new Vector2(1, 1);
        }

        #Alternar os bools quando chegar em casa
    }
}
