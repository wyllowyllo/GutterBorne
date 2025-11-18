using System;
using UnityEngine;

public class Exit : MonoBehaviour
{
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (!other.gameObject.CompareTag("Player")) return;

      GameManager.Instance.GameClear();

   }
}
