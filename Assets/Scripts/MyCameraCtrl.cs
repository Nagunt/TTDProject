using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class MyCameraCtrl : MonoBehaviour
{
    public Transform leftDownPos;
    public Transform rightUpPos;

    public float zoomSpeed;
    public float moveSpeed;

    private Camera mainCamera;

    private float minSize;
    [SerializeField]
    private float maxSize;

    private void Start()
    {
        minSize = 1;
        maxSize = Mathf.Min((rightUpPos.position.x - leftDownPos.position.x) * .5f, (rightUpPos.position.y - leftDownPos.position.y) * .5f);
        mainCamera = GetComponent<Camera>();
        mainCamera.orthographicSize = maxSize;
        if (mainCamera.orthographicSize > maxSize) mainCamera.orthographicSize = maxSize;
        if (mainCamera.orthographicSize < minSize) mainCamera.orthographicSize = minSize;
        StartCoroutine(MouseDragRoutine());
    }

    private void Update()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput != 0)
        {
            mainCamera.orthographicSize -= wheelInput * zoomSpeed;
            if (mainCamera.orthographicSize > maxSize) mainCamera.orthographicSize = maxSize;
            if (mainCamera.orthographicSize < minSize) mainCamera.orthographicSize = minSize;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        AdjustCameraPosition();
    }

    private IEnumerator MouseDragRoutine()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false);
        Vector2 mousePosition = Input.mousePosition;
        while (Input.GetMouseButton(0))
        {
            Vector2 direction = mousePosition - (Vector2)Input.mousePosition;
            if (direction.sqrMagnitude > 0.1f)
            {
                transform.Translate(direction * .5f * Time.deltaTime);
                AdjustCameraPosition();
                mousePosition = Input.mousePosition;
            }
            yield return 0;
        }
        StartCoroutine(MouseDragRoutine());
    }

    private void AdjustCameraPosition()
    {
        Vector3 cam_leftDownPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 cam_rightUpPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        if (cam_rightUpPos.y > rightUpPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (cam_rightUpPos.y - rightUpPos.position.y), transform.position.z);
        }
        if (cam_rightUpPos.x > rightUpPos.position.x)
        {
            transform.position = new Vector3(transform.position.x - (cam_rightUpPos.x - rightUpPos.position.x), transform.position.y, transform.position.z);
        }
        if (cam_leftDownPos.x < leftDownPos.position.x)
        {
            transform.position = new Vector3(transform.position.x + (leftDownPos.position.x - cam_leftDownPos.x), transform.position.y, transform.position.z);
        }
        if (cam_leftDownPos.y < leftDownPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (leftDownPos.position.y - cam_leftDownPos.y), transform.position.z);
        }
        if (mainCamera.orthographicSize * 2 * mainCamera.aspect > (rightUpPos.position.x - leftDownPos.position.x))
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
