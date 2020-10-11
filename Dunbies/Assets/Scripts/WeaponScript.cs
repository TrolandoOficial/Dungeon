using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private bool swing = false;
    int degree = 20;
    private float weaponX = 0.3f;
    private float weaponY = -0.4f;

    Vector3 pos;
    public GameObject player;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (swing) 
        {
            degree -= 5;
            if (degree < -75) 
            {
                degree = 0;
                swing = false;
                GetComponent<SpriteRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            transform.eulerAngles = Vector3.forward * degree;
        }
    }

    void Attack() 
    {
        if (player.GetComponent<Player>().turnedLeft)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            weaponX = -0.3f;
        } else {
            GetComponent<SpriteRenderer>().flipX = false;
            weaponX = 0.3f;
        }
        pos = player.transform.position;
        pos.x += weaponX;
        pos.y += weaponY;
        transform.position = pos;
        swing = true;
    }
}
