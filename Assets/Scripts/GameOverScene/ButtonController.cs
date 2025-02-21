using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
   

   public void ClickButtonRestart()
   {
      SceneManager.LoadScene("game");
   }
}
