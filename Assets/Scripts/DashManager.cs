using System.Security.Cryptography;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    [SerializeField] private int maxDashes = 3;
    public int currentDashes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDashes = maxDashes;
    }

    public void IncreaseDash(int increaseAmt)
    {
        currentDashes += increaseAmt;

        if (currentDashes >= maxDashes)
        {
            currentDashes = maxDashes;
        }
    }

    public void UseDash()
    {
        currentDashes--;
    }

    public int GetDashCount()
    {
        return currentDashes;
    }
}

