using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour, IAnyBoardListener
{
    public float heightPadding;
    public float widthPadding;

    private float _sceneWidth = 10;
    private float _sceneHeight = 10;
    private float _boardWidth = 10;
    private float _boardHeight = 10;
    private Camera _camera;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
        AttachToBoard();
        SubscribeToUiResetEvent();
        _transform = transform;
    }

    private void Update()
    {
        _sceneHeight = _boardHeight + heightPadding;
        _sceneWidth = _boardWidth + widthPadding;

        var newPos = _transform.position;
        var unitsPerPixelAccToWidth = _sceneWidth / Screen.width;
        var desiredHalfHeightAccToWidth = 0.5f * unitsPerPixelAccToWidth * Screen.height;

        var unitsPerPixelAccToHeight = _sceneHeight / Screen.height;
        var desiredHalfHeightAccToHeight = 0.5f * unitsPerPixelAccToHeight * Screen.height;

        newPos.x = (_sceneWidth - widthPadding) / 2f;
        newPos.y = (_sceneHeight - heightPadding) / 1.8f;
        _transform.position = newPos;

        _camera.orthographicSize = Mathf.Max(desiredHalfHeightAccToHeight, desiredHalfHeightAccToWidth);
    }

    public void OnAnyBoard(GameEntity entity, Vector2Int size, bool isFallActive, bool isTouchActive, bool isHintActive,
        int touchBlockCounter, int fallBlockCounter, int hintBlockCounter)
    {
        _boardWidth = size.x;
        _boardHeight = size.y;
    }

    private void AttachToBoard()
    {
        Contexts.sharedInstance.game.CreateEntity().AddAnyBoardListener(this);
    }

    private void Reset()
    {
        AttachToBoard();
    }

    private void SubscribeToUiResetEvent()
    {
        GameController.Instance.ResetEvent += Reset;
    }
}