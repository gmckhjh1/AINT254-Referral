using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageHandler
{
    void TakeDamage(int _damage, GameObject _attackingObject);
}
