using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BubbleRelationshipManager : MonoBehaviour
{
    public static BubbleRelationshipManager Instance { get; private set; }
    private List<LinkBetweenBubbles> ActiveLinks { get; set; } = new List<LinkBetweenBubbles>();
    public LineRenderer LineRendererPF;

    [SerializeField]
    private List<BubbleRelationshipDefinition> BubbleRelationshipDefinitions = new List<BubbleRelationshipDefinition>();

    private void OnEnable()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Suspiciously, there was another BubbleRelationshipManager instance...?");
        }

        Instance = this;
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void AddRelationship(Bubble bubbleA, Bubble bubbleB)
    {
        LineRenderer newRenderer = Instantiate(this.LineRendererPF);
        newRenderer.gameObject.SetActive(true);

        this.ActiveLinks.Add(new LinkBetweenBubbles(bubbleA, bubbleB, BubbleRelationshipDefinitions, newRenderer));
    }

    private void Update()
    {
        foreach (LinkBetweenBubbles linkBetween in this.ActiveLinks)
        {
            linkBetween.MyLineRenderer.SetPositions(new Vector3[] { linkBetween.Bubbles[0].transform.position, linkBetween.Bubbles[1].transform.position });
        }
    }

    private void FixedUpdate()
    {
        float time = Time.deltaTime;

        foreach (LinkBetweenBubbles link in this.ActiveLinks)
        {
            foreach (BubbleRelationshipInstance relationships in link.RelationshipInstances)
            {
                relationships.BubbleRelationDefinition.ExecuteRelationship(relationships.BubbleA, relationships.BubbleB, time);
            }
        }
    }

    public void OnBubblePop(Bubble poppingBubble)
    {
        for (int ii = this.ActiveLinks.Count - 1; ii >= 0; ii--)
        {
            LinkBetweenBubbles thisLink = this.ActiveLinks[ii];
            if (thisLink.Bubbles.Contains(poppingBubble))
            {
                Destroy(thisLink.MyLineRenderer.gameObject);
            }
            this.ActiveLinks.RemoveAt(ii);
        }
    }
}
