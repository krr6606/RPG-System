using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraX;
    public ParallaxLayer[] BGLayers;
    private float cameraHalfWidth;
    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }
    private void FixedUpdate()
    {
        float currentCameraX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraX - lastCameraX;
        lastCameraX = currentCameraX;

        float leftBoundary = currentCameraX - cameraHalfWidth;
        float rightBoundary = currentCameraX + cameraHalfWidth;

        foreach (ParallaxLayer parallaxLayer in BGLayers)
        {
            parallaxLayer.Move(distanceToMove);
            parallaxLayer.LoopBackground(leftBoundary, rightBoundary);
        }
    }
    private void InitializeLayers()
    {
        foreach (ParallaxLayer parallaxLayer in BGLayers)
        {
            parallaxLayer.CalculateImageWidths();
        }
    }
}
