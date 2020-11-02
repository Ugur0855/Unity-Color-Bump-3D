using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    // Yavaş modu zaman süresinin hız sabiti
    [SerializeField] private float slowMotionFactor = 0.1f;

    // Topun başlangıç ve bitiş pozisyonları
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform goalTransform;

    // Topun pozisyonu
    [SerializeField] private BallController ball;

    public float EntireDistance { get; private set; }

    public float DistanceLeft { get; private set; }

    private void Start()
    {
        EntireDistance = goalTransform.position.z - startTransform.position.z;
    }

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }else if(singleton != this){
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    public void StartGame()
    {
        GameStarted = true;
        Debug.Log("Game started.");
    }

    public void EndGame(bool win)
    {
        GameEnded = true;
        Debug.Log("Game Ended.");

        if (!win)
        {
            // 2 saniye sonra oyunu tekrar başlat.
            Invoke("RestartGame", 2 * slowMotionFactor);

            // timeScale yavaş hareket modundaki zaman ölçütü
            Time.timeScale = slowMotionFactor;

            // Fizik ve frame güncellemelerinin yapıldığı süre aralığı
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        else
        {
            Invoke("RestartGame", 2 * slowMotionFactor);
            Time.timeScale = slowMotionFactor;
             
            // Fizik ve frame güncellemelerinin yapıldığı süre aralığı
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        DistanceLeft = Vector3.Distance(ball.transform.position, goalTransform.position);

        if(DistanceLeft > EntireDistance)
        {
            // Henüz başlangıç çizgisi geçilmediği için
            DistanceLeft = EntireDistance;
        }

        // Top bitiş çizgisini geçti
        if(ball.transform.position.z > goalTransform.transform.position.z)
        {
            DistanceLeft = 0;
        }

        Debug.Log("Gidilen yol: " + (EntireDistance - DistanceLeft) + "/" + EntireDistance); 
    }
}
