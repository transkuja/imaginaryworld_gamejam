using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCard : Card {

    // Override of card use();
    public override void Use() {

    }

    public SwapCard(SwapCard card) : base (card)
    {
    }
}
