// 월드맵 토글 및 플레이어 아이콘 UI 위치 관리
using UnityEngine;
using UnityEngine.UI;

public class WorldMapController : MonoBehaviour
{
    [Header("월드맵 UI 패널")]
    public GameObject worldmapPanel;

    [Header("월드맵 카메라와 타겟")]
    public Transform player; // 플레이어 Transform
    public Camera mapCamera; // 월드맵 카메라

    [Header("UI 요소")]
    public RectTransform minimapRect; // RawImage의 RectTransform
    public RectTransform playerIcon;  // 플레이어 아이콘 Image    [Header("Terrain 크기 (가로 x 세로)")]
    public Vector2 worldMapSize = new Vector2(1000, 1000); // Terrain 크기 (Unity Inspector에서 확인된 실제 크기)

    [Header("카메라 자동 설정")]
    public Vector2 worldCenter = new Vector2(0, 0); // 월드 중심점 (Terrain Transform 기준)
    public float cameraHeight = 700f; // 카메라 높이 (터레인 전체를 보기 위해 충분히 높게)
    public float heightMargin = 1.1f; // 여백 비율 (1.0 = 딱 맞게, 1.1 = 10% 여백)

    [Header("UI Size Settings")]
    public Vector2 mapUISize = new Vector2(800, 800); // 월드맵 UI 크기 설정

    void Start()
    {
        // 터레인 자동 감지 및 설정
        DetectAndSetupTerrain();

        // // 월드맵 UI 크기 자동 설정
        // SetupWorldMapUISize(); if (player == null)
        // {
        //     PlayerControll playerControll = FindFirstObjectByType<PlayerControll>();
        //     if (playerControll != null)
        //     {
        //         player = playerControll.transform;
        //     }
        // }
    }

    void Update()
    {
        HandleToggle();
        UpdatePlayerIcon();
    }
    void SetupMapCamera()
    {
        if (mapCamera == null) return;

        // Terrain Transform 위치 확인 (만약 Terrain이 (0,0,0)이 아닌 곳에 있다면)
        Terrain terrain = FindAnyObjectByType<Terrain>();
        if (terrain != null)
        {
            // Terrain의 실제 중심점 계산
            Vector3 terrainPos = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;

            worldCenter.x = terrainPos.x + terrainData.size.x / 2f;
            worldCenter.y = terrainPos.z + terrainData.size.z / 2f;

            // Terrain Data에서 실제 크기 가져오기
            worldMapSize.x = terrainData.size.x;
            worldMapSize.y = terrainData.size.z;

        }

        // 카메라를 월드 중심 위에 배치
        Vector3 cameraPosition = new Vector3(worldCenter.x, cameraHeight, worldCenter.y);
        mapCamera.transform.position = cameraPosition;

        // 카메라를 아래쪽을 바라보도록 설정
        mapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Orthographic 카메라 사이즈 자동 계산
        if (mapCamera.orthographic)
        {
            // 월드 크기 중 더 큰 값을 기준으로 카메라 사이즈 설정
            float maxWorldSize = Mathf.Max(worldMapSize.x, worldMapSize.y);
            mapCamera.orthographicSize = (maxWorldSize * heightMargin) / 2f;
        }
    }

    void HandleToggle()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            worldmapPanel.SetActive(!worldmapPanel.activeSelf);
        }
    }
    void UpdatePlayerIcon()
    {
        if (player == null || minimapRect == null || playerIcon == null) return;

        Vector3 playerPos = player.position;

        // 월드 중심점을 기준으로 상대 좌표 계산
        float relativeX = playerPos.x - worldCenter.x;
        float relativeZ = playerPos.z - worldCenter.y;

        // 월드 범위에서 0~1로 정규화 (음수 좌표 포함)
        float normX = (relativeX + worldMapSize.x / 2f) / worldMapSize.x;
        float normZ = (relativeZ + worldMapSize.y / 2f) / worldMapSize.y;

        // 안전한 클램핑
        normX = Mathf.Clamp01(normX);
        normZ = Mathf.Clamp01(normZ);

        // 미니맵 내부 UI 좌표 계산
        float posX = (normX - 0.5f) * minimapRect.rect.width;
        float posY = (normZ - 0.5f) * minimapRect.rect.height;

        playerIcon.anchoredPosition = new Vector2(posX, posY);

        playerIcon.localRotation = Quaternion.Euler(0, 0, -player.eulerAngles.y);
    }

    private void SetupWorldMapUISize()
    {
        if (minimapRect != null)
        {
            // RectTransform의 크기 설정
            minimapRect.sizeDelta = mapUISize;

        }

    }

    private void DetectAndSetupTerrain()
    {
        // Terrain 자동 감지 및 설정 로직 (기존 코드 유지)
        Terrain terrain = FindAnyObjectByType<Terrain>();
        if (terrain != null)
        {
            // Terrain의 실제 중심점 계산
            Vector3 terrainPos = terrain.transform.position;
            TerrainData terrainData = terrain.terrainData;

            worldCenter.x = terrainPos.x + terrainData.size.x / 2f;
            worldCenter.y = terrainPos.z + terrainData.size.z / 2f;

            // Terrain Data에서 실제 크기 가져오기
            worldMapSize.x = terrainData.size.x;
            worldMapSize.y = terrainData.size.z;

        }


        // 카메라 및 UI 재설정
        SetupMapCamera();
        SetupWorldMapUISize();
    }
}
