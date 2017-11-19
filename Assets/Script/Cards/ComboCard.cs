using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCard : Card {

    // Override of card use();
    public override void Use() {

    }

    public ComboCard(ComboCard card) : base (card)
    {
    }
}
