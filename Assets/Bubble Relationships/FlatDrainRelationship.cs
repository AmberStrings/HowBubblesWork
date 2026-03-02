using System.Collections.Generic;
using UnityEngine;
using static Bubble;

[CreateAssetMenu(fileName = "New Flat Drain Relationship.asset", menuName = "Bubble Relationships/New Flat Drain Relationship", order = 0)]
public class FlatDrainRelationship : BubbleRelationshipDefinition
{
    public List<ResourceAmount> ResourceChangeAmountForBubbleA;
    public List<ResourceAmount> ResourceChangeAmountForBubbleB;

    public override void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, double time)
    {
        foreach (ResourceAmount resourceChangeForBubbleA in this.ResourceChangeAmountForBubbleB)
        {
            if (resourceChangeForBubbleA.AmountOfCapital != 0)
            {
                bubbleA.ModifyCapital(resourceChangeForBubbleA.AmountOfCapital * time);
            }
            if (resourceChangeForBubbleA.OfResourceKind != null)
            {
                bubbleA.ModifyResource(resourceChangeForBubbleA.OfResourceKind, resourceChangeForBubbleA.AmountOfResource * time);
            }
        }

        foreach (ResourceAmount resourceChangeForBubbleB in this.ResourceChangeAmountForBubbleB)
        {
            if (resourceChangeForBubbleB.AmountOfCapital != 0)
            {
                bubbleB.ModifyCapital(resourceChangeForBubbleB.AmountOfCapital * time);
            }
            if (resourceChangeForBubbleB.OfResourceKind != null)
            {
                bubbleB.ModifyResource(resourceChangeForBubbleB.OfResourceKind, resourceChangeForBubbleB.AmountOfResource * time);
            }
        }
    }
}
