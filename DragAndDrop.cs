using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{

    [SerializeField]
    private InputAction mouseClick;
    [SerializeField]
    private float mouseDragSpeed = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;

    private void Awake(){
        mainCamera = Camera.main;
    }

    private void OnEnable(){
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }

    private void OnDisable(){
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    private void MousePressed(InputAction.CallbackContext context){
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider != null){
                StartCoroutine(DragUpdate(hit.collider.gameObject));
            }
        }
    }


    private IEnumerator DragUpdate(GameObject clickedObject){
        float initialDistance = Vector3.Distance(clickedObject.transform.position, mainCamera.transform.position);
        
        while(mouseClick.ReadValue<float>() != 0){
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Vector3 direction = ray.GetPoint(initialDistance) - clickedObject.transform.position;
            clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, ray.GetPoint(initialDistance), ref velocity, mouseDragSpeed);
            yield return null;
        }
    }

}
