using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public Camera mainCamera;

    float zoomSpeed = 500;

    private void Start() {
        
    }

    private void Update() {
        if(Input.GetMouseButton(1)){
            MoveMouse();
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0){
            ZoomCamera();
        }
        if(Input.GetMouseButton(2)){
            RotateCamera();
        }
    }

    private void MoveMouse(){
        Vector3 direction = new Vector3(-Input.GetAxis("Mouse X"), 0, -Input.GetAxis("Mouse Y"));
        mainCamera.transform.Translate(direction, Space.World);
    }

    private void ZoomCamera(){
        float zoom = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;
        mainCamera.transform.Translate(zoom * Vector3.forward, Space.Self);
        
    }
    private void RotateCamera(){
        // YET TO IMPLEMENT
    }
}
