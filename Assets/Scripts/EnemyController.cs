using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Animator e_animator { get; set; }
    public Rigidbody2D e_rb2D { get; private set; }
    public BoxCollider2D e_boxCollider2D { get; private set; }

    [Header("Attacking")]
    public float attackRange;
    public float attackRate;
    private float nextAttackTime;

    [Header("Detection")]
    public Transform player;
    public float detectionRange;

    [Header("Movement")]
    public float speed;
    public bool ifFlyingType;
    public Vector2[] patrolPoints ;
    int indexCheck;
    bool isFlipped;

    [Header("Health")]
    [SerializeField] Image healthBar;
    private Health healthScript;

    public enum Behaviour { Patrol, MoveToPlayer, Attack, Dead};

    public Behaviour enemyBehaviour;

    public static event Action<GameObject> OnKilledEvent;

    private void Awake()
    {
        e_rb2D = GetComponent<Rigidbody2D>();
        e_animator = GetComponent<Animator>();
        e_boxCollider2D = GetComponent<BoxCollider2D>();
        healthScript = GetComponent<Health>();
    }

    private void OnEnable()
    {
        healthScript.OnHealthChange.AddListener(OnHealthChange);
    }

    private void Update()
    {
        switch (enemyBehaviour)
        {
            case Behaviour.Patrol:
                Patrol();
                break;
            case Behaviour.MoveToPlayer:
                MoveToPlayer();
                break;
            case Behaviour.Attack:         
                Attack();
                break;
            default:
                break;
        }
    }

    private void OnHealthChange(float amount)
    {
        healthBar.fillAmount = amount;

        if (amount == 0) StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        e_animator.SetTrigger("isDead");

        OnKilledEvent?.Invoke(gameObject);

        this.enabled = false;

        yield return new WaitForSeconds(0.6f);

        Destroy(this.gameObject);
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            e_animator.Play(this.name + "Attack");

            nextAttackTime = Time.time + 1f / attackRate;               
        }

        if (Vector2.Distance(player.position, transform.position) > attackRange)
            enemyBehaviour = Behaviour.MoveToPlayer;



    }

    void MoveToPlayer()
    {
        Move(player.position);

        FlipSprite(player.position);

        if (Vector2.Distance(player.position, transform.position) > detectionRange)
        {
            enemyBehaviour = Behaviour.Patrol;
            Debug.Log("OutOfDetectRange");
        }
        else if(Vector2.Distance(player.position, transform.position) < attackRange)
        {
            enemyBehaviour = Behaviour.Attack;
            Debug.Log("InAttackRange");
        }
    }

    void Patrol()
    {
        if ((Vector2)transform.position == patrolPoints[indexCheck])
        {
            if (indexCheck == patrolPoints.Length - 1) indexCheck = 0;
            else indexCheck++;
        }
        
        Move(patrolPoints[indexCheck]);

        FlipSprite(patrolPoints[indexCheck]);

        if (Vector2.Distance(player.position, transform.position) < detectionRange)
        {
            enemyBehaviour = Behaviour.MoveToPlayer;
            Debug.Log("InPlayerRange");
        }
    }

    void Move(Vector2 pos)
    {
        if (!ifFlyingType)
        {
            Vector2 newPos = new Vector2(pos.x, transform.position.y);

            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);


        e_animator.SetBool("isMoving", true);
    }

    private void FlipSprite(Vector2 target)
    {
        Vector3 flip = transform.localScale;

        flip.x *= -1f;

        if(transform.position.x > target.x && !isFlipped)
        {
            transform.localScale = flip;
            isFlipped = true;
        }
        else if(transform.position.x < target.x && isFlipped)
        {
            transform.localScale = flip;
            isFlipped = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (!ifFlyingType)patrolPoints[i].y = transform.position.y;
            Gizmos.DrawWireSphere(patrolPoints[i], 0.5f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnDisable()
    {
        healthScript.OnHealthChange.RemoveListener(OnHealthChange);
    }
}
