using System;
public class HealthManager
{
    private int maxHealth, health;
    private bool isDead;
    private float rateScale;
    public int getHealth {get {return health;}}
    public bool getIsDead {get {return isDead;}}
    public float getRateScale {get {return rateScale;}}

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
