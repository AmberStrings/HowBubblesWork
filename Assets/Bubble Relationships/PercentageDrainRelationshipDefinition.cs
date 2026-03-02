using UnityEngine;
using static Bubble;

[CreateAssetMenu(fileName = "New Percentage Drain Relationship.asset", menuName = "Bubble Relationships/New Percentage Drain Relationship", order = 0)]
public class PercentageDrainRelationshipDefinition : BubbleRelationshipDefinition
{
    public ResourceKind ResourceKindToTransfer;
    public double PercentageAmountAGivesPerSecond;
    public double HighestAmountAWillGivePerSecond;

    public override void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, double time)
    {
        double currentResourceCount = bubbleA.GetResource(this.ResourceKindToTransfer);
        double amountToDrain = (double)Mathf.Min((float)currentResourceCount, (float)this.HighestAmountAWillGivePerSecond * (float)time, (float)this.PercentageAmountAGivesPerSecond * (float)time * (float)currentResourceCount);
        bubbleA.ModifyResource(this.ResourceKindToTransfer, -amountToDrain);
        bubbleB.ModifyResource(this.ResourceKindToTransfer, amountToDrain);
    }
}
