using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Changed from UnityEngine.UI

public class OrderUIManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private OrderSystem orderSystem;

    [Header("Canvas 1: Shop Status (Wall)")]
    [SerializeField] private TMP_Text dayText;       // Changed to TMP_Text
    [SerializeField] private TMP_Text progressText;

    [Header("Canvas 2: Current Task (Anvil/Station)")]
    [SerializeField] private TMP_Text currentTaskText;

    [Header("Canvas 3: Order Scroll (Clipboard)")]
    [SerializeField] private TMP_Text orderListText;

    // Track state to avoid updating text every single frame (optimization)
    private int lastCompletedCount = -1;
    private int lastDayCount = -1;

    void Start()
    {
        if (orderSystem == null)
            orderSystem = FindObjectOfType<OrderSystem>();

        // Force an update at the start
        UpdateAllCanvases();
    }

    void Update()
    {
        // Check if data changed. If it did, update the UI.
        if (orderSystem.GetCompletedCount() != lastCompletedCount ||
            orderSystem.GetDayCount() != lastDayCount)
        {
            UpdateAllCanvases();
        }
    }

    void UpdateAllCanvases()
    {
        // update trackers
        lastCompletedCount = orderSystem.GetCompletedCount();
        lastDayCount = orderSystem.GetDayCount();

        // --- CANVAS 1: GENERAL INFO ---
        if (dayText != null)
            dayText.text = "Day: " + orderSystem.GetDayCount();

        if (progressText != null)
            progressText.text = $"Progress: {orderSystem.GetCompletedCount()} / {orderSystem.getNumOrders()}";


        // --- CANVAS 2: CURRENT ACTIVE ORDER ---
        if (currentTaskText != null)
        {
            // If we have finished all orders, say "Done"
            if (orderSystem.GetCompletedCount() >= orderSystem.getNumOrders())
            {
                currentTaskText.text = "Shop Closed!\nGood Job.";
                currentTaskText.color = Color.green;
            }
            else
            {
                // Get quality of the specific order we are working on (index = completedOrders)
                int currentQ = orderSystem.GetCurrentOrderQuality(orderSystem.GetCompletedCount());

                // Make stars string (e.g. ***)
                string stars = "";
                for (int i = 0; i < currentQ; i++) stars += "?";

                // TextMeshPro supports rich text tags like <size> and <color> natively
                currentTaskText.text = $"NEXT ORDER:\nQuality {currentQ}\n<size=150%><color=#FFD700>{stars}</color></size>";
                currentTaskText.color = Color.white;
            }
        }


        // --- CANVAS 3: THE LIST ---
        if (orderListText != null)
        {
            string listBuilder = "Today's Orders:\n";
            List<int> orders = orderSystem.GetOrderList();
            int currentParamsIndex = orderSystem.GetCompletedCount();

            for (int i = 0; i < orders.Count; i++)
            {
                int qualityReq = orders[i];
                string stars = new string('?', qualityReq);

                if (i < currentParamsIndex)
                {
                    // Finished orders (Green strikethrough using TMP tags)
                    listBuilder += $"<s><color=green>[DONE] Sword (Qual {qualityReq})</color></s>\n";
                }
                else if (i == currentParamsIndex)
                {
                    // Current order (Highlight Yellow/Bold)
                    listBuilder += $"<b><color=yellow>>> MAKE THIS: Qual {qualityReq} {stars}</color></b>\n";
                }
                else
                {
                    // Future orders (Greyed out slightly)
                    listBuilder += $"<color=#AAAAAA>[ ] Sword (Qual {qualityReq})</color>\n";
                }
            }
            orderListText.text = listBuilder;
        }
    }
}