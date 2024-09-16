using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using System;
using Random = UnityEngine.Random;

[System.Serializable]
public class ArmSystem
{
	public Transform bone1;
	public Transform bone2;
	public Transform bone3;
}

public class Football_GoalKeeper : MonoBehaviour
{
    public static Football_GoalKeeper instance;

   // public Animator _animator;

	public static Action EventBallHitGK = delegate { };

	public Transform _ball;
	public Transform _ballTarget;	

	public float _weight = 1.0f;
	private Animator _animator;
	public Animator _animatorSave;
	private bool isLook = true;

	public AnimationCurve unityFreeHitWeight; // Workaround for Unity Free users that don't have access to Mecanim curves
	public AnimationCurve distanceWeight; // Workaround for Unity Free users that don't have access to Mecanim curves
	public AnimationCurve distanceWeight1;

	//public ArmSystem armLeft;
	//public ArmSystem armRight;

	public bool isShot = false;
	public bool useLimb = false;
	public bool useFBBIK = true;
	public bool isEvaluateTarget = true;
	public bool updateParams = true;

	public float hitWeight;

	private float _height = 0f;
	private float _distance = 0f;
	public float _direction = 0f;

	public float _minDistance = 0.4f;
	public float _maxDistance = 2.4f;

	public float _minHeight = 0.3f;
	public float _maxHeight = 1.8f;

	public Transform _endPoint;
	public Transform _cachedTrans;

	float _temp;
	float _freeTime = 0.5f;
	float _freeTimeCount = 0;
	public float _predictFactor = 2f;
	private float _startTime;
	public float _delayFactor = 1f;
	public float _goalKeeperZ = -0.5f;

	bool updateFrame;
	public float _speedBall;
	private Vector3 _previousPos;
	private float _deltaPos;
	public float _jumpDuration = 1f; // la the time it take cua hanh` dong bay nguoi can~ fa' cua thu mon, jump duration se duoc tinh' at runtime
	public float _timeLeftForBallToReachEndPoint;   // con bao nhieu giay nua la banh se den' diem~ giao cat'
	public bool _isJumped = false;

	Vector3 posTemp;
	public bool _updateDirection = false;
	public bool _isShootEnd;
	private float trueLength;
	public int _isTouchedTheBall = 1; // = 1 nghia la chua cham banh, 0 nghia la cham roi
	public Material _matGK;
	private void Awake()
    {
        instance = this;
		_cachedTrans = transform;
		_animator = GetComponent<Animator>();
	}

    private void Start()
    {
        Football_Shot.EventShoot += eventShootBegin;
        Football_GoalDetermine.EventFinishShoot += eventShootFinish;

        //_ballTarget = BallTarget.share.transform;
        //_animatorSave = GoalKeeperClone.share.GetComponent<Animator>();
        _ball = Football_ShotAI.intance.transform;


        _previousPos = _ball.position;
        _isShootEnd = true;
    }

	private void eventShootBegin()
	{
		_isJumped = false;
		_isShootEnd = false;
	}

	void OnDestroy()
	{
		Football_Shot.EventShoot -= eventShootBegin;
		Football_GoalDetermine.EventFinishShoot -= eventShootFinish;
	}

	private void eventShootFinish(bool isGoal, Area area)
	{
		_isShootEnd = true;
	}

	public void reset()
    {
        _animator.Play("rig|GK_Idle");
        placeGK();
    }

    private void placeGK()
    {
        Vector3 ballPosition = Football_Shot.intance._ball.transform.position;
        ballPosition.y = 0f;
        Vector3 poleLeft = Football_GoalDetermine.share._poleLeft.position;
        poleLeft.y = 0;
        Vector3 poleRight = Football_GoalDetermine.share._poleRight.position;
        poleRight.y = 0;

        Vector3 intersection = Vector3.zero;
        intersection.x = (poleRight.x + ((poleRight - ballPosition).magnitude / (poleLeft - ballPosition).magnitude) * poleLeft.x) / (1 + ((poleRight - ballPosition).magnitude / (poleLeft - ballPosition).magnitude));
        intersection.z = (poleRight.z + ((poleRight - ballPosition).magnitude / (poleLeft - ballPosition).magnitude) * poleLeft.z) / (1 + ((poleRight - ballPosition).magnitude / (poleLeft - ballPosition).magnitude));

        _cachedTrans.position = Vector3.Lerp(ballPosition, intersection, (_goalKeeperZ - ballPosition.z) / (intersection.z - ballPosition.z));


		Quaternion _lookRotation =
		 Quaternion.LookRotation((Football_ShotAI.instance.transform.position - transform.position).normalized);

		//over time
		transform.rotation =
			Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 100f);

		//instant
		transform.rotation = _lookRotation;
		Debug.Log(string.Format("<color=#c3ff44>Place GK Pos = {0}</color>", _cachedTrans.position.ToString()));
    }

    private void FixedUpdate()
    {
		if (_isShootEnd)
		{
			return;
		}

		updateFrame = true;

		if (isEvaluateTarget)       // tham~ dinh distance, height and direction cua ball
			evaluateTarget();

		if (updateParams && 0f > _ball.GetComponent<Rigidbody>().position.z)            // update parameter dzo animator va animator clone
			updateParamToAnimator();

		if (_isJumped == false)     // check xem khi nao thi fai~ jump
			checkJump();
	}

	public void evaluateTarget()
	{
		float val = _ballTarget.position.x - _cachedTrans.position.x;       // khoang cach giua _ballTarget va thu mon theo truc x (_balltarget la diem giao cat giua~ duong` di cua banh va mat phang ngay vi tri thu mon dung')
		float val1 = val;

		val1 = Mathf.Clamp(val1, -_minDistance, _minDistance);          // tinh' huong' coi left hay right
		_direction = (val1 + _minDistance) / (2 * _minDistance);

		val = Mathf.Abs(val);
		val = Mathf.Clamp(val, _minDistance, _maxDistance);             // tinh khoang cach coi xa hay gan`, thuc ra cung ko can thiet ve class GoalKeeperHorizontalFly da lam nhiem vu tinh khoang cach de tu do' xach dinh xem thu mon fai~ bay theo truc x bao xa
		_distance = (val - _minDistance) / (_maxDistance - _minDistance);

		val = Mathf.Clamp(_ballTarget.position.y, _minHeight, _maxHeight);      // tinh' height
		val = (val - _minHeight) / (_maxHeight - _minHeight);
		_height = val;
	}

	private void updateParamToAnimator()
	{
		if (_animator.enabled)
		{
			if (_updateDirection)
			{
				_animator.SetFloat("direction", _direction);
				//_animatorSave.SetFloat("direction", _direction);
			}

			_animator.SetFloat("distance", _distance);
			//_animatorSave.SetFloat("distance", _distance);


			if (_isTouchedTheBall == 1)
			{       // neu chua cham banh thi van cho cap nhat height
				_animator.SetFloat("height", _height);
				//_animatorSave.SetFloat("height", _height);
			}
		}
	}

    private void checkJump()
    {
		//Debug.Log("1");
		//AnimatorStateInfo info = _animatorSave.GetCurrentAnimatorStateInfo(0);
		//_jumpDuration = info.length * _delayFactor; // + info.length * 0.07f;			// lay jump duration tu` clone wa
		_speedBall = _ball.GetComponent<Rigidbody>().velocity.z;

        if (_speedBall > 0)
			//Debug.Log("2");
		{       // van toc' ball > 0 thi moi' tinh'
            _timeLeftForBallToReachEndPoint = (_ballTarget.position.z - _ball.GetComponent<Rigidbody>().position.z) / _speedBall;       // tinh' thoi gian de~ trai' banh bay den' endpoint
			//Debug.Log(_timeLeftForBallToReachEndPoint +"&& "+ (_timeLeftForBallToReachEndPoint +" ^^ " + _jumpDuration));
			if (_timeLeftForBallToReachEndPoint >= 0 && _timeLeftForBallToReachEndPoint <= _jumpDuration)
            {       // neu' thoi gian nay` <= 1 giay thi` cho thu mon bay nguoi can~ fa'

				//Debug.Log("123");
                _isJumped = true;
                _startTime = (_jumpDuration - _timeLeftForBallToReachEndPoint) * _predictFactor;
                doJump(_startTime);
            }
        }


    }

	private void doJump(float startTime)
	{
		//int a = Random.Range(0, 2);

		//if (a == 0 )
		//      {
		//	_animator.Play("rig|GK_Catch5", 0, startTime);
		//}
		//else
		//      {
		//	_animator.Play("post-save", 0, startTime);
		//}
		Debug.Log(_distance);
        if (_distance < 0.1f)
        {
            _animator.Play("rig|GK_Catch5", 0, startTime);
        }
        else
        {
            _animator.Play("post-save", 0, startTime);
        }
        isShot = true;
	}

	

	public void setUniform(Country country)
	{
		_matGK.mainTexture = (Texture2D)Resources.Load("Uniform/UniformFootball_" + country.ToString());
	}
}
