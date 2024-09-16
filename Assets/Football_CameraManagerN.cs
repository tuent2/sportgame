using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
public class Football_CameraManagerN : MonoBehaviour
{
    public static Football_CameraManagerN instance;

    public static Action EventReset = delegate { };
    public static Action EventBeginIntroMovement = delegate { };
    public static Action EventEndIntroMovement = delegate { };

    public GameObject _cameraMain;
    public GameObject _cameraBack;
    public Camera[] _camerasMain;

    //[SerializeField]
    int _FOVportrait = 40;
    //[SerializeField]
    int _FOVlandscape = 30;

    private bool isPortrait = true;
    public Camera _cameraMainComponent;
    public Collider _colliderFullScreen;

    [SerializeField]
    private float yBackCamera = 1f;
    [SerializeField]
    private float cameraMainDistanceToBall = -5f;
    [SerializeField]
    private float cameraMainY = 1.7f;

    private void Awake()
    {
        instance = this;
        _cameraMainComponent = _cameraMain.GetComponent<Camera>();
    }

	void Start()
	{
		updateCameraFOV();
		Football_Shooter.EventShoot += eventShoot;
	}

	void OnDestroy()
	{
		Football_Shooter.EventShoot -= eventShoot;
	}

	public void turnOn()
	{
		_cameraMain.SetActive(true);
		_cameraBack.SetActive(false);
	}

	public void turnOff()
	{
		_cameraMain.SetActive(false);
		_cameraBack.SetActive(false);
	}

	private bool _isCameraMoving = false;
	Tweener _tweenCamera;

	[SerializeField] private float testXZ = 0.8f;
	[SerializeField] private float testY = 5f;

	private void eventShoot()
	{
		MoveCameraWhenShoot();
	}

	public void MoveCameraWhenShoot()
	{
		if (_isCameraMoving == true)
			return;

		_isCameraMoving = true;

		Transform ballTrans = Football_Shot.intance._ball.transform;
		_cameraMain.GetComponent<Football_SmoothLookAt>().target = ballTrans;

		Tween _tweenCamera = _cameraMain.transform.DOMove(new Vector3(ballTrans.position.x * testXZ, testY, ballTrans.position.z * testXZ), 3f)
	.SetLoops(1, LoopType.Restart) // Đặt số lần lặp là 1 và kiểu lặp là Restart
	.SetEase(Ease.Linear) // Đặt loại dễ dàng là Linear
	.OnComplete(cameraFinishMoving) // Đặt phương thức sẽ gọi khi tween hoàn thành
	.SetAutoKill(true);


		_currentFOV = _FOVportrait;
	}


	public float _currentFOV;
	private void cameraFinishMoving()
	{
		//		_cameraMain.GetComponent<SmoothLookAt>().target = null;
	}

	public void introMovement(Action callback, Transform target)
	{
		EventBeginIntroMovement();

		Vector3[] path = new Vector3[3];
		path[2] = target.position;
		path[0] = new Vector3(0, 1.5f, -60f);
		path[1] = (path[2] + path[0]) / 2;
		path[1].y = 6f;

		Quaternion rotation = target.rotation;
		_cameraMain.transform.localEulerAngles = Vector3.zero;
		_cameraMain.transform.position = path[0];

		DOTween.To(() => _cameraMain.transform.position,   // Lấy giá trị hiện tại của position
		   newPosition => _cameraMain.transform.position = newPosition,  // Gán giá trị mới vào position
		   path[path.Length - 1],   // Điểm cuối cùng trong path làm điểm đến
		   6f)   // Thời gian diễn ra tweening

	// Các thuộc tính khác của tween
	.SetEase(Ease.InOutQuad)   // Thiết lập easing type
	.SetLoops(1, LoopType.Restart)   // Thiết lập số lần lặp lại và kiểu lặp lại
	.OnComplete(() => {   // Xử lý khi tween hoàn thành
		if (callback != null)
			callback();

		EventEndIntroMovement();   // Gọi hàm khi hoàn thành
	})
	.SetAutoKill(true);   // Tự động hủy tween khi hoàn thành
	}

	public void reset()
	{
		if (_tweenCamera != null)
			_tweenCamera.Kill();
		_cameraMain.GetComponent<Football_SmoothLookAt>().target = null;
		_isCameraMoving = false;

		updateCameraFOV();

		StartCoroutine(resetPosition());

		EventReset();
	}


	private IEnumerator resetPosition()
	{

		Transform ball = Football_Shot.intance._ball.transform;

		Vector3 diff = -ball.position;
		diff.Normalize();
		float angleRadian = Mathf.Atan2(diff.x, diff.z);
		float angle = angleRadian * Mathf.Rad2Deg;          // goc lech so voi goc toa do
		angleRadian = angle * Mathf.Deg2Rad;

		Vector3 pos = ball.position;        // pos se duoc gan' la vi tri cua camera
		pos.y = cameraMainY;            // camera cach' mat dat 1.7m

		if (isPortrait)
		{       // neu la portrait thi camera nam dang sau trai banh 4m va huong ve goc toa do, noi cach khac' la cung huong' voi' truc z cua parent cua ball
			pos.x += cameraMainDistanceToBall * Mathf.Sin(angleRadian);
			pos.z += cameraMainDistanceToBall * Mathf.Cos(angleRadian);
		}
		else
		{       // neu la landscape thi camera nam dang sau trai banh 4m va huong ve goc toa do, noi cach khac' la cung huong' voi' truc z cua parent cua ball
			pos.x += cameraMainDistanceToBall * Mathf.Sin(angleRadian);
			pos.z += cameraMainDistanceToBall * Mathf.Cos(angleRadian);
		}

		_cameraMain.transform.position = pos;

		Vector3 rotation = _cameraMain.transform.eulerAngles;
		rotation.y = angle;
		rotation.x = 6f;            // quay 6 do theo truc x
		rotation.z = 0f;
		_cameraMain.transform.eulerAngles = rotation;

		float distanceBackCameraToBall = 10f; // 18.31871f;

		float x = distanceBackCameraToBall * Mathf.Sin(angleRadian);
		float z = distanceBackCameraToBall * Mathf.Cos(angleRadian);

		if (_cameraBack)
		{
			_cameraBack.transform.position = new Vector3(x, yBackCamera, z);

			Vector3 posGK = Football_GoalKeeper.instance.transform.position;
			posGK.y = yBackCamera;

			Quaternion rotationLook = Quaternion.LookRotation(Football_Shot.intance._cachedTrans.position - _cameraBack.transform.position);
			_cameraBack.transform.rotation = rotationLook;
		}

		yield break;
	}

	public void updateCameraFOV()
	{

		if (Screen.height > Screen.width)
		{       // portrait
			isPortrait = true;
			foreach (Camera camera in _camerasMain)
			{
				if (camera.orthographic == false)
				{
					camera.fieldOfView = _FOVportrait;
					_currentFOV = _FOVportrait;
				}
			}
		}
		else
		{           // landscape
			isPortrait = false;
			foreach (Camera camera in _camerasMain)
			{
				if (camera.orthographic == false)
				{
					camera.fieldOfView = _FOVlandscape;
					_currentFOV = _FOVlandscape;
				}
			}
		}

		//reset();
	}
}
