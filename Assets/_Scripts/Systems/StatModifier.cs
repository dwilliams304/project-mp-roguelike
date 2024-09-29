/*
                THANK YOU TO KRYZAREL!!!
                
                TUTORIAL:
                https://www.youtube.com/watch?v=SH25f3cXBVc

*/


using System;

namespace ContradictiveGames.Systems.Stats
{
    public enum StatModifierType {
        Flat_Add = 100,
        Percent_Add = 200,
        Percent_Mult = 300,
    }


    [Serializable]
    public class StatModifier {
        public float Value;
        public StatModifierType ModifierType;
        public readonly int Order;
        public readonly object Source;

        public StatModifier(float value, StatModifierType modifierType, int order, object source){
            Value = value;
            ModifierType = modifierType;
            Order = order;
            Source = source;
        }

        public StatModifier(float value, StatModifierType modifierType) : this (value, modifierType, (int)modifierType) { }
        public StatModifier(float value, StatModifierType modifierType, int order) : this (value, modifierType, order, null) { }
        public StatModifier(float value, StatModifierType modifierType, object source) : this (value, modifierType, (int)modifierType, source) { }
    }
}