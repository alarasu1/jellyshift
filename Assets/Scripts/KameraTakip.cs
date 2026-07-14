using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    public Transform hedef; // Buraya hiyerarþideki en dýþtaki boþ "jelly" atanacak
    public float arkasindanUzaklik = 5f;
    public float yukseklik = 2f;

    void LateUpdate()
    {
        // Eðer hedef seçilmediyse kamerayý oynatýp hata verme
        if (hedef == null) return;

        // Kamerayý karakterin tam arkasýna zýmbala (karakter kendi içinde yamulsa bile kamera etkilenmez)
        Vector3 hedefPozisyon = hedef.position - (hedef.forward * arkasindanUzaklik) + (Vector3.up * yukseklik);
        transform.position = hedefPozisyon;

        // Kamera her zaman karaktere baksýn
        transform.LookAt(hedef);
    }
}