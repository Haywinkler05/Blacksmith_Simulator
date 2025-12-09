using Oculus.Movement.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int dayCount = 0;
    [SerializeField] private int numOrders = 0;
    [SerializeField] private int[] quality = { 1, 2, 3 };
    [SerializeField] private bool finishedAllOrders = false;

    private List<int> currentOrders = new List<int>();
    [SerializeField]private int completeOrders = 0;
    void Start()
    {
        dayCount = 0;
        orderGen();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentOrders.Count == completeOrders)
        {
            finishedAllOrders = true;
        }
        if (finishedAllOrders) { 
            nextDay();
            finishedAllOrders= false;
        
        }
    }
    public int getDay(int day){return day;}

    public int getNumOrders() { return numOrders;}

    void orderGen()
    {
        currentOrders.Clear();
        if (dayCount == 0)
        {
            //Tutorial Level
            numOrders = 1;
            //Will need to output instructions somewhere for each mini game
            currentOrders.Add(1);



        }else if(dayCount == 1){
            numOrders = 2;
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 3)); // Quality 1-2

            }

        }
        else if(dayCount  == 2){
            numOrders = 4;
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 4)); // Quality 1-3
            }
        }
        else
        {
            //Generate random orders for the num order
            numOrders = Random.Range(4, 10);
            for (int i = 0; i < numOrders; i++)
            {
                currentOrders.Add(Random.Range(1, 4)); // Quality 1-3
            }
        }

        Debug.Log($"Day {dayCount} - Orders: {numOrders} - Quality Reqs: {string.Join(", ", currentOrders)}");
    
    }

    void nextDay()
    {
        dayCount++;
        orderGen();
    }
    public void CompleteOrder()
    {
        completeOrders++;
        Debug.Log($"Completed {completeOrders}/{numOrders} orders");
    }
    public int GetCurrentOrderQuality(int orderIndex)
    {
        if (orderIndex < currentOrders.Count)
            return currentOrders[orderIndex];
        return 1; 
    }

    public int GetDayCount() { return dayCount; }

    // Logic to let UI know how many we have finished
    public int GetCompletedCount() { return completeOrders; }

    // Logic to let UI see the whole list of orders
    public List<int> GetOrderList() { return currentOrders; }
}
