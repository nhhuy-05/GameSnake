using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera mainCamera;
    public int width, height;

    void Start()
    {
        // set height of main camera
        mainCamera.orthographicSize = height / 2;
        // set width of main camera
        mainCamera.aspect = (float)width / height;
        float viewportHeight = mainCamera.orthographicSize * 2.0f;
        float viewportWidth = viewportHeight * mainCamera.aspect;
        float startX = mainCamera.transform.position.x - viewportWidth / 2.0f;
        float startY = mainCamera.transform.position.y - viewportHeight / 2.0f;
        float stepX = viewportWidth / width;
        float stepY = viewportHeight / height;

        // set width of line renderer
        lineRenderer.startWidth = stepX / 10;
        lineRenderer.endWidth = stepX / 10;
        


        //// draw vertical lines
        //for (int i = 0; i < width + 1; i++)
        //{
        //    lineRenderer.SetPosition(0, new Vector3(startX + i * stepX, startY, 0));
        //    lineRenderer.SetPosition(1, new Vector3(startX + i * stepX, startY + viewportHeight, 0));
        //    lineRenderer.SetPosition(2, new Vector3(startX + i * stepX, startY + viewportHeight, 0));
        //    lineRenderer.SetPosition(3, new Vector3(startX + i * stepX, startY, 0));
        //}
        //// draw horizontal lines
        //for (int i = 0; i < height + 1; i++)
        //{
        //    lineRenderer.SetPosition(0, new Vector3(startX, startY + i * stepY, 0));
        //    lineRenderer.SetPosition(1, new Vector3(startX + viewportWidth, startY + i * stepY, 0));
        //    lineRenderer.SetPosition(2, new Vector3(startX + viewportWidth, startY + i * stepY, 0));
        //    lineRenderer.SetPosition(3, new Vector3(startX, startY + i * stepY, 0));
        //}
        // draw vertical lines
        // draw vertical lines
        for (int i = 0; i < width + 1; i++)
        {
            lineRenderer.positionCount += 4;
            lineRenderer.SetPosition(lineRenderer.positionCount - 4, new Vector3(startX + i * stepX, startY, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 3, new Vector3(startX + i * stepX, startY + viewportHeight, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 2, new Vector3(startX + i * stepX, startY + viewportHeight, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(startX + i * stepX, startY, 0));
            
            //lineRenderer.SetPosition(i * 2, new Vector3(startX + i * stepX, startY, 0));
            //lineRenderer.SetPosition(i * 2 + 1, new Vector3(startX + i * stepX, startY + viewportHeight, 0));
        }

        // draw horizontal lines
        for (int i = 0; i < height + 1; i++)
        {
            lineRenderer.positionCount += 4;
            lineRenderer.SetPosition(lineRenderer.positionCount - 4, new Vector3(startX, startY + i * stepY, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 3, new Vector3(startX + viewportWidth, startY + i * stepY, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 2, new Vector3(startX + viewportWidth, startY + i * stepY, 0));
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(startX, startY + i * stepY, 0));
            
            //lineRenderer.SetPosition((width + 1) * 2 + i * 2, new Vector3(startX, startY + i * stepY, 0));
            //lineRenderer.SetPosition((width + 1) * 2 + i * 2 + 1, new Vector3(startX + viewportWidth, startY + i * stepY, 0));
        }


    }
}
