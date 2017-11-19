using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils  {

    public static Card CopyCard(Card _card)
    {
        if (_card.GetType() == typeof(ShieldCard))
            return new ShieldCard((ShieldCard)_card);
        if (_card.GetType() == typeof(SwordCard))
            return new SwordCard((SwordCard)_card);
        if (_card.GetType() == typeof(HealCard))
            return new HealCard((HealCard)_card);
        if (_card.GetType() == typeof(SwapCard))
            return new SwapCard((SwapCard)_card);
        if (_card.GetType() == typeof(ComboCard))
            return new ComboCard((ComboCard)_card);
        return new Card(_card);
    }
}
