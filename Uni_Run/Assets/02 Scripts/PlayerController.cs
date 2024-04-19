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

    public int jumpCount = 0;       // 2�� ������ �ʰ��ϸ� �ȵǴϱ�

    public bool isGrounded = false; // ���� ��Ҵ��� ����
    public bool isDeaded = false;   // ��� ó���� ����


    void Start()
    {
        playerRd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // ���� �ٸ� ���� ������Ʈ�� ���带 ��������� �ʹٸ�
        // playerAudio = enemy.GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if(isDeaded == true)
        {
            return;
        }

        // ���콺 ���� : 0, ������ : 1, �� : 2
        if (Input.GetMouseButtonDown(0) && jumpCount < 2) { // ���콺 ��ư�� ������ ��
            
            jumpCount++;
            playerRd.velocity = Vector2.zero;
            //playerRd.velocity = new Vector2 (0, 0);   // �� �ٰ� ���� ��

            playerRd.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();

        }
        else if(Input.GetMouseButtonUp(0) && playerRd.velocity.y > 0) { // ���콺 ��ư�� ������ �� �� �ϰ�
            playerRd.velocity = playerRd.velocity * 0.5f;   // �ӵ��� ���̰ڴ�.
        }

        animator.SetBool("Grounded", isGrounded);
    }

    // �÷��̾ �׾��� ��
    public void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRd.velocity = Vector2.zero;   // �׾����ϱ� �������� ���ϵ��� �ӵ� 0 ����
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
