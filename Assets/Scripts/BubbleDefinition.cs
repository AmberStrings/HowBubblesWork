using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bubble.asset", menuName = "Create New Bubble", order = 0)]
public class BubbleDefinition : ScriptableObject
{
    public Color BubbleColor;
    public double MassPerSize;

    public List<ResourceAmount> ResourceAmounts = new List<ResourceAmount>();

    public List<ResourceAmount> SelfAffectingTransactions = new List<ResourceAmount>();
}
