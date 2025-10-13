using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
   public void TombolKeluar()
   { 
       Application.Quit();
       Debug.Log("Game Close");
   }

   public void loadpenulis()
   {
       SceneManager.LoadScene("penulis");   
   }

   public void loadbangunruang()
   {
       SceneManager.LoadScene("bangunruang");   
   }

   public void loadbangundatar()
   {
       SceneManager.LoadScene("bangundatar");   
   }

   public void loadtentangaplikasi()
   {
       SceneManager.LoadScene("tentangaplikasi");   
   }

   // ✅ Tambahan ini untuk tombol Kuis
   public void loadKuis()
   {
       SceneManager.LoadScene("Kuis");   
   }
}
