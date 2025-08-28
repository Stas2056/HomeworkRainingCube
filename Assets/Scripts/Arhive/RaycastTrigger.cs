using System;
using UnityEngine;

public class RaycastTrigger : MonoBehaviour
{  
    public event Action<GameObject> CubeClicked;
    
    private void Update()
    {
        int leftMouseButtonIndex = 0;

        if (Input.GetMouseButtonDown(leftMouseButtonIndex))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out GameObject cube))
                {
                    CubeClicked?.Invoke(cube);
                    cube.InvokeDestroy();
                }
            }
        }
    }
}