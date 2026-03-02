using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Bubble : MonoBehaviour
{
    public enum ResourceTypeTarget
    {
        Resources,
        Capital,
    }

    public const float MINIMUMBUBBLESIZETOPOP = .001f;

    static readonly HashSet<Bubble> Bubbles = new HashSet<Bubble>();

    public BubbleDefinition MyDefinition { get; private set; }

    public SpriteRenderer BubbleRenderer;

    public AnimationCurve ForcePerDistance;
    public Rigidbody2D MyRigidbody;
    public float ForceMultiplier = 3f;
    public float MassToAttractionForceMultiplier = 1f;

    public double CapitalAmount;
    public Dictionary<ResourceKind, double> ResourceToAmount = new Dictionary<ResourceKind, double>();

    public float Size
    {
        get
        {
            double total = this.CapitalAmount;
            foreach (ResourceKind kind in this.ResourceToAmount.Keys)
            {
                total += this.ResourceToAmount[kind];
            }
            return (float)total;
        }
    }


    public bool IAmBeingDragged { get; private set; } = false;

    public LineRenderer MyBaseLineRenderer;

    public TMP_Text ValuesLabel;

    public void SetFromDefinition(BubbleDefinition myDefinition)
    {
        foreach (ResourceAmount amount in myDefinition.ResourceAmounts)
        {
            this.ResourceToAmount.Add(amount.OfResourceKind, amount.AmountOfResource);
            this.CapitalAmount += amount.AmountOfCapital;
        }

        this.MyDefinition = myDefinition;
        float mass = this.GetMass();
        this.BubbleRenderer.transform.localScale = Vector3.one * (mass / (float)this.MyDefinition.MassPerSize);
        this.BubbleRenderer.color = this.MyDefinition.BubbleColor;

        this.MyRigidbody.mass = mass;
        this.name = this.MyDefinition.name;

        this.MyBaseLineRenderer.gameObject.SetActive(false);
        this.UpdateLabel();
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
        double time = Time.deltaTime;

        foreach (ResourceAmount amount in this.MyDefinition.SelfAffectingTransactions)
        {
            if (amount.AmountOfCapital != 0)
            {
                this.ModifyCapital(amount.AmountOfCapital * time);
            }
            if (amount.OfResourceKind != null)
            {
                this.ModifyResource(amount.OfResourceKind, amount.AmountOfResource * time);
            }
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

    public void Pop()
    {
        BubbleRelationshipManager.Instance.OnBubblePop(this);
        Destroy(this.MyBaseLineRenderer.gameObject);
        Destroy(this.gameObject);
    }

    public void SetResourcesAndCapital(Dictionary<ResourceKind, double> resourceValues, double capitalValue)
    {
        this.ResourceToAmount = resourceValues;
        this.CapitalAmount = capitalValue;

        this.BubbleRenderer.transform.localScale = Vector3.one * this.Size;
        this.UpdateLabel();
    }

    public void ModifyCapital(double byAmount)
    {
        this.CapitalAmount = Math.Max(this.CapitalAmount + byAmount, 0);
        float mass = this.GetMass();
        this.BubbleRenderer.transform.localScale = Vector3.one * (mass / (float)this.MyDefinition.MassPerSize);
    }

    public void ModifyResource(ResourceKind type, double byAmount)
    {
        if (this.ResourceToAmount.TryGetValue(type, out double existingAmount))
        {
            this.ResourceToAmount[type] = existingAmount + byAmount;
        }
        else
        {
            this.ResourceToAmount.Add(type, byAmount);
        }

        float mass = this.GetMass();
        this.BubbleRenderer.transform.localScale = Vector3.one * (mass / (float)this.MyDefinition.MassPerSize);
        this.UpdateLabel();
    }

    public double GetResource(ResourceKind type)
    {
        if (this.ResourceToAmount.TryGetValue(type, out double value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    public void UpdateLabel()
    {
        this.ValuesLabel.text = $"{this.CapitalAmount.ToString("F2")}C\n{this.GetResourcesLabel()}";
    }

    public string GetResourcesLabel()
    {
        StringBuilder resourcesLabel = new StringBuilder();

        foreach (ResourceKind kind in this.ResourceToAmount.Keys)
        {
            resourcesLabel.AppendLine($"[{kind.ResourceName} = {this.ResourceToAmount[kind].ToString("F2")}]");
        }

        return resourcesLabel.ToString();
    }

    public float GetMass()
    {
        double mass = this.CapitalAmount;
        foreach (ResourceKind kind in this.ResourceToAmount.Keys)
        {
            mass += this.ResourceToAmount[kind];
        }
        return (float)mass;
    }
}
