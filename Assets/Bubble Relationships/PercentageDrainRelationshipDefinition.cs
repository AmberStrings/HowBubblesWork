using UnityEngine;
using static Bubble;

[CreateAssetMenu(fileName = "New Percentage Drain Relationship.asset", menuName = "Bubble Relationships/New Percentage Drain Relationship", order = 0)]
public class PercentageDrainRelationshipDefinition : BubbleRelationshipDefinition
{
    public ResourceTypeTarget ResourceTypeToTransfer;
    public float PercentageAmountAGivesPerSecond;
    public float HighestAmountAWillGivePerSecond;

    public override void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, float time)
    {
        float currentResourceCount = bubbleA.GetResource(this.ResourceTypeToTransfer);
        float amountToDrain = Mathf.Min(currentResourceCount, this.HighestAmountAWillGivePerSecond * time, this.PercentageAmountAGivesPerSecond * time * currentResourceCount);
        bubbleA.ModifyResource(this.ResourceTypeToTransfer, -amountToDrain);
        bubbleB.ModifyResource(this.ResourceTypeToTransfer, amountToDrain);
    }
}
