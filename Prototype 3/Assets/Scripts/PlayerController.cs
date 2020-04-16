using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;

    public float jumpForce;
    public float gravityModifier;

    private bool isOnGround = true;

    internal bool isMainPhase = false;
    internal bool isGameOver = false;

    public float initialWalkSpeed;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;

        playerAnim = GetComponent<Animator>();
        playerAnim.SetFloat("Speed_f", 0.4f);

        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMainPhase)
        {
            transform.Translate(Vector3.forward * initialWalkSpeed * Time.deltaTime);
            if (transform.position.x >= 0)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
                isMainPhase = true;
                playerAnim.SetFloat("Speed_f", 1f);
                dirtParticle.Play();
            }
        }
        if (Input.GetButtonDown("Jump") && isOnGround && !isGameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;

            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            if (isMainPhase)
            {
                dirtParticle.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            isGameOver = true;
            Debug.Log("GAME OVER");

            playerAnim.SetBool("Death_b", true);
            // playerAnim.SetInteger("DeathType_int", 1);

            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound);
        }
    }
}
