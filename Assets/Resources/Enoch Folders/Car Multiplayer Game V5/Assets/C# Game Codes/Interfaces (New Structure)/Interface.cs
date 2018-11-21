using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount);
    float GetDamage { get; set; }
}

public interface IPowerUpUse
{
    void TriggerPowerup(string powerUp);

    //Powerup logic just call this interface, then destroy or hideselves
}

