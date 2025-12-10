using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSwordScript : MonoBehaviour
{
    [SerializeField] OrderSystem complete;
    [SerializeField] GamePhase gamePhase;
    
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
                complete.CompleteOrder();
                other.gameObject.SetActive(false);
                if (gamePhase.Finish == 1)
                {
                  
                }
                
            }
        }
    }
}
  
