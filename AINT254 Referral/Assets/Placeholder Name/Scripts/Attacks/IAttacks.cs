using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacks
{
    int AttackPower { get; }
    float AttackReload { get; }

    void StartAttack();
}
