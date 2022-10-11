using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb2D { get; private set; }
    public CapsuleCollider2D capCollider2D { get; private set; }

    private Health healthScript;
    private PlayerMovement playerMovementScript;

    private float nextAttackTime;
    public float attackRate;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capCollider2D = GetComponent<CapsuleCollider2D>();
        playerMovementScript = GetComponent<PlayerMovement>();
        healthScript = GetComponent<Health>();
    }

    private void OnEnable()
    {
        healthScript.OnHealthChange.AddListener(OnHealthChange);
    }

    private void OnHealthChange(float amount)
    {
        UIController.Instance.UpdateHealthBar(amount);
    }

    private void Update()
    {
        if (this.TryGetComponent<Health>(out Health health))
        {
            if (health.GetCurrentHealth() <= 0.0f)
            {
                Debug.Log(this.name + " is Dead");

                StartCoroutine(Die());
            }
        }

    }

    private void LateUpdate()
    {
        Animate();
        FlipSprite();
    }

    IEnumerator Die()
    {
        capCollider2D.size = new Vector2(capCollider2D.size.x, 0.65f);

        animator.Play("PlayerDeath");

        yield return new WaitForSeconds(1f);

        this.enabled = false;

        Destroy(this.gameObject);

        GameManager.Instance.LoseGame();
    }

    private void Animate()
    {          
        if(playerMovementScript.isGrounded)
            animator.SetFloat("Speed", Mathf.Abs(playerMovementScript.horizontalMovement));

        animator.SetFloat("Vertical Velocity", rb2D.velocity.y);

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
               
        }
    }

    private void FlipSprite()
    {
        if (playerMovementScript.horizontalMovement == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (playerMovementScript.horizontalMovement == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))        
            this.transform.parent = collision.gameObject.transform;


        if (collision.gameObject.CompareTag("DeathBox"))
            transform.position = new Vector3(-9.57f, -3.43f, 1);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))      
            this.transform.parent = null;
    }

    private void OnDisable()
    {
        healthScript.OnHealthChange.RemoveListener(OnHealthChange);
    }
}
