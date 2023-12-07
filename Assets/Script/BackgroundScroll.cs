using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
  private Material material;
  float distance;
  [Range(0f, 1f)]
  public float scrollSpeed = 0.2f; // Speed of the scrolling

  public bool isActive = true;




  void Start()
  {
    material = GetComponent<Renderer>().material;

  }

  void Update()
  {
    if (isActive)
    {

      distance += Time.deltaTime * scrollSpeed;
      material.SetTextureOffset("_MainTex", Vector2.right * distance);


    }

  }
}
