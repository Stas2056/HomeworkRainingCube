using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorRandomiser : MonoBehaviour
{  
    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV();
    }
}