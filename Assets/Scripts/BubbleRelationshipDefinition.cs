using System.Collections.Generic;
using UnityEngine;

public abstract class BubbleRelationshipDefinition : ScriptableObject
{
    public BubbleDefinition DefinitionA;
    public BubbleDefinition DefinitionB;

    public List<Condition> Conditions = new List<Condition>();

    public abstract void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, double time);

    public virtual bool ShouldApply(Bubble bubbleA, Bubble bubbleB)
    {
        foreach (Condition condition in this.Conditions)
        {
            if (!condition.ConditionMet(bubbleA))
            {
                return false;
            }
        }

        return true;
    }
}
