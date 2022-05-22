using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.92f;
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private LayerMask enemyLayers;
    private const int MAX_HEALTH = 100;
    private Animator playerAnim;
    private int playerDamage = 20;
    private float attackFrequency = 0.75f, attackTime = 0;
    private HealthBar playerHealthBar;
    private HealthManager _PlayerHealth;
    private GameManager gameManager;
    private void Awake() {
        playerAnim = GetComponent<Animator>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        _PlayerHealth = new HealthManager(MAX_HEALTH);

        playerHealthBar = gameManager.getHealthBar;
        playerHealthBar.SetMaxHealth(_PlayerHealth.getHealth);
    }
    
    void Update()
    {
        if(Time.time >= attackTime && gameManager.getIsGameActive)
        {
            if(Input.GetKeyDown(KeyCode.Keypad1))
            {
                PlayerAttack();
                attackTime = Time.time + attackFrequency;
            }
        }      
    }

    void PlayerAttack(){
        playerAnim.SetTrigger("Attack");
        
        Collider2D[] hitFields = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitFields)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(playerDamage);
            if(enemy != null) Instantiate(bloodParticle, enemy.transform.position, Quaternion.identity);
        }
    }

    public void PlayerHurt(int damageAmount)
    {

        _PlayerHealth.Damage(damageAmount);

        playerHealthBar.SetCurrentHealth(_PlayerHealth.getHealth);

        playerAnim.SetTrigger("Hurt");

        if (_PlayerHealth.getHealth <=0 ) StartCoroutine(PlayerDied());
    }
    //for visual hit fields in the editor.
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator PlayerDied()
    {
        playerAnim.SetBool("isDead", true);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        this.enabled = false;
        
        yield return new WaitForSeconds(0.5f);
        gameManager.GameOver();
    }
}
