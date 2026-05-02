using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DuckSpawner : MonoBehaviour
{
    [Header("Danh sách tài nguyên")]
    public GameObject[] duckPrefabs; // Kéo 3 loại vịt (Thường, Nhỏ, To) vào đây
    public Transform[] spawnPoints;  // Kéo các Object điểm spawn vào đây

    [Header("Cấu hình độ khó")]
    public float initialSpawnRate = 3f;
    public float minSpawnRate = 0.8f;
    public float difficultyIncreaseRate = 0.05f; // Mỗi lần spawn sẽ nhanh hơn 0.05s

    private float currentSpawnRate;

    void Start()
    {
        currentSpawnRate = initialSpawnRate;

        // Kiểm tra xem bạn đã kéo đủ điểm spawn chưa
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("LỖI: Bạn chưa kéo bất kỳ SpawnPoint nào vào mảng!");
            return;
        }

        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnRate);

            SpawnDuck();

            // Cơ chế Endless: Giảm thời gian chờ để game khó dần
            if (currentSpawnRate > minSpawnRate)
            {
                currentSpawnRate -= difficultyIncreaseRate;
            }
        }
    }

    void SpawnDuck()
    {
        // 1. Chọn vị trí ngẫu nhiên từ danh sách spawnPoints
        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform point = spawnPoints[randomPointIndex];

        // 2. Chọn loại vịt ngẫu nhiên
        // Mẹo: Bạn có thể chỉnh tỉ lệ để vịt To và vịt Nhỏ hiếm hơn vịt Thường
        int randomDuckIndex = Random.Range(0, duckPrefabs.Length);
        GameObject duckPrefab = duckPrefabs[randomDuckIndex];

        // 3. Sinh ra vịt tại vị trí đó
        GameObject newDuck = Instantiate(duckPrefab, point.position, point.rotation);

        // Mẹo Technical Artist: Thêm một chút ngẫu nhiên về vị trí X và Z 
        // để vịt không xuất hiện chính xác 100% tại tâm điểm spawn
        float randomOffset = Random.Range(-1.5f, 1.5f);
        newDuck.transform.position += new Vector3(randomOffset, 0, randomOffset);
    }

    // Vẽ biểu tượng trong Scene để bạn dễ nhìn thấy các điểm Spawn
    void OnDrawGizmos()
    {
        if (spawnPoints == null) return;
        Gizmos.color = Color.yellow;
        foreach (var p in spawnPoints)
        {
            if (p != null) Gizmos.DrawSphere(p.position, 0.5f);
        }
    }
}