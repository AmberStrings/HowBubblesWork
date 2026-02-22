using UnityEngine;

public abstract class BubbleRelationshipDefinition : ScriptableObject
{
    public BubbleDefinition DefinitionA;
    public BubbleDefinition DefinitionB;

    public abstract void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, float time);
}
