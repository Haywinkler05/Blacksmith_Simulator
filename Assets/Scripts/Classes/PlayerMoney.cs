using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [Header("Player Money")]
    [SerializeField] private float amount;
   

    public void setAmount(float money){ amount = money;}


    public float getAmount(){return amount;}


    public void decreasePlayerMoney(float price) { amount -= price;}

    public void increasePlayerMoney(float pay) { amount += pay; }
}
