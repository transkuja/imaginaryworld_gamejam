using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCard : Card {


    public int defenseValue;

    // Override of card use();
    public override void Use() {

    }

    public ShieldCard(int value1, int value2, int value3, int value4)
        : base(value1, value2, value3, value4) {
        defenseValue = 1;
    }
}
