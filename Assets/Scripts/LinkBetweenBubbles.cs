using System;
using System.Collections.Generic;
using UnityEngine;

public class LinkBetweenBubbles
{
    public readonly List<Bubble> Bubbles = new List<Bubble>();
    public readonly HashSet<BubbleRelationshipInstance> RelationshipInstances = new HashSet<BubbleRelationshipInstance>();
    public readonly LineRenderer MyLineRenderer;

    public LinkBetweenBubbles(Bubble bubbleA, Bubble bubbleB, List<BubbleRelationshipDefinition> relationshipsLookup, LineRenderer lineRenderer)
    {
        this.Bubbles.Add(bubbleA);
        this.Bubbles.Add(bubbleB);
        this.MyLineRenderer = lineRenderer;

        foreach (BubbleRelationshipDefinition definition in relationshipsLookup)
        {
            // Check if Bubble A is a match for the first type
            if (bubbleA.MyDefinition == definition.DefinitionA)
            {
                // If it is, then check that the bubbleB lines up with definitionB
                if (bubbleB.MyDefinition == definition.DefinitionB)
                {
                    // We have a hit!
                    this.RelationshipInstances.Add(new BubbleRelationshipInstance(bubbleA, bubbleB, definition));
                }
            }
            // If it's not a match, check if bubbleA lines up with definitionB
            else if (bubbleA.MyDefinition == definition.DefinitionB)
            {
                // If it is, then check that the bubbleB lines up with definitionA
                if (bubbleB.MyDefinition == definition.DefinitionA)
                {
                    // We have a hit!
                    this.RelationshipInstances.Add(new BubbleRelationshipInstance(bubbleB, bubbleA, definition));
                }
            }
        }
    }
}
