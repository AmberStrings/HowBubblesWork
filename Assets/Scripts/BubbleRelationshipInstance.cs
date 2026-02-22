using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleRelationshipInstance
{
    public readonly Bubble BubbleA;
    public readonly Bubble BubbleB;
    public readonly BubbleRelationshipDefinition BubbleRelationDefinition;

    public BubbleRelationshipInstance(Bubble bubbleA, Bubble bubbleB, BubbleRelationshipDefinition bubbleRelationshipDefinition)
    {
        this.BubbleA = bubbleA;
        this.BubbleB = bubbleB;
        this.BubbleRelationDefinition = bubbleRelationshipDefinition;
    }
}
