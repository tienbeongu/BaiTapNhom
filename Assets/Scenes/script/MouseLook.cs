using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    float xRotation = 0f; // Xoay lên xuống
    float yRotation = 0f; // Xoay trái phải

    void Start()
    {
        // Ẩn con trỏ chuột và khóa vào giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Lấy dữ liệu chuột (Lưu ý: Đảm bảo đã chỉnh Active Input Handling là 'Both')
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Tính toán góc xoay
        yRotation += mouseX;
        xRotation -= mouseY;

        // Giới hạn góc nhìn lên/xuống để không bị lộn ngược (thường là 90 độ)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Áp dụng xoay cho chính Camera này
        // Súng là con nên sẽ tự xoay theo 100%
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}