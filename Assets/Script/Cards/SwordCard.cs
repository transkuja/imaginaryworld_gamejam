using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCard : Card {

    public int damage;

    // Override of card use();
    public override void Use() {

    }

    public SwordCard(int value1, int value2, int value3, int value4, int health = 3)
        : base(value1, value2, value3, value4, health) {
        damage = 1000;
    }
}
