using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Bubble : MonoBehaviour
{
    public const float MINIMUMBUBBLESIZETOPOP = .001f;

    static readonly HashSet<Bubble> Bubbles = new HashSet<Bubble>();

    public BubbleDefinition MyDefinition { get; private set; }

    public SpriteRenderer BubbleRenderer;

    public AnimationCurve ForcePerDistance;
    public Rigidbody2D MyRigidbody;
    public float ForceMultiplier = 3f;
    public float MassToAttractionForceMultiplier = 1f;

    public float Size { get; private set; } = 1f;

    public bool IAmBeingDragged { get; private set; } = false;

    public LineRenderer MyBaseLineRenderer;

    public void SetFromDefinition(BubbleDefinition myDefinition)
    {
        this.MyDefinition = myDefinition;
        this.transform.localScale = Vector3.one * this.MyDefinition.StartingBubbleSize;
        this.BubbleRenderer.color = this.MyDefinition.BubbleColor;
        this.Size = MyDefinition.StartingBubbleSize;
        this.MyRigidbody.mass = this.MyDefinition.StartingBubbleSize * this.MyDefinition.MassPerSize;
        this.name = this.MyDefinition.name;

        this.MyBaseLineRenderer.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Bubbles.Add(this);
    }

    private void OnDisable()
    {
        Bubbles.Remove(this);
    }

    private void Update()
    {
        if (this.Size <= MINIMUMBUBBLESIZETOPOP)
        {
            this.Pop();
            return;
        }

        Vector3 myPosition = this.transform.position;

        if (this.IAmBeingDragged)
        {
            if (!Mouse.current.leftButton.IsPressed())
            {
                this.OnEndDrag();
            }
            else
            {
                this.MyBaseLineRenderer.SetPositions(new Vector3[] { myPosition, Camera.main.ScreenToWorldPoint(Mouse.current.position.value) });
            }
        }
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
    public void OnBeginDrag(BaseEventData dragEventArgs)
    {
        this.IAmBeingDragged = true;
        this.MyBaseLineRenderer.gameObject.SetActive(true);
    }

    public void OnEndDrag()
    {
        this.IAmBeingDragged = false;
        this.MyBaseLineRenderer.gameObject.SetActive(false);

        // Is there a Bubble under the cursor? If so, link these two Bubbles
        Vector2 screenPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        Collider2D point = Physics2D.OverlapPoint(screenPoint, 1 << this.gameObject.layer);
        if (point == null)
        {
            return;
        }

        Bubble foundBubble = point.gameObject.GetComponent<Bubble>();
        if (foundBubble == null)
        {
            return;
        }

        BubbleRelationshipManager.Instance.AddRelationship(this, foundBubble);
    }

    public void ModifyMass(float amount)
    {
        if (this.transform.localScale.x <= amount)
        {
            // TODO: This should die!
            return;
        }

        float newScaleUnclamped = this.transform.localScale.x + amount;
        this.Size = newScaleUnclamped;
        this.transform.localScale = Vector3.one * newScaleUnclamped;
        this.MyRigidbody.mass = this.MyDefinition.MassPerSize * newScaleUnclamped;
    }

    public void Pop()
    {
        BubbleRelationshipManager.Instance.OnBubblePop(this);
        Destroy(this.gameObject);
    }
}
