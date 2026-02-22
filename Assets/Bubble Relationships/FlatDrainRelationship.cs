using UnityEngine;
using static Bubble;

[CreateAssetMenu(fileName = "New Flat Drain Relationship.asset", menuName = "Bubble Relationships/New Flat Drain Relationship", order = 0)]
public class FlatDrainRelationship : BubbleRelationshipDefinition
{
    public ResourceTypeTarget ResourceTypeToTransfer;
    public float AmountThatAGivesBPerSecond;

    public override void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, float time)
    {
        float amountToDrain = Mathf.Min(bubbleA.GetResource(this.ResourceTypeToTransfer), this.AmountThatAGivesBPerSecond * time);
        bubbleA.ModifyResource(this.ResourceTypeToTransfer, -amountToDrain);
        bubbleB.ModifyResource(this.ResourceTypeToTransfer, amountToDrain);
    }
}
