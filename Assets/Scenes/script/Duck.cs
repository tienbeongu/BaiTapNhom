using UnityEngine;

public class Duck : MonoBehaviour
{
    public enum DuckState { FlyingUp, FallingNatural, Dead }
    private DuckState currentState = DuckState.FlyingUp;

    [Header("Thông số di chuyển")]
    public float flyUpSpeed = 5f;
    public float fallSpeed = 3f;
    public float targetHeight = 10f; // Độ cao tối đa vịt sẽ bay lên

    [Header("Thông số sinh tồn")]
    public int health = 1; // Số lần bắn để chết
    public float scale = 1f;

    private Vector3 startPos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        transform.localScale = Vector3.one * scale; // Chỉnh kích thước

        rb.isKinematic = true; // Ban đầu dùng code để bay
        rb.useGravity = false;
    }

    void Update()
    {
        switch (currentState)
        {
            case DuckState.FlyingUp:
                FlyUp();
                break;
            case DuckState.FallingNatural:
                FallNatural();
                break;
        }
    }

    void FlyUp()
    {
        transform.Translate(Vector3.up * flyUpSpeed * Time.deltaTime);
        if (transform.position.y >= startPos.y + targetHeight)
        {
            currentState = DuckState.FallingNatural;
        }
    }

    void FallNatural()
    {
        // Vịt rơi xuống vì không bị bắn
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Nếu rơi xuống quá thấp mà chưa chết -> Tính là hụt (Miss)
        if (transform.position.y <= startPos.y - 2f)
        {
            GameManager.Instance.AddMiss();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentState == DuckState.Dead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        currentState = DuckState.Dead;
        rb.isKinematic = false; // Trả về vật lý thật
        rb.useGravity = true;

        GameManager.Instance.AddScore(100); // Cộng điểm

        // Hiệu ứng chết
        transform.Rotate(90, 0, 0);
        Destroy(gameObject, 2f); // Biến mất sau 2s
    }
}