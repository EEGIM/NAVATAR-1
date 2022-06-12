using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class armenu : MonoBehaviour
{
  public GameObject menu;
  public GameObject bigmenu;
  public List<GameObject> panels = new List<GameObject>();
  public Text text;
  public RawImage rawImage;
  public List<Texture> textures = new List<Texture>();
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public void OnClickmenuButton()
  {
    if (menu.gameObject.activeSelf == true)
    {
      menu.SetActive(false);
      bigmenu.SetActive(true);
    }
    else if (bigmenu.gameObject.activeSelf == true)
    {
      menu.SetActive(true);
      bigmenu.SetActive(false);
    }
  }

  public void Left()
  {
    if (text.text == "로맨틱 크라운")
    {
      text.text = "리";
      rawImage.texture = textures[4];
      panels[0].SetActive(false);
      panels[4].SetActive(true);
    }
    else if (text.text == "어피스오브케이크")
    {
      text.text = "로맨틱 크라운";
      rawImage.texture = textures[0];
      panels[1].SetActive(false);
      panels[0].SetActive(true);
    }
    else if (text.text == "마하그리드")
    {
      text.text = "어피스오브케이크";
      rawImage.texture = textures[1];
      panels[2].SetActive(false);
      panels[1].SetActive(true);
    }
    else if (text.text == "러브이즈트루")
    {
      text.text = "마하그리드";
      rawImage.texture = textures[2];
      panels[3].SetActive(false);
      panels[2].SetActive(true);
    }
    else if (text.text == "리")
    {
      text.text = "러브이즈트루";
      rawImage.texture = textures[3];
      panels[4].SetActive(false);
      panels[3].SetActive(true);
    }
  }

  public void Right()
  {
    if (text.text == "로맨틱 크라운")
    {
      text.text = "어피스오브케이크";
      rawImage.texture = textures[1];
      panels[0].SetActive(false);
      panels[1].SetActive(true);
    }
    else if (text.text == "어피스오브케이크")
    {
      text.text = "마하그리드";
      rawImage.texture = textures[2];
      panels[1].SetActive(false);
      panels[2].SetActive(true);
    }
    else if (text.text == "마하그리드")
    {
      text.text = "러브이즈트루";
      rawImage.texture = textures[3];
      panels[2].SetActive(false);
      panels[3].SetActive(true);
    }
    else if (text.text == "러브이즈트루")
    {
      text.text = "리";
      rawImage.texture = textures[4];
      panels[3].SetActive(false);
      panels[4].SetActive(true);
    }
    else if (text.text == "리")
    {
      text.text = "로맨틱 크라운";
      rawImage.texture = textures[0];
      panels[4].SetActive(false);
      panels[0].SetActive(true);
    }
  }

}
