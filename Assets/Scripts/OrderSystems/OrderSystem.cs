using Oculus.Movement.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField] private int dayCount = 0;
    [SerializeField] private int numOrders = 0;
    [SerializeField] private int[] quality = { 1, 2, 3 };
    [SerializeField] private bool Finished = false;

    private List<int> currentOrders = new List<int>();
    [SerializeField] private int completeOrders = 0;

    void Start()
    {
        dayCount = 0;
        completeOrders = 0;
        orderGen();
    }
    
    void Update()
    {
        // Check if all orders are completed (use >= instead of ==)
        if (numOrders > 0 && completeOrders >= numOrders)
        {
            Finished = true;
            StartCoroutine(NextDayDelay());
            nextDay();
        }
    }

    void orderGen()
    {
        currentOrders.Clear(); // Clear old orders
        completeOrders = 0;     // Reset completed count

        if (dayCount == 0)
        {
            // Tutorial Level
            numOrders = 1;
            currentOrders.Add(1);
        }
        else if (dayCount == 1)
        {
            numOrders = 2;
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 3)); // Quality 1-2
            }
        }
        else if (dayCount == 2)
        {
            numOrders = 4;
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 4)); // Quality 1-3
            }
        }
        else
        {
            // Generate random orders
            numOrders = Random.Range(4, 10);
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 4)); // Quality 1-3
            }
        }

        Debug.Log($"Day {dayCount + 1} - Orders: {numOrders} - Quality Reqs: {string.Join(", ", currentOrders)}");
    }

    void nextDay()
    {
        dayCount++;
        Finished = false;
        orderGen();
    }

    public void CompleteOrder()
    {
        completeOrders++;
        Debug.Log($"Completed {completeOrders}/{numOrders} orders on Day {dayCount + 1}");
    }

    // Get methods for UI
    public bool getFinished() { return Finished; }
    public int GetDayCount()
    {
        return dayCount;
    }

    public int GetNumOrders()
    {
        return numOrders;
    }

    public int GetCompletedOrders()
    {
        return completeOrders;
    }

    public int GetOrderQuality(int orderIndex)
    {
        if (orderIndex < currentOrders.Count)
            return currentOrders[orderIndex];
        return 1;
    }
    private IEnumerator NextDayDelay()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to let other scripts check Finished
        nextDay();
    }
    public List<int> GetAllOrderQualities()
    {
        return new List<int>(currentOrders); // Return a copy
    }
}