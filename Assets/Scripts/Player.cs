using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private ParticleSystem trailParticles;
    public AudioSource jumpSound;
    public float gravity = 9.81f*2f;
    public float jumpForce = 8f;
    public AudioSource powerupSound;
    private SpriteRenderer spriteRenderer;
    public bool IsInvincible { get; private set; } = false;
    public int extraLives = 0;
    public void init()
    {
        if (trailParticles != null)
            trailParticles.Stop();
    }

    private void Start()
    {
        trailParticles = GetComponentInChildren<ParticleSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        init();
    }
    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        direction = Vector3.zero;
    }
    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            direction = Vector3.down;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                jump();
            }
        }
        character.Move(direction * Time.deltaTime);
    }

    public void jump()
    {
        if (character.isGrounded)
        {
            direction = Vector3.up * jumpForce;
            jumpSound.Play();
        }
        character.Move(direction * Time.deltaTime);
    }

    public void ActivateInvincibility(float duration, float speedMultiplier)
    {
        StartCoroutine(InvincibilityRoutine(duration, speedMultiplier));
    }

    private IEnumerator InvincibilityRoutine(float duration, float speedMultiplier)
    {
        powerupSound.Play();
        IsInvincible = true; // Enable invincibility
        Debug.Log("Invincibility Activated!");

        if (trailParticles != null)
            trailParticles.Play();

        GameManager.Instance.gameSpeed *= speedMultiplier;

        yield return new WaitForSeconds(duration-duration/4);
        StartCoroutine(FlashEffect());
        yield return new WaitForSeconds(duration/4);
        IsInvincible = false; // Disable invincibility
        Debug.Log("Invincibility Over!");
        if (trailParticles != null)
            trailParticles.Stop();

        GameManager.Instance.gameSpeed /= speedMultiplier;
    }

    private IEnumerator FlashEffect()
    {
        Renderer renderer = GetComponent<Renderer>();
        for (int i = 5; i > 0; i--)  // Flash 5 times
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        renderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !IsInvincible)
        {
            GameManager.Instance.GameOver();
        }
        if (other.CompareTag("ExtraLife"))
        {
            extraLives++;
            powerupSound.Play();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("MirrageObstacle"))
        {
            Destroy(other.gameObject);
            return; 
        }
    }

}
