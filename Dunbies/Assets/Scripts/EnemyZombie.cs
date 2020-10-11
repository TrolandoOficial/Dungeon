using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyZombie : MonoBehaviour
{
    private float range;
    private Transform target;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    private float speed = 2.0f;
    private float thrust = 1.5f;
    public int health = 5;
    private int hitStrength = 20;
    private bool isDead = false;

    public GameManager gameManager;
    public Sprite deathSprite;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int rnd = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        target = GameObject.Find("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        range = Vector2.Distance(transform.position, target.position);
        if (range < minDistance && !isDead) 
        {
            if (!targetCollision) 
            {
                //Pega a posição do Player
                transform.LookAt(target.position);

                //Corrige a rotação
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }

        transform.rotation = Quaternion.identity;
    }

    //Impede que o inimigo possa ficar em cima do Player causando dano contínuo, faz com que ele seja repelido por um curto tempo após acertar o Player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !targetCollision) 
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;

            targetCollision = true;

            //Define a condição para todas as direções de contato com o Player
            bool right = contactPoint.x > center.x;
            bool left= contactPoint.x < center.x;
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            if (right) GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            if (left) GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            if (top) GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
            if (bottom) GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust, ForceMode2D.Impulse);

            //Define o tempo que leverá para executar a função FalseCollision
            Invoke("FalseCollision", 0.3f);
        }
    }

    //Só voltará a atacar o Player quando essa função for acionada, assim evita que ele fique preso na animação de ataque ao ser repelido
    void FalseCollision() 
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void TakeDamage(int amount) 
    {
        health -= amount;
        GetComponent<SpriteRenderer>().color = Color.red;
        if (health < 1) 
        {
            isDead = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<SpriteRenderer>().sprite = deathSprite;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            target.GetComponent<Player>().GainExperience(100);
            Invoke("EnemyDeath", 1.5f);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        Invoke("HideBlood", 0.25f);
        Invoke("DefaultColor", 0.3f);
    }

    void HideBlood() 
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void EnemyDeath() 
    {
        Destroy(gameObject);
        gameManager.SetZombieCount(-1);
    }

    public int GetHitStrength() 
    {
        return hitStrength;
    }
    private void DefaultColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
