using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Image fillBarProgress;

    private float lastValue;
    // Update is called once per frame
    void Update()
    {
        // Oyun başlamamışsa bir şey yapma
        if (!GameManager.singleton.GameStarted)
        {
            return;
        }

        float travelledDistance = GameManager.singleton.EntireDistance - GameManager.singleton.DistanceLeft;
        float value = travelledDistance / GameManager.singleton.EntireDistance;
    
        // Oyun bittikten sonra ilerleme çizgisi geriye doğru gitmesin diye return döndürülür.
        if(GameManager.singleton.GameEnded && value < lastValue)
        {
            return;
        }
        
        // İlerleme çizgisinin değerini, deltatime ve ilerleme yüzdesine göre değiştirir.
        fillBarProgress.fillAmount = Mathf.Lerp(fillBarProgress.fillAmount, value, 5 * Time.deltaTime);

        lastValue = value;
    }
}
