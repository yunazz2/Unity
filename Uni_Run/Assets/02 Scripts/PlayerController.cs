using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRd;
    private Animator animator;
    private AudioSource playerAudio;

    public AudioClip deathClip;
    public float jumpForce = 700.0f;

    public int jumpCount = 0;       // 2단 점프를 초과하면 안되니까

    public bool isGrounded = false; // 땅에 닿았는지 여부
    public bool isDeaded = false;   // 사망 처리를 위해


    void Start()
    {
        playerRd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // 만약 다른 게임 오브젝트의 사운드를 가지고오고 싶다면
        // playerAudio = enemy.GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(isDeaded == true)
        {
            return;
        }

        // 마우스 왼쪽 : 0, 오른쪽 : 1, 휠 : 2
        if (Input.GetMouseButtonDown(0) && jumpCount < 2) { // 마우스 버튼을 눌렀을 때
            
            jumpCount++;
            playerRd.velocity = Vector2.zero;
            //playerRd.velocity = new Vector2 (0, 0);   // 윗 줄과 같은 말

            playerRd.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();

        }
        else if(Input.GetMouseButtonUp(0) && playerRd.velocity.y > 0) { // 마우스 버튼을 눌렀다 뗄 때 하강
            playerRd.velocity = playerRd.velocity * 0.5f;   // 속도를 줄이겠다.
        }

        animator.SetBool("Grounded", isGrounded);
    }

    // 플레이어가 죽었을 때
    public void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRd.velocity = Vector2.zero;   // 죽었으니까 움직이지 못하도록 속도 0 설정
        isDeaded = true;

        GameManager.instance.onPlayerDead();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dead") && isDeaded == false)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
