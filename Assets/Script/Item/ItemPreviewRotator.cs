using UnityEngine;

public class ItemPreviewRotator : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
    }
}
