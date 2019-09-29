using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public static Vector3 clickPosition;

    [SerializeField]
    private LayerMask _clickMask;

    [SerializeField]
    private GameObject _checkpoint;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = -Vector3.one;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 80f, _clickMask))
            {
                clickPosition = hit.point;
            }

            clickPosition.y += (float)0.1;
            Instantiate(_checkpoint, clickPosition, Quaternion.identity);
        }
    }
}
