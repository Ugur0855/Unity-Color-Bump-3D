using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Çizgiye çarpan şeyin bileşen tipini alıyoruz.
        BallController ball = other.GetComponent<BallController>();

        // Boş bol değilse veya oyun bitmişse bir şey yapma 
        if(!ball || GameManager.singleton.GameEnded)
        {
            return;
        }

        Debug.Log("Çizgi bitti.");

        GameManager.singleton.EndGame(true);
    }
}