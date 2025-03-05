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
        
        if(character.isGrounded)
        {
            direction = Vector3.down;
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector3.up * jumpForce;
                jumpSound.Play();
            }
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

        yield return new WaitForSeconds(duration);

        IsInvincible = false; // Disable invincibility
        Debug.Log("Invincibility Over!");
        if (trailParticles != null)
            trailParticles.Stop();

        GameManager.Instance.gameSpeed /= speedMultiplier;
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
