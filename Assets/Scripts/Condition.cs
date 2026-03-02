using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Condition
{
    public enum Operator
    {
        LessThan,
        GreaterThan,
    }

    public Operator ConditionOperator = Operator.LessThan;
    public ResourceAmount Amount;

    public bool ConditionMet(Bubble bubbleA)
    {
        if (this.Amount.AmountOfCapital != 0)
        {
            if (this.ConditionOperator == Operator.LessThan && bubbleA.CapitalAmount >= this.Amount.AmountOfCapital)
            {
                return false;
            }
            else if (this.ConditionOperator == Operator.GreaterThan && bubbleA.CapitalAmount <= this.Amount.AmountOfCapital)
            {
                return false;
            }
        }

        if (this.Amount.OfResourceKind != null)
        {
            if (this.ConditionOperator == Operator.LessThan && bubbleA.GetResource(this.Amount.OfResourceKind) >= this.Amount.AmountOfResource)
            {
                return false;
            }
            else if (this.ConditionOperator == Operator.GreaterThan && bubbleA.GetResource(this.Amount.OfResourceKind) <= this.Amount.AmountOfResource)
            {
                return false;
            }
        }

        return true;
    }
}
