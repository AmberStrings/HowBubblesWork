using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public Bubble BubblePF;
    public List<BubbleDefinition> DefinitionsToSpawn = new List<BubbleDefinition>();

    public float DistanceFromCenterToSpawnMax = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (BubbleDefinition curDefinition in this.DefinitionsToSpawn)
        {
            Bubble newBubble = Instantiate(this.BubblePF, this.transform);
            newBubble.transform.position = Random.insideUnitCircle * this.DistanceFromCenterToSpawnMax;
            newBubble.SetFromDefinition(curDefinition);
        }
    }

    public void OnSpawnBubblePress(BubbleDefinition spawnThis)
    {
        Bubble newBubble = Instantiate(this.BubblePF, this.transform);
        newBubble.transform.position = Random.insideUnitCircle * this.DistanceFromCenterToSpawnMax;
        newBubble.SetFromDefinition(spawnThis);
    }
}
