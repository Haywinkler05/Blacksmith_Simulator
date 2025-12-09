using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class UIManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private OrderSystem orderSystem;

    [Header("Canvas 1: Day Status (Wall)")]
    [SerializeField] private TMP_Text dayText;

    [Header("Canvas 2: Number of Orders Status (Wall)")]
    [SerializeField] private TMP_Text numOrder;

    [Header("Canvas 3: Current Order (Wall)")]
    [SerializeField] private TMP_Text quality;

    private void Start()
    {
        if (orderSystem == null)
        {
            orderSystem = FindObjectOfType<OrderSystem>();
        }

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (orderSystem == null) return;

        // Update day text (add 1 for player display - Day 1 instead of Day 0)
        if (dayText != null)
        {
            dayText.text = $"Day: {orderSystem.GetDayCount() + 1}";
        }

        // Update number of orders
        if (numOrder != null)
        {
            numOrder.text = $"Orders: {orderSystem.GetCompletedOrders()}/{orderSystem.GetNumOrders()}";
        }

        // Update current order quality requirement
        if (quality != null)
        {
            int currentOrderIndex = orderSystem.GetCompletedOrders();

            // Check if there are still orders to complete
            if (currentOrderIndex < orderSystem.GetNumOrders())
            {
                int requiredQuality = orderSystem.GetOrderQuality(currentOrderIndex);
                quality.text = $"Required Quality:\n{getQuality(requiredQuality)}";
            }
            else
            {
                quality.text = "All Orders Complete!";
            }
        }
    }

    private string getQuality (int qualityLevel)
    {
        switch (qualityLevel)
        {
            case 1: return "Low Quality";
            case 2: return "Medium Quality";
            case 3: return "High Quality";
            default: return "Unknown";
        }

    }
}