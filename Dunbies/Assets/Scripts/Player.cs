using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private float speed = 4.0f;
    Rigidbody2D rb2D;
    public bool turnedLeft = false;

    private int health = 500;
    private float startHealth;
    public Image healthFill;
    private float healthWidth;
    private int exp;

    public Text mainText;
    public Text expText;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        healthWidth = healthFill.sprite.rect.width;
        startHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb2D.velocity = new Vector2(horizontal * speed, vertical * speed);
        turnedLeft = false;

        if (horizontal > 0) 
        {
            GetComponent<Animator>().Play("Right");
        }else if (horizontal < 0)
        {
            GetComponent<Animator>().Play("Left");
            turnedLeft = true;
        }
        else if (vertical > 0)
        {
            GetComponent<Animator>().Play("Up");
        }else if (vertical > 0)
        {
            GetComponent<Animator>().Play("Down");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            transform.GetChild(0).gameObject.SetActive(true);
            healthWidth -= collision.gameObject.GetComponent<EnemyZombie>().GetHitStrength();
            if (healthWidth < 1)
            {
                healthFill.enabled = false;
                mainText.gameObject.SetActive(true);
            }
            Vector2 temp = new Vector2(healthWidth * (health / startHealth), healthFill.sprite.rect.height);
            healthFill.rectTransform.sizeDelta = temp;
            Invoke("HidePlayerBlood", 0.25f);
            Invoke("DefaultColor", 0.3f);
        } 
        else if (collision.gameObject.CompareTag("Spawner"))
        {
            collision.gameObject.GetComponent<SpawnerScript>().GetGateway();
        }
    }
    void HidePlayerBlood()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void GainExperience(int amount) 
    {
        exp += amount;
        expText.text = exp.ToString();
    }
    private void DefaultColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
