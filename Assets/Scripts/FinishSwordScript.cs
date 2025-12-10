using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinishSwordScript : MonoBehaviour
{
    [SerializeField] OrderSystem complete;
    [SerializeField] SpawnSword spawn;
    [SerializeField] QualityCalculator quality;

    [SerializeField] float score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {

            if (other.gameObject.CompareTag("Sword"))
            {
                score = quality.CalculateScore();
                int qualityCheck = quality.qualityLookup(score);
                int index = complete.GetCompletedOrders();
                int cmp = complete.GetOrderQuality(index);
                if (cmp <= qualityCheck) {
                    complete.CompleteOrder();
                    spawn.currentSpawnedSword = null;
                    Destroy(other.gameObject);
                    GamePhase.Instance.SetPhaseForge();
                }
              
                
                
            }
        }
    }
}
  
