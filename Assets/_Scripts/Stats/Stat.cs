/*
                THANK YOU TO KRYZAREL!!!
                
                TUTORIAL:
                https://www.youtube.com/watch?v=SH25f3cXBVc
*/



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ContradictiveGames.Systems.Stats
{
    public class Stat
    {
        public float BaseValue;
        public bool canNotAugment = false;
        public virtual float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateValue();
                    isDirty = false;
                }
                return _value;
            }
        }
        public virtual int ValueAsInt => (int)Math.Round(Value);

        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;
        protected readonly List<StatModifier> _statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        public Stat()
        {
            _statModifiers = new List<StatModifier>();
            StatModifiers = _statModifiers.AsReadOnly();
        }
        public Stat(float _baseValue) : this()
        {
            BaseValue = _baseValue;
        }
        public Stat(int _baseValue) : this(){
            BaseValue = _baseValue;
        }
        public virtual void AddModifier(StatModifier modifier)
        {
            if (!canNotAugment)
            {
                isDirty = true;
                _statModifiers.Add(modifier);
                _statModifiers.Sort(CompareModifierOrder);
            }
        }
        public virtual bool RemoveModifier(StatModifier modifier)
        {
            if (!canNotAugment)
            {
                if (_statModifiers.Remove(modifier))
                {
                    isDirty = true;
                    return true;
                }
            }
            return false;
        }
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order) return -1;
            else if (a.Order > b.Order) return 1;
            return 0;
        }
        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            if (!canNotAugment)
            {
                for (int i = _statModifiers.Count - 1; i >= 0; i--)
                {
                    if (_statModifiers[i].Source == source)
                    {
                        isDirty = true;
                        didRemove = true;
                        _statModifiers.RemoveAt(i);
                    }
                }
            }
            return didRemove;
        }
        protected virtual float CalculateValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0; // This will hold the sum of our "PercentAdd" modifiers
            for (int i = 0; i < _statModifiers.Count; i++)
            {
                StatModifier modifier = _statModifiers[i];
                if (modifier.ModifierType == StatModifierType.Flat_Add)
                {
                    finalValue += modifier.Value;
                }
                else if (modifier.ModifierType == StatModifierType.Percent_Add) // When we encounter a "PercentAdd" modifier
                {
                    sumPercentAdd += modifier.Value; // Start adding together all modifiers of this type
                    // If we're at the end of the list OR the next modifer isn't of this type
                    if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].ModifierType != StatModifierType.Percent_Add)
                    {
                        finalValue *= 1 + sumPercentAdd; // Multiply the sum with the "finalValue", like we do for "PercentMult" modifiers
                        sumPercentAdd = 0; // Reset the sum back to 0
                    }
                }
                else if (modifier.ModifierType == StatModifierType.Percent_Mult) // Percent renamed to PercentMult
                {
                    finalValue *= 1 + modifier.Value;
                }
            }
            return (float)Math.Round(finalValue, 4);
        }
    }
}