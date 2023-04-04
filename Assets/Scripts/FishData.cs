using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishData : MonoBehaviour
{
    [SerializeField] GameObject[] Fishes;


    int[] Fish_Probability;

    private void Awake()
    {
        Fish_Probability = new int[Fishes.Length];
        for (int i = 0; i < Fish_Probability.Length; i++)
        {
            Fish_Probability[i] = Fishes[i].GetComponent<FishController>().WeightDistribution;
        }

    }

    public GameObject[] getFishes() {
        return Fishes;
    }

    public int GetRandomFishIndex() {
        int totalWeight = 0;
       
        for (int i = 0; i < Fish_Probability.Length ; i++)
        {
            totalWeight += Fish_Probability[i];
        }

        float randomValue = Random.Range(0, totalWeight);
        for (int i = 0; i < Fish_Probability.Length ; i++)
        {
            if (randomValue < Fish_Probability[i])
                return i;
            else
                randomValue -= Fish_Probability[i];
        }
        return 0;
    }
}
