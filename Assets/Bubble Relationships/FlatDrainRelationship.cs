using UnityEngine;

[CreateAssetMenu(fileName = "New Flat Drain Relationship.asset", menuName = "Bubble Relationships/New Flat Drain Relationship", order = 0)]
public class FlatDrainRelationship : BubbleRelationshipDefinition
{
    public float AmountThatAGivesBPerSecond;

    public override void ExecuteRelationship(Bubble bubbleA, Bubble bubbleB, float time)
    {
        float amountToDrain = Mathf.Min(bubbleA.Size, this.AmountThatAGivesBPerSecond * time);
        bubbleA.ModifyMass(-amountToDrain);
        bubbleB.ModifyMass(amountToDrain);
    }
}
