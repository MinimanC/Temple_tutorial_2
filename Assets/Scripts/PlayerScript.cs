using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;
    public Text winText;
    public Text lifeText;

    private int scoreValue = 0;
    private int lifeTotal = 3;

    public AudioClip background;
    public AudioClip winMusic;

    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        score.text = scoreValue.ToString();
        winText.text = "";
        lifeText.text = "Lives: " + lifeTotal.ToString();

        musicSource.clip = background;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement *speed));

        if (hozMovement > 0)
        {
            anim.SetInteger("State", 1);
        }

        if (hozMovement < 0)
        {
            anim.SetInteger("State", 1);
        }

        if (hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (vertMovement > 0)
        {
            anim.SetBool("Air", true);
        }
        
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                lifeTotal = 3;
                lifeText.text = "Lives: " + lifeTotal.ToString();
                transform.position = new Vector2(44f, -1f);
            }

            if (scoreValue >= 8)
            {
                musicSource.clip = winMusic;
                musicSource.Play();
                winText.text = "You Win! Game created by Casey Temple.";
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            lifeTotal -= 1;
            lifeText.text = "Lives: " + lifeTotal.ToString();
            Destroy(collision.collider.gameObject);

            if (lifeTotal <= 0)
            {
                winText.text = "You Lose.";
                gameObject.SetActive (false);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }

            anim.SetBool("Air", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
