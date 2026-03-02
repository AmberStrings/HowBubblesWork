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
        foreach (ResourceAmount resourceChangeForBubbleA in this.ResourceChangeAmountForBubbleA)
        {
            if (resourceChangeForBubbleA.AmountOfCapital != 0)
            {
                bubbleA.ModifyCapital(resourceChangeForBubbleA.AmountOfCapital * (resourceChangeForBubbleA.Instant ? 1f : Time.deltaTime));
            }
            if (resourceChangeForBubbleA.OfResourceKind != null)
            {
                bubbleA.ModifyResource(resourceChangeForBubbleA.OfResourceKind, resourceChangeForBubbleA.AmountOfResource * (resourceChangeForBubbleA.Instant ? 1f : Time.deltaTime));
            }
        }

        foreach (ResourceAmount resourceChangeForBubbleB in this.ResourceChangeAmountForBubbleB)
        {
            if (resourceChangeForBubbleB.AmountOfCapital != 0)
            {
                bubbleB.ModifyCapital(resourceChangeForBubbleB.AmountOfCapital * (resourceChangeForBubbleB.Instant ? 1f : Time.deltaTime));
            }
            if (resourceChangeForBubbleB.OfResourceKind != null)
            {
                bubbleB.ModifyResource(resourceChangeForBubbleB.OfResourceKind, resourceChangeForBubbleB.AmountOfResource * (resourceChangeForBubbleB.Instant ? 1f : Time.deltaTime));
            }
        }
    }

    public override bool ShouldApply(Bubble bubbleA, Bubble bubbleB)
    {
        if (!base.ShouldApply(bubbleA, bubbleB))
        {
            return false;
        }

        foreach (ResourceAmount resourceChange in this.ResourceChangeAmountForBubbleA)
        {
            if (resourceChange.AmountOfCapital < 0 && bubbleA.CapitalAmount < -resourceChange.AmountOfCapital * (resourceChange.Instant ? 1f : Time.deltaTime))
            {
                return false;
            }
            if (resourceChange.OfResourceKind != null && resourceChange.AmountOfResource < 0 && bubbleA.GetResource(resourceChange.OfResourceKind) < -resourceChange.AmountOfResource * (resourceChange.Instant ? 1f : Time.deltaTime))
            {
                return false;
            }
        }

        foreach (ResourceAmount resourceChange in this.ResourceChangeAmountForBubbleB)
        {
            if (resourceChange.AmountOfCapital < 0 && bubbleB.CapitalAmount < -resourceChange.AmountOfCapital * (resourceChange.Instant ? 1f : Time.deltaTime))
            {
                return false;
            }
            if (resourceChange.OfResourceKind != null && resourceChange.AmountOfResource < 0 && bubbleB.GetResource(resourceChange.OfResourceKind) < -resourceChange.AmountOfResource * (resourceChange.Instant ? 1f : Time.deltaTime))
            {
                return false;
            }
        }

        return true;
    }
}
