using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
public class BallController : MonoBehaviour
{
    // Topun hızını ayarlar.
    [SerializeField] private float thrust = 250f;
    
    // Topa fizik ekler.
    [SerializeField] private Rigidbody rb;

    // Yolun genişliğinin yarısı
    [SerializeField] private float wallDistance = 5f;

    // Topun kameraya minimum uzaklığı
    [SerializeField] private float minCamDistance = 5f;

    // Farenin sürüklendiği 2 boyutlu vektör konumu
    // Her frame de update fonksiyonunda kullanılacağı için Update fonksiyonunun dışında tanımlandı.
    private Vector2 lastMousePos;

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Game sadece bir kez başlatılsın diye tutulan basit bir sayaç 
    int birkez = 0;
    
    // Update is called once per frame
    void Update()
    {
        Vector2 deltaPos = Vector2.zero;//Vector2(0,0) değeri

        //Fare sol tuşu veya ekrana basılı tutulunca true olur.
        if (Input.GetMouseButton(0))// Sol buton(0), sağ buton(1)
        {
            if (birkez == 0) 
            {
                GameManager.singleton.StartGame();
                birkez++;
            } 

            Vector2 currentMousePos = Input.mousePosition;

            // Farenin son konumunu atadık. 
            if (lastMousePos == Vector2.zero)
            {
                lastMousePos = currentMousePos;
            }

            // Aradak fark(uygulanack kuvvetin yönü)
            deltaPos = currentMousePos - lastMousePos;

            // Farenin son konumu her frame de güncellenir.
            lastMousePos = currentMousePos;

            // Top 3d dünyada bu yüzden, kuvvet vektörü 3 boyutlu vektör
            // Kuvvet vektörü x ve z düzleminde topa kuvvet uygular. 
            // y=0 çünkü topu havaya kaldırmaya gerek yok.
            // 2 boyuttaki deltaPos vektörünün y değeri, 3 boyutta force vektörünün z yönüne denk gelir. 
            Vector3 force = new Vector3(deltaPos.x, 0, deltaPos.y) * thrust;

            //Kuvvet uygulanır.
            rb.AddForce(force);
        }

        else
        {
            lastMousePos = Vector2.zero;
        }
    }

    // Topa aktif olarak kuvvet uygulanmasas bile pasif olarak sabit bir kuvvet etki eder.
    private void FixedUpdate()
    {
        if (GameManager.singleton.GameEnded)
        {
            return;
        }
        if (GameManager.singleton.GameStarted)
        {
            rb.MovePosition(transform.position + Vector3.forward * 5 * Time.fixedDeltaTime); 
        }
    }

    // Late update içindeki olaylar olmadan önce bütün fizik işlemleri hesaplanmıştır. 
    // Hesaplanmalardan sonra bu fonksiyon hareket hesaplanmaları için kullanılmıştır.
    // Bu yüzden late update kullanıldı.
    private void LateUpdate()
    {
        // Topun pozisyonu
        Vector3 pos = transform.position;

        // Top sol veya sağ duvara 5 birimden fazla yaklaşırsa, topun merkezden uzaklığı, 
        // yolun genişliğinin yarısına sabitlenir.

        if (transform.position.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        else if (transform.position.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        // Top kameranın arkasında kalırsa topun z konumu
        // kameranın + minimum kamera yakınlığının z pozisyonuna sabitlenir.
        // Böylece oyuncu topu hareket ettirmese bile top kameradan geride kalmayacak.
        if (transform.position.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
            transform.position = pos;
        }
        
        // Yeni koordinatlar topun pozisyonuna uygulanır
        transform.position = pos;
    }

    int beyazcarpisma = 0;
    int saricarpisma = 0;
    // Top çarpıştığında
    // Collision parametresindeki şey çarpıştığı objedir.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Zemin")
        {
            if(collision.gameObject.tag == "Death")
            {
                saricarpisma++;
                Debug.Log("Sarıya " + saricarpisma + " kez çarptı.");
            }else
            {
                beyazcarpisma++;
                Debug.Log("Beyaza " + beyazcarpisma + " kez çarptı.");
            }
        }

        // Oyun bitmişse bir şey yapma
        if (GameManager.singleton.GameEnded)
        {
            return;
        }

        // Sarı tolara çarparsa oyun biter.
        if (collision.gameObject.tag == "Death")
        {
            GameManager.singleton.EndGame(false);
        }
    }
}