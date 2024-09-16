using UnityEngine;
using System.Collections;
using DG.Tweening;

public enum Area
{
	Top,
	Left,
	Right,
	Normal,
	CornerLeft,
	CornerRight,
	None
}
public delegate void GoalEvent(bool isGoal, Area area);

public class Football_GoalDetermine : MonoBehaviour
{

	public static Football_GoalDetermine share;

	public static GoalEvent EventFinishShoot;
	//public GameObject _prefabGoalSuccess;

	private bool _isGoal;
	public bool _goalCheck;

	private Transform _ball;

	[SerializeField]
	private Transform _pointLeft;
	[SerializeField]
	private Transform _pointRight;
	[SerializeField]
	private Transform _pointUp;
	[SerializeField]
	private Transform _pointDown;
	[SerializeField]
	private Transform _pointBack;

	public Renderer _areaTop;
	public Renderer _areaCornerLeft;
	public Renderer _areaCornerRight;
	public Renderer _areaLeft;
	public Renderer _areaRight;

	//public Renderer _effectTop;
	//public Renderer _effectLeft;
	//public Renderer _effectCornerLeft;
	//public Renderer _effectCornerRight;
	//public Renderer _effectRight;

	public Transform _poleLeft;
	public Transform _poleRight;

	//public Material _matCenter;
	//public Texture2D _centerNormal;
	//public Texture2D _centerCritical;

	private Area _poleArea;

	void Awake()
	{
		share = this;
		//_prefabGoalSuccess.SetActive(false);
	}

	// Use this for initialization
	void Start()
	{
		_ball = Football_Shot.intance._ball.transform;
		Football_Shooter.EventShoot += eventShoot;
	}


	void OnDestroy()
	{
		Football_Shooter.EventShoot -= eventShoot;
	}

	private Vector3 _contactPointWithPole;

	public void hitPole(Area poleArea, Vector3 contactPoint)
	{
		if (_goalCheck)
		{
			_contactPointWithPole = contactPoint;
			_poleArea = poleArea;
		}
	}

	private void eventShoot()
	{


		_count1 = _count2 = _count3 = 0;
		_goalCheck = true;
		_posPrev = _posCur = _ball.position;

		_poleArea = Area.None;
	}

	public void reset()
	{
		_goalCheck = false;
	}

	public void flashingHighScoreAreas()
	{
		//_effectTop.enabled = true;
		//_effectLeft.enabled = true;
		//_effectRight.enabled = true;
		//_effectCornerLeft.enabled = true;
		//_effectCornerRight.enabled = true;

		//Material mat = _effectTop.sharedMaterial;
		//Color col = mat.GetColor("_TintColor");
		//col.a = 50f / 255f;
		//mat.SetColor("_TintColor", col);

		//mat = _effectCornerLeft.sharedMaterial;
		//col = mat.GetColor("_TintColor");
		//col.a = 50f / 255f;
		//mat.SetColor("_TintColor", col);

		//// Sử dụng DOTween để thay thế iTween
		//float fromValue = 50f / 255f;
		//float toValue = 0f;
		//float duration = 1.5f;
		//float delay = 0.5f;

		//// Tạo tweener sử dụng DOTween
		//DOTween.To(() => fromValue, x => fromValue = x, toValue, duration)
		//	.SetDelay(delay)
		//	.SetEase(Ease.Linear)
		//	.OnUpdate(()=> {
		//		onUpdateFlashing(fromValue);
		//	})  // Gọi hàm onUpdate
		//	.OnComplete(()=> {
		//		completeFlashing();
		//	});  // Gọi hàm onComplete
	}

	private void onUpdateFlashing(float val)
	{
		//Material mat = _effectTop.sharedMaterial;
		//Color col = mat.GetColor("_TintColor");
		//col.a = val;
		//mat.SetColor("_TintColor", col);

		//mat = _effectCornerLeft.sharedMaterial;
		//col = mat.GetColor("_TintColor");
		//col.a = 50f / 255f;
		//mat.SetColor("_TintColor", col);
	}

	private void completeFlashing()
	{
		//_effectTop.enabled = false;
		//_effectLeft.enabled = false;
		//_effectRight.enabled = false;
		//_effectCornerLeft.enabled = false;
		//_effectCornerRight.enabled = false;
	}

	private Vector3 _posPrev;
	private Vector3 _posCur;

	private float _count1;
	private float _count2;
	private float _count3;

	float _distance;

	// Update is called once per frame
	void Update()
	{

		if (_goalCheck)
		{

			_posPrev = _posCur;
			_posCur = _ball.position;

			if (_posCur.z >= _pointDown.position.z)
			{       // ko check goal nua
				if (_posCur.z <= _pointBack.position.z && _posCur.x < _pointRight.position.x && _posCur.x > _pointLeft.position.x && _posCur.y < _pointUp.position.y && _posCur.y > 0)
				{
					_count1 = 0;

					_goalCheck = false;

					Area area = Area.Normal;

					if (_poleArea != Area.None)
					{       // neu truoc do trung xa ngang hay cot doc xong roi banh vo luoi 

						area = Area.CornerLeft;
						setGoalSuccesIcon(area, _contactPointWithPole);

					}
					else
					{       // khong trung xa ngang cot doc gi het
						if (_areaTop.bounds.Contains(_posCur))
							area = Area.Top;
						else if (_areaLeft.bounds.Contains(_posCur))
						{
							area = Area.Left;
						}
						else if (_areaRight.bounds.Contains(_posCur))
						{
							area = Area.Right;
						}
						else if (_areaCornerLeft.bounds.Contains(_posCur))
						{
							area = Area.CornerLeft;
						}
						else if (_areaCornerRight.bounds.Contains(_posCur))
						{
							area = Area.CornerRight;
						}

						setGoalSuccesIcon(area, _ball.transform.position);
					}


					if (EventFinishShoot != null)
						EventFinishShoot(true, area);
				}
				else
				{
					_count1 += Time.deltaTime;
					if (_count1 > 1f)
					{
						_goalCheck = false;
						if (EventFinishShoot != null)
							EventFinishShoot(false, Area.None);
					}
				}
			}
			else
			{      // tiep tuc check goal
				if (_posCur.z > _posPrev.z)
				{
					_count2 = 0;

					_distance = 10f;
					if (Football_ShotAI.intance._ball.velocity.sqrMagnitude < 0.3f)
						_distance = 0f;
					else if (Football_ShotAI.intance._ball.velocity.sqrMagnitude < 2f)
						_distance = 4f;

					if (Mathf.Abs(_posCur.x) > _distance && (Mathf.Abs(_posCur.x) > Mathf.Abs(_posPrev.x) || Football_ShotAI.intance._ball.velocity.sqrMagnitude < 0.3f))
					{
						_count3 += Time.deltaTime;
						if (_count3 > 1f)
						{
							_goalCheck = false;
							if (EventFinishShoot != null)
								EventFinishShoot(false, Area.None);
						}
					}
					else
					{
						_count3 = 0;
					}
				}
				else
				{
					_count2 += Time.deltaTime;
					if (_count2 > 1f)
					{
						_goalCheck = false;
						if (EventFinishShoot != null)
							EventFinishShoot(false, Area.None);
					}
				}
			}
		}
	}

	private void setGoalSuccesIcon(Area area, Vector3 position)
	{
		//_prefabGoalSuccess.SetActive(true);
		//_prefabGoalSuccess.transform.position = position;

		//iTween.ScaleAdd(_prefabGoalSuccess, iTween.Hash("time", 0.25f
		//                                                , "looptype", iTween.LoopType.pingPong
		//                                                , "amount", new Vector3(0.3f, 0.3f, 0)
		//                                                , "easetype", iTween.EaseType.linear
		//                                                ));

		//Vector3 originalScale = _prefabGoalSuccess.transform.localScale;
		//Vector3 targetScale = originalScale + new Vector3(0.3f, 0.3f, 0);
		//DOTween.Sequence()
	//.Append(_prefabGoalSuccess.transform.DOScale(targetScale, 0.25f))
	//.Append(_prefabGoalSuccess.transform.DOScale(originalScale, 0.25f))
	//.SetLoops(-1, LoopType.Restart) // SetLoops(-1) cho vòng lặp vô hạn, Restart để bắt đầu lại sau khi hoàn thành
	//.SetEase(Ease.Linear);


	//	if (area == Area.None || area == Area.Normal)
	//	{
	//		_matCenter.mainTexture = _centerNormal;

	//	}
	//	else
	//	{
	//		_matCenter.mainTexture = _centerCritical;

	//	}

	//	_prefabGoalSuccess.transform.localScale = new Vector3(0.6f, 0.6f, 1);
	//	_prefabGoalSuccess.transform.DOScale(new Vector3(0.9f, 0.9f, 1), 1f)
	//					   .SetLoops(1, LoopType.Restart)
	//					   .SetEase(Ease.Linear)
	//					   .OnComplete(animationFinished);

	//	Material mat = _prefabGoalSuccess.GetComponent<Renderer>().sharedMaterial;
	//	Color col = mat.GetColor("_TintColor");
	//	col.a = 0.5f;
	//	mat.SetColor("_TintColor", col);

	//	Tween tween = DOTween.To(() => 0.5f, x => onUpdateColor(x), 1f, 0.5f)
	//.SetEase(Ease.InCubic) // SetEase để đặt loại dễ dàng là EaseInCubic
	//.OnComplete(() => complete1());

	//	flashingHighScoreAreas();
	}

	private void onUpdateColor(float val)
	{
		//Material mat = _prefabGoalSuccess.GetComponent<Renderer>().sharedMaterial;
		//Color col = mat.GetColor("_TintColor");
		//col.a = val;
		//mat.SetColor("_TintColor", col);
	}

	private void complete1()
	{
		//iTween.ValueTo(gameObject, iTween.Hash("time", 0.5f
		//									   , "from", 1f
		//									   , "to", 0f
		//									   , "easetype", iTween.EaseType.easeOutCubic
		//									   , "onupdate", "onUpdateColor"
		//									   ));

		Tween tween = DOTween.To(() => 1f, x => onUpdateColor(x), 0f, 0.5f)
	.SetEase(Ease.OutCubic);
	}

	private void animationFinished()
	{
		//_prefabGoalSuccess.SetActive(false);
	}
}
