using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCard : Card {

    public int damage;

    // Override of card use();
    public override void Use() {

    }

    public SwordCard(int value1, int value2, int value3, int value4)
        : base(value1, value2, value3, value4) {
        damage = 1;
    }
}
