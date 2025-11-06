using UnityEngine;
[System.Serializable]
public class ParallaxLayer
{
    public Transform background;
    public float parallaxMultiplier;
    public float parallaxOffset = 10; 

    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalculateImageWidths()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2f; 
    }
    public void Move(float distance)
    {

        background.position +=  Vector3.right * (distance * parallaxMultiplier);
    } 
    public void LoopBackground(float leftBoundary, float rightBoundary)
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth) + parallaxOffset;
        float imageRightEdge = (background.position.x + imageHalfWidth) - parallaxOffset;
        if (imageRightEdge < leftBoundary)
        {
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > rightBoundary)
        {
            background.position += Vector3.right * -imageFullWidth;
        }
    }

}
