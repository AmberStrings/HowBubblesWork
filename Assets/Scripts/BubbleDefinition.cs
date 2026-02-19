using UnityEngine;

[CreateAssetMenu(fileName = "New Bubble.asset", menuName = "Create New Bubble", order = 0)]
public class BubbleDefinition : ScriptableObject
{
    public Color BubbleColor;
    public float StartingBubbleSize;
    public float MassPerSize;
}
