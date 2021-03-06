﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : Card {


    public int healValue;

    // Override of card use();
    public override void Use() {

    }

    public HealCard(int value1, int value2, int value3, int value4, int health = 3)
        : base(value1, value2, value3, value4, health) {
        healValue = 1;
    }

    public HealCard(HealCard card) : base (card)
    {
        healValue = card.healValue;
    }
}
