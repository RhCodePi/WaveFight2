using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject popup, bloodParticle;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField ]private int enemyDamage;
    [SerializeField] private float rotationY_1, rotationY_2;
    [SerializeField] public float speed = 3f;
    [SerializeField] private float attackRange = 0.75f;
    private const int MAX_HEALTH = 100;
    private float enemyMoveRange = 2.5f;
    private float attackTime = 0, attackDuration = 2f;
    private Vector3 enemyPos, playerPos;
    private Animator enemyAnim;
    private GameManager gameManager;
    private HealthManager _health;
    void Start()
    {
        enemyAnim = GetComponent<Animator>();

        _health = new HealthManager(MAX_HEALTH);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update() 
    {
        if(gameManager.getIsGameActive) EnemyMovement();
    }

    void EnemyMovement()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

        Vector3 displacement = (playerPos - transform.position);

        transform.eulerAngles = (displacement.x < 0) ? new Vector3(0, rotationY_1, 0) : new Vector3(0, rotationY_2, 0);

        if(Mathf.Abs(displacement.x) < enemyMoveRange)
        {
            enemyAnim.SetBool("isMove", false);
            EnemyAttackPatern();
        }
        else{
            enemyAnim.SetBool("isMove", true);
            transform.position += displacement.normalized * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damegeAmount)
    {
        _health.Damage(damegeAmount);
        DamageIndicator dmgIndicator = Instantiate(popup.transform, transform.position + Vector3.up, Quaternion.identity).GetComponent<DamageIndicator>();
        dmgIndicator.SetDamageText(damegeAmount);
        gameManager.SlideBar(_health.getRateScale);
        enemyAnim.SetTrigger("Hurt");
        
        if(_health.getHealth <= 0) Died();
    }

    public void Died()
    {
        enemyAnim.SetBool("IsDead", true);
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(this.gameObject, 2f);
    }
    void EnemyAttackPatern()
    {
        enemyAnim.SetInteger("attackTime", 0);
        if(Time.time > attackTime)
        {
            EnemyCombat();
            attackTime = Time.time + attackDuration;
        }
    }

    void EnemyCombat()
    {
        enemyAnim.SetInteger("attackTime", 1);

        Collider2D[] hitFields = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitFields)
        {
            player.GetComponent<PlayerCombat>().PlayerHurt(enemyDamage);
            if(player != null) Instantiate(bloodParticle, player.transform.position, Quaternion.identity);
        }
    }
    // for visual hitfields in the editor.
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
