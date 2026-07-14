using UnityEngine;
using System.Collections;

public class JellyController : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    public float ileriHiz = 5f;
    public float geriGitmeHizi = 8f;     // Engele çarpýnca ne kadar hýzlý geriye kayacak?
    public float geriGitmeSuresi = 0.5f;   // Kaç saniye boyunca geriye fýrlayacak?

    [Header("Esneme Ayarlari")]
    public float hassasiyet = 0.005f;
    public float minBoy = 0.5f;
    public float maxBoy = 2.5f;
    public float esnemeHizi = 15f;

    private float guncelBoy = 1f;
    private Vector2 ilkDokunusPozisyonu;
    private bool geriyeGidiyor = false;   // Karakterin o an geriye sekip sekmediðini tutar

    void Update()
    {
        // Geriye gitmiyorsak normal þekilde ileri git, geriye gidiyorsak geriye fýrla
        if (!geriyeGidiyor)
        {
            // 1. Karakteri dümdüz ileri götürür
            transform.Translate(Vector3.forward * ileriHiz * Time.deltaTime);
        }
        else
        {
            // Engele çarptýysa geriye doðru fýrla
            transform.Translate(Vector3.back * geriGitmeHizi * Time.deltaTime);
        }

        // 2. Ekrandaki sürüklemeyi yakalar
        KullaniciGirisiniKontrolEt();

        // 3. Karakteri jel gibi boyutlandýrýr
        JeliBoyutlandir();
    }

    void KullaniciGirisiniKontrolEt()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ilkDokunusPozisyonu = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 guncelDokunusPozisyonu = Input.mousePosition;
            float farkY = guncelDokunusPozisyonu.y - ilkDokunusPozisyonu.y;

            // Boyu dikey harekete göre deðiþtiriyoruz
            guncelBoy += farkY * hassasiyet;
            guncelBoy = Mathf.Clamp(guncelBoy, minBoy, maxBoy);

            ilkDokunusPozisyonu = guncelDokunusPozisyonu;
        }
    }

    void JeliBoyutlandir()
    {
        // Uzadýkça yanlardan daralan hacim formülü
        float hedefGenislik = 1f / Mathf.Sqrt(guncelBoy);
        Vector3 yeniBoyut = new Vector3(hedefGenislik, guncelBoy, hedefGenislik);

        // Deðiþimi pürüzsüzce modele uygula
        transform.localScale = Vector3.Lerp(transform.localScale, yeniBoyut, Time.deltaTime * esnemeHizi);
    }

    // Karakter bir Trigger (Sensör) collider'a girdiðinde burasý çalýþýr
    private void OnTriggerEnter(Collider other)
    {
        // Çarptýðýmýz nesnenin Tag'i "Engel" ise
        if (other.CompareTag("Engel"))
        {
            // Eðer zaten geriye doðru fýrlamýyorsak, sekme sürecini baþlat
            if (!geriyeGidiyor)
            {
                StartCoroutine(GeriyeSekmeSureci());
            }
        }
    }

    // Belirli bir süre karakteri geriye ittiren Coroutine fonksiyonu
    IEnumerator GeriyeSekmeSureci()
    {
        geriyeGidiyor = true; // Geriye kaymayý baþlat
        yield return new WaitForSeconds(geriGitmeSuresi); // Belirlediðimiz süre kadar (0.5 sn) bekle
        geriyeGidiyor = false; // Süre dolunca tekrar ileri gitmeye baþla
    }
}