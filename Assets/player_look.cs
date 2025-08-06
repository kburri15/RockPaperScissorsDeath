using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private float yRotation = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            yRotation -= 90f;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            yRotation += 90f;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            yRotation += 180f;

        transform.rotation = Quaternion.Euler(0f, yRotation % 360f, 0f);
    }
}
