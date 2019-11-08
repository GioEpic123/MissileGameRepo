using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable<T>
{
    void OnTakeDamage(T damageTaken);
}

public interface IPowerup
{
    void OnActivation();
}
  
