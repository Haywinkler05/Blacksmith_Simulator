using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellSword : MonoBehaviour
{
    [SerializeField] private OrderSystem orderSystem;
    [SerializeField] private PlayerMoney playerEcon;
    [SerializeField] private GamePhase gamePhase;

    [SerializeField] private int lowQualityPrice = 10;
    [SerializeField] private int mediumQualityPrice = 25;
    [SerializeField] private int highQualityPrice = 50;

    private bool hasReceivedPayment = false; // ADD THIS FLAG


    
    void Update()
    {
        if (orderSystem.getFinished() && !hasReceivedPayment)
        {
            PayPlayerForOrders();
            hasReceivedPayment = true; // Prevent multiple payments
        }

        // Reset flag when new day starts
        if (!orderSystem.getFinished() && hasReceivedPayment)
        {
            hasReceivedPayment = false;
        }
    }

    private void PayPlayerForOrders()
    {
        // Get all completed orders and pay for each
        List<int> orders = orderSystem.GetAllOrderQualities();
        int totalEarnings = 0;

        for (int i = 0; i < orderSystem.GetCompletedOrders(); i++)
        {
            int quality = orders[i];
            int payment = GetPaymentForQuality(quality);
            totalEarnings += payment;
        }

        playerEcon.increasePlayerMoney(totalEarnings);
        Debug.Log($"Day complete! Earned ${totalEarnings} for {orderSystem.GetCompletedOrders()} swords");
    }

    private int GetPaymentForQuality(int qualityLevel)
    {
        switch (qualityLevel)
        {
            case 1: return lowQualityPrice;
            case 2: return mediumQualityPrice;
            case 3: return highQualityPrice;
            default: return 0;
        }
    }
}