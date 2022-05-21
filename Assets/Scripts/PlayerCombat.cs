using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private const int MAX_HEALTH = 100;
    private Animator playerAnim;
    public Transform attackPoint;
    [SerializeField] public float attackRange = 0.8f;
    [SerializeField] private GameObject bloodParticle;
    public LayerMask enemeyLayers;
    private float attackFrequency = 0.75f;
    private float attackTime = 0;
    private GameManager gameManager;
    private HealthBar playerHealthBar;
    private int playerDamage = 20;
    HealthManager _PlayerHealth;
    private void Awake() {
        playerAnim = GetComponent<Animator>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        _PlayerHealth = new HealthManager(MAX_HEALTH);

        playerHealthBar = gameManager.getHealthBar;

        playerHealthBar.SetMaxHealth(_PlayerHealth.GetHealth);
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
        
        Collider2D[] hitFields = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemeyLayers);

        foreach (Collider2D enemy in hitFields)
        {
            enemy.GetComponent<EnemyScript>().TakeDamage(playerDamage);
            if(enemy != null) Instantiate(bloodParticle, enemy.transform.position, Quaternion.identity);
        }
    }

    public void PlayerHurt(int damageAmount)
    {

        _PlayerHealth.Damage(damageAmount);

        playerHealthBar.SetCurrentHealth(_PlayerHealth.GetHealth);

        playerAnim.SetTrigger("Hurt");

        if (_PlayerHealth.GetHealth <=0 ) StartCoroutine(PlayerDied());
    }

    private void OnDrawGizmosSelected() {

        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator PlayerDied()
    {
        playerAnim.SetBool("isDead", true);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
        this.enabled = false;
        yield return  new WaitForSeconds(0.5f);
        gameManager.GameOver();
        //Time.timeScale = 0;
    }
}
