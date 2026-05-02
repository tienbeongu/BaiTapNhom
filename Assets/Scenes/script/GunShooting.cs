using UnityEngine;
using System.Collections;

public class GunShooting : MonoBehaviour
{
    [Header("Cấu hình súng")]
    public float range = 100f;           // Khoảng cách bắn xa
    public int maxAmmo = 7;              // Lượng đạn tối đa (theo yêu cầu của bạn)
    public float reloadTime = 1.5f;      // Thời gian nạp đạn

    [Header("Hiệu ứng (Tech Art)")]
    public ParticleSystem muzzleFlash;   // Hiệu ứng tia lửa ở nòng súng
    public GameObject impactPrefab;      // Hiệu ứng khi bắn trúng (lông vũ/khói)

    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        // Gợi ý: Bạn có thể tạo thêm UI để hiển thị số đạn hiện tại
    }

    void Update()
    {
        if (isReloading) return;

        // Cơ chế tự động nạp đạn khi hết đạn
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Nhấn chuột trái để bắn
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Phím R để nạp đạn chủ động
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        currentAmmo--;
        Debug.Log("Pằng! Số đạn còn lại: " + currentAmmo);

        // Hiệu ứng tia lửa nòng súng (nếu bạn đã kéo vào Inspector)
        if (muzzleFlash != null) muzzleFlash.Play();

        // Tạo tia Raycast từ tâm màn hình (vị trí Crosshair)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Kiểm tra xem thứ vừa bắn trúng có phải là Vịt không
            Duck duck = hit.transform.GetComponent<Duck>();

            if (duck != null)
            {
                // Gây sát thương cho vịt (Vịt to có thể cần nhiều phát bắn hơn)
                duck.TakeDamage(1);

                // Hiệu ứng khi trúng đích (Tech Art)
                if (impactPrefab != null)
                {
                    Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Đang nạp đạn...");

        // Chờ thời gian nạp đạn đã thiết lập
        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Đã nạp đạn xong!");
    }
}
