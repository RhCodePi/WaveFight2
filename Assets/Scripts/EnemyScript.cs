using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour , IEnemy
{
    private const int MAX_HEALTH = 100;
    [SerializeField] private GameObject popup, bloodParticle;
    public Transform attackPoint;
    public LayerMask playerLayer;
    private Vector3 enemyPos, playerPos;
    private float enemyMoveRange = 2.5f, enemyDir= 0;
    [SerializeField ]private int enemyDamage;
    private float attackTime = 0, attackDuration = 2f; [SerializeField] private float attackRange = 0.75f;
    [SerializeField] private float rotationY_1, rotationY_2;
    private Animator enemyAnim;
    private GameManager gameManager;
    [SerializeField] public float speed = 3f;
    HealthManager _health;
    void Start()
    {
        enemyAnim = GetComponent<Animator>();

        _health = new HealthManager(MAX_HEALTH);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Update() {
        if(gameManager.getIsGameActive)
        {
            EnemyMovement();
        }
    }

    void EnemyMovement()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>().position;
        enemyPos = transform.position;
        enemyDir = (enemyPos - playerPos).x; 
        Vector3 displacement = (playerPos - enemyPos).normalized;

        transform.eulerAngles = (enemyDir < 0) ? new Vector3(0, rotationY_1, 0) : new Vector3(0, rotationY_2, 0);

        if(Mathf.Abs((enemyPos - playerPos).x) < enemyMoveRange)
        {
            enemyAnim.SetBool("isMove", false);
            EnemyAttackPatern();
        }
        else{
            enemyAnim.SetBool("isMove", true);
            transform.position += displacement * speed * Time.deltaTime;
        }
    }

    public void TakeDamage(int damegeAmount)
    {
        _health.Damage(damegeAmount);
        DamageIndicator dmgIndicator = Instantiate(popup.transform, transform.position + Vector3.up, Quaternion.identity).GetComponent<DamageIndicator>();
        dmgIndicator.SetDamageText(damegeAmount);
        gameManager.SlideBar(_health.RateScale);
        enemyAnim.SetTrigger("Hurt");
        
        if(_health.GetHealth <= 0) Died();
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

    private void OnDrawGizmosSelected()
    {

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
