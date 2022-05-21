using System;

public class HealthManager
{
    private int maxHealth, health;
    private bool isDead;
    private float rateScale;
    public int GetHealth {get {return health;}}
    public bool IsDead {get {return isDead;}}
    public float RateScale {get {return rateScale;}}

    public HealthManager(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        rateScale = (float) health / maxHealth;
        isDead = (health <=0) ? true : false;
    }
    
}
