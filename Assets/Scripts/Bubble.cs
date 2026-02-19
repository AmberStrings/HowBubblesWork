using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    static readonly HashSet<Bubble> Bubbles = new HashSet<Bubble>();

    public BubbleDefinition MyDefinition { get; private set; }

    public SpriteRenderer BubbleRenderer;

    public AnimationCurve ForcePerDistance;
    public Rigidbody2D MyRigidbody;
    public float ForceMultiplier = 3f;
    public float MassToAttractionForceMultiplier = 1f;

    public void SetFromDefinition(BubbleDefinition myDefinition)
    {
        this.MyDefinition = myDefinition;
        this.transform.localScale = Vector3.one * this.MyDefinition.StartingBubbleSize;
        this.BubbleRenderer.color = this.MyDefinition.BubbleColor;
        this.MyRigidbody.mass = this.MyDefinition.StartingBubbleSize * this.MyDefinition.MassPerSize;
        this.name = this.MyDefinition.name;
    }

    private void OnEnable()
    {
        Bubbles.Add(this);
    }

    private void OnDisable()
    {
        Bubbles.Remove(this);
    }

    private void FixedUpdate()
    {
        if (Bubbles.Count <= 1)
        {
            return;
        }

        Vector2 myPos = this.transform.position;
        foreach (Bubble bubble in Bubbles)
        {
            Vector2 differenceInPosition = myPos - (Vector2)bubble.transform.position;
            float forceByDistance = this.ForcePerDistance.Evaluate(differenceInPosition.magnitude);

            if (!Mathf.Approximately(forceByDistance, 0))
            {
                this.MyRigidbody.AddForce(differenceInPosition.normalized * forceByDistance * ForceMultiplier
                    * bubble.MyRigidbody.mass * MassToAttractionForceMultiplier);
            }
        }
    }
}
