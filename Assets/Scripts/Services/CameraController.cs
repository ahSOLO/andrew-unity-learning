using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : IGameService
{
    [SerializeField] GameObject gimbal;
    [SerializeField] float rotationSpeed = 270f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        gimbal.transform.Rotate(-(Input.GetAxisRaw("Mouse Y")) * Time.deltaTime * rotationSpeed, (Input.GetAxisRaw("Mouse X")) * Time.deltaTime * rotationSpeed, 0f);

        gimbal.transform.rotation = Quaternion.Euler(Mathf.Clamp(Helpers.NormalizeAngle(gimbal.transform.rotation.eulerAngles.x), -45f, 45f), gimbal.transform.rotation.eulerAngles.y, 0f);
        // Why doesn't this work?
        // gimbal.transform.rotation.eulerAngles = new Vector3(Mathf.Clamp(gimbal.transform.rotation.eulerAngles.x, -30f, 10f), gimbal.transform.rotation.eulerAngles.y, 0f);

    }
}
