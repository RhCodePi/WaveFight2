using System.Collections;
using System.Collections.Generic;

public interface IEnemy
{
    void TakeDamage(int damageAmount);
    void Died();
}
