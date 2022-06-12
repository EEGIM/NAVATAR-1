using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System;

public class Clothes : MonoBehaviour
{
  public List<Material> Clothmats = new List<Material>();
  public GameObject mantoman;
  public GameObject banpal;
  public GameObject pants;
  public GameObject skinny;
  public GameObject skirt;
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void OnClickClothButton()
  {
    GameObject clickObject = EventSystem.current.currentSelectedGameObject;
    GameObject Cloth;
    Cloth = mantoman; 
    if(clickObject.name.Substring(0, 5) == "Image")
    {
      int index = int.Parse(clickObject.name.Substring(8)) - 1;
      if (clickObject.name.Substring(6, 2) == "mt")
      {
        Debug.Log("1");
        if (banpal.gameObject.activeSelf == true)
        { banpal.SetActive(false); }
        mantoman.SetActive(true);
        Cloth = mantoman;
      }
      else if (clickObject.name.Substring(6, 2) == "bp")
      {
        if (mantoman.gameObject.activeSelf == true)
        { mantoman.SetActive(false);}
        banpal.SetActive(true);
        Cloth = banpal;
      }
      if (clickObject.name.Substring(6, 2) == "sn") 
      {
        if (pants.gameObject.activeSelf == true || skirt.gameObject.activeSelf == true)
        { pants.SetActive(false); skirt.SetActive(false); }
        skinny.SetActive(true);
        Cloth = skinny;
      }
      else if (clickObject.name.Substring(6, 2) == "pt")
      {
        if (skinny.gameObject.activeSelf == true || skirt.gameObject.activeSelf == true)
        { skinny.SetActive(false); skirt.SetActive(false); }
        pants.SetActive(true);
        Cloth = pants;
      }
      else if (clickObject.name.Substring(6, 2) == "st")
      {
        if (pants.gameObject.activeSelf == true || skinny.gameObject.activeSelf == true)
        { pants.SetActive(false); skinny.SetActive(false); }
        skirt.SetActive(true);
        Cloth = skirt;
      }
      Cloth.GetComponent<SkinnedMeshRenderer>().material = Clothmats[index];
    }
  }
}
