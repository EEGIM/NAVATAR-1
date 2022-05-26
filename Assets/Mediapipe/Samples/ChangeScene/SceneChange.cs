using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
  public void LoadSigninScene()
  {
    SceneManager.LoadScene(1);
  }
  public void LoadMenuScene()
  {
    SceneManager.LoadScene(2);
  }
  public void LoadUserInfoScene()
  {
    SceneManager.LoadScene(3);
  }
  public void LoadCalculateScene()
  {
    SceneManager.LoadScene(4);
  }
  public void LoadARFitting()
  {
    SceneManager.LoadScene(5);
  }

}
