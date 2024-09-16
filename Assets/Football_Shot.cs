using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Football_Shot : MonoBehaviour
{
    public static Football_Shot intance;

    public static Action EventShoot = delegate { };
    public static Action<float> EventChangeSpeedZ = delegate { };
    public static Action<float> EventChangeBallZ = delegate { };
    public static Action<float> EventChangeBallX = delegate { };
    public static Action<float> EventChangeBallLimit = delegate { };
    public static Action<Collision> EventOnCollisionEnter = delegate { };
    public static Action EventDidPrepareNewTurn = delegate { };


    public float _ballControlLimit;

    public Transform _goalKeeper;
    public Transform _ballTarget;
    protected Vector3 beginPos;
    protected bool _isShoot = false;

    public float minDistance = 100;     // 40f


    public Rigidbody _ball;
    public float factorUp = 0.012f;             // 10f
    public float factorDown = 0.003f;           // 1f
    public float factorLeftRight = 0.025f;		// 2f
    public float factorLeftRightMultiply = 0.8f;        // 2f
    public float _zVelocity = 24f;

    public AnimationCurve _curve;
    protected Camera _shootCamera;

    protected float factorUpConstant = 0.017f * 960f;   // 0.015f * 960f;
    protected float factorDownConstant = 0.006f * 960f; // 0.005f * 960f;
    protected float factorLeftRightConstant = 0.0235f * 640f; // 0.03f * 640f; // 0.03f * 640f;

    public Transform _ballShadow;


    public float _speedMin = 18f;   // 20f;
    public float _speedMax = 30f;   // 36f;

    public float _distanceMinZ = 16.5f;
    public float _distanceMaxZ = 35f;

    public float _distanceMinX = -25f;
    public float _distanceMaxX = 25f;

    public bool _isShooting = false;
    public bool _canControlBall = false;

    public Transform _cachedTrans;

    public bool _enableTouch = false;
    public float screenWidth;
    public float screenHeight;

    Vector3 _prePos, _curPos;
    public float angle;
    protected ScreenOrientation orientation;

    protected Transform _ballParent;

    protected RaycastHit _hit;
    public bool _isInTutorial = false;
    public Vector3 ballVelocity;

    private float _ballPostitionZ = -22f;
    private float _ballPostitionX = 0f;

    public float BallPositionZ
    {
        get { return _ballPostitionZ; }
        set { _ballPostitionZ = value; }
    }

    public float BallPositionX
    {
        get { return _ballPostitionX; }
        set { _ballPostitionX = value; }
    }
    public TrailRenderer _effect;

    /// <summary>
    /// ////////////////////////Add new
    /// </summary>
    /// 
    public Direction _shootDirection = Direction.Both;
    private List<Vector3> _ballPath;
    public float _curveLevel = 0;
    public float _difficulty = 0.5f;
    private List<Vector3> _debugPath = new List<Vector3>();
    public int _index;
    private MyMultiSelection _multiSelection;
    public float _maxLeft = 0f;
    public float _maxRight = 0f;
    public Interpolate.EaseType easeType = Interpolate.EaseType.EaseInOutCubic;
    public AnimationCurve _animationCurve;

    public float _remainTimeToNextPointZ;
    private float yVelocityTarget;
    private float angleZY;
    private bool _isDown;
    protected virtual void Awake()
    {
        intance = this;
        
        _cachedTrans = transform;
        _isShooting = true;
        _ballParent = _ball.transform.parent;

        _enableTouch = true;
        _isInTutorial = true;

        _distanceMinX = -15f;
        _distanceMaxX = 15f;
        _distanceMaxZ = 30f;

        _ballPath = new List<Vector3>();
        _multiSelection = GetComponent<MyMultiSelection>();
    }

    protected virtual void Start()
    {
        _shootCamera = Football_CameraManagerN.instance._cameraMainComponent;

#if UNITY_WP8 || UNITY_ANDROID
        Time.maximumDeltaTime = 0.2f;
        Time.fixedDeltaTime = 0.008f;
#else
		Time.maximumDeltaTime = 0.1f;
		Time.fixedDeltaTime = 0.005f;
#endif
        orientation = Screen.orientation;
        calculateFactors();
        EventChangeBallLimit(_ballControlLimit);

        reset();
        Football_CameraManagerN.instance.reset();
        Football_Shooter.instance.reset();
        Football_GoalKeeper.instance.reset();

        Football_GoalDetermine.EventFinishShoot += goalEvent;
        
    }

    private void OnEnable()
    {
        Football_Shooter.EventShoot += NormalShot;
    }

    private void OnDisable()
    {
        Football_Shooter.EventShoot -= NormalShot;
    }

    public virtual void goalEvent(bool isGoal, Area area)
    {
        _canControlBall = false;
        _isShooting = false;
    }

    public void calculateFactors()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        minDistance = (100 * screenHeight) / 960f;
        factorUp = factorUpConstant / screenHeight *1.5f;
        factorDown = factorDownConstant / screenHeight;
        factorLeftRight = factorLeftRightConstant / screenWidth;

        Debug.Log("Orientation : " + orientation + "\t Screen height = " + screenHeight
            + "\t Screen width = " + screenWidth + "\t factorUp = " + factorUp + "\t factorDown = " + factorDown
            + "\t factorLeftRight = " + factorLeftRight + "\t minDistance = " + minDistance);
    }
    protected void LateUpdate()
    {
        if (screenHeight != Screen.height)
        {
            orientation = Screen.orientation;
            calculateFactors();
            //CameraManager.share.reset();
        }
    }

    protected virtual void FixedUpdate()
    {
        ballVelocity = _ball.velocity;

        Vector3 pos = _ball.transform.position;
        pos.y = 0.015f;
        //_ballShadow.position = pos;   
    }

    protected virtual void Update()
    {
        if (_isShooting)
        {

            if (_enableTouch && _isInTutorial)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    mouseBegin(Input.mousePosition);
                }
                else if (Input.GetMouseButton(0))
                {
                    mouseMove(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {

                    mouseEnd(Input.mousePosition);
                }
            }
            if (_isShoot1)
            {
               // Debug.Log("aaaaaaaaaaa");
                while (_cachedTrans.position.z > _ballPath[_index].z && _index >= 1)
                {
                    --_index;
                }

                if (_cachedTrans.position.z > _ballPath[_index].z && --_index < 0)
                {       // neu vi tri cua banh da~ vuot wa diem dang xet va khong con diem~ de xet' thi stop
                    _isShoot1 = false;
                }
                else
                {
                    // dai y' : lay' diem dang xet ra, tinh' toan' goc', tu do' tinh' toan van toc' theo x,y,z lam sao de banh bay duoc den' diem~ dang xet'


                    Vector3 temp = (_ballParent.InverseTransformPoint(_ballPath[_index]) - _cachedTrans.localPosition).normalized;      // convert vi tri diem dang xet ve he toa do cua parent cua trai banh, lay vector chi phuong
                    float angleZX = Mathf.Atan2(temp.x, temp.z);            // tinh goc'
                    angleZY = Mathf.Atan2(temp.y, temp.z);

                    float xVelocityTarget = _zVelocity * Mathf.Tan(angleZX);        // tinh van toc theo truc x
                                                                                    //				xVelocityTarget = Mathf.Clamp(xVelocityTarget, -factorLeftRightConstant * 0.7f, factorLeftRightConstant * 0.7f);		// clamp van toc theo truc x voi' cac' limit, nho` co' dong` nay` ma` ve mat ngu~ nghia~ may' ko choi an gian, no' cung bi gioi' han nhu nguoi` choi, ko fai muon' xoay' sao thi xoay'

                    float tempVel = _zVelocity * Mathf.Tan(angleZY);            // tinh van toc theo truc y

                    if (tempVel < yVelocityTarget)
                    {           // check xem banh dang di xuong fai ko
                        if (_isDown == false)
                        {               // neu o~ frame truoc do' banh con dang di len
                            _isDown = true;
                        }
                    }
                    else
                    {                   // banh dang di len
                        _isDown = false;
                    }

                    //				yVelocityTarget = Mathf.Clamp(tempVel, _yVelocityLimitDown, factorUpConstant);
                    yVelocityTarget = tempVel;

                    angleZY = angleZY * Mathf.Rad2Deg;

                    //				_ball.velocity = _ballParent.TransformDirection(new Vector3(xVelocityTarget, yVelocityTarget, _zVelocity));
                    _remainTimeToNextPointZ = Mathf.Abs((_ballPath[_index].z - _cachedTrans.position.z)) / _ball.velocity.z;        // thoi gian con lai de trai banh di den diem dang xet voi' van toc' z hien tai

                    Vector3 speed = _ballParent.InverseTransformDirection(_ball.velocity);          // convert van toc global cua trai banh ve he truc toa do cua cha trai' banh
                    speed.z = _zVelocity;
                    speed.x = Mathf.Lerp(speed.x, xVelocityTarget, _remainTimeToNextPointZ);
                    speed.y = Mathf.Lerp(speed.y, yVelocityTarget, _remainTimeToNextPointZ);

                    _ball.velocity = _ballParent.TransformDirection(speed);         // convert nguoc lai thanh van toc global roi gan' cho trai banh
                                                                                    //				_ball.angularVelocity = new Vector3(0, xVelocityTarget, 0f);

                }
            }
        }
    }
    public void mouseBegin(Vector3 pos)
    {
        startPos =_prePos = _curPos = pos;
        beginPos = _curPos;
        Debug.Log("123");
    }
    
    
    //float angle1;
    float x1;
    Vector3 distance1;
    bool _isShoot1 = false;
    private Vector3 startPos, endPos;
    public void NormalShot()
    {
        shoot();
       // _isShoot1 = true;
       // _ball.velocity = _ballParent.TransformDirection(new Vector3(x1, distance1.y * factorUp, _zVelocity));
       // _ball.angularVelocity = new Vector3(0, x1, 0f);

        //Debug.Log("x" + x1);
       // Debug.Log("distance" + distance1);
    }
    public void mouseMove(Vector3 pos)
    {
        if (_curPos != pos)
        {       // touch phase moved
            _prePos = _curPos;
            _curPos = pos;


            //Vector3 distance = _curPos - beginPos;

            //if (_isShoot == false)
            //{               // CHUA SUT
            //    if (distance.y > 0 && distance.magnitude >= minDistance)
            //    {
            //        if (Physics.Raycast(_shootCamera.ScreenPointToRay(_curPos), out _hit, 100f) && !_hit.transform.tag.Equals("ballTag"))
            //        {
            //            _isShoot = true;

            //            Vector3 point1 = _hit.point;        // contact point
            //            point1.y = 0;
            //            point1 = _ball.transform.InverseTransformPoint(point1);     // dua point1 ve he truc toa do cua ball, coi ball la goc toa do cho de~
            //            point1 -= Vector3.zero;         // vector tao boi point va goc' toa do

            //            Vector3 diff = point1;
            //            diff.Normalize();               // normalized rat' quan trong khi tinh' goc

            //            float angle1 = 90 - Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;     // doi ra degree va lay 90 tru` vi nguoc
            //            //                                                                    //								Debug.Log("angle = " + angle);

            //            float x = _zVelocity * Mathf.Tan(angle * Mathf.Deg2Rad);
            //            x1 = x;
                        
            //            distance1 = distance;

            //           // Debug.Log(x1);
            //            //Debug.Log(distance1);

            //            ////							float x = distance.x * factorLeftRight;
            //            //_ball.velocity = _ballParent.TransformDirection(new Vector3(x, distance.y * factorUp, _zVelocity));
            //            //_ball.angularVelocity = new Vector3(0, x, 0f);

            //            //if (EventShoot != null)
            //            //{
            //            //    EventShoot();
            //            //}
            //        }
            //    }
            //}
            //else
            //{               // da~ sut' roi`, tuy theo do lech cua touch frame hien tai va truoc do' ma se lam cho banh xoay' trai', phai~, len va xuong' tuong ung'
            //    //if (_canControlBall == true)
            //    //{   // neu nhac ngon tay len khoi man hinh roi thi ko cho dieu khien banh nua

            //    //    if (_cachedTrans.position.z < -_ballControlLimit)
            //    //    {
            //    //        // neu banh xa hon khung thanh 6m thi moi' cho dieu khien banh xoay, di vo trong khoang cach 6m thi ko cho nua~ de~ lam cho game can bang`

            //    //        distance = _curPos - _prePos;

            //    //        Vector3 speed = _ballParent.InverseTransformDirection(_ball.velocity);
            //    //        speed.y += distance.y * ((distance.y > 0) ? factorUp : factorDown);
            //    //        speed.x += distance.x * factorLeftRight * factorLeftRightMultiply;
            //    //        _ball.velocity = _ballParent.TransformDirection(speed);

            //    //        speed = _ball.angularVelocity;
            //    //        speed.y += distance.x * factorLeftRight;
            //    //        _ball.angularVelocity = speed;
            //    //    }
            //    //    else
            //    //    {
            //    //        _canControlBall = false;
            //    //    }
            //    //}
            //}
        }
    }






    public void mouseEnd(Vector3 pos)
    {
        //Vector3 hitPoint;

        //// Tạo điểm bắt đầu của tia ray dựa trên vị trí chuột và vị trí camera
        //Vector3 startPoint = Football_CameraManagerN.instance._cameraMainComponent.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Football_CameraManagerN.instance._cameraMainComponent.nearClipPlane));

        //// Lấy hướng forward của camera để tạo tia ray
        //Vector3 rayDirection = Football_CameraManagerN.instance._cameraMainComponent.transform.forward;

        //// Tạo một tia ray theo hướng forward của camera
        //Ray ray = new Ray(startPoint, rayDirection);
        //RaycastHit hit;
        endPos = pos;
        Vector3 hitPoint;
        Ray ray = Football_CameraManagerN.instance._cameraMainComponent.ScreenPointToRay(pos);
        RaycastHit hit;
        Debug.Log(Vector3.Distance(startPos, endPos));
        if (Vector3.Distance(startPos, endPos) <= 200f) return;
        
            // Thực hiện raycast và kiểm tra xem tia ray có va chạm với đối tượng nào không
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // Kiểm tra xem đối tượng va chạm có tag "goalcheck" không
                if (hit.collider.CompareTag("shootpoint"))
                {
                    // Lấy điểm va chạm
                    hitPoint = hit.point;

                    // Hiển thị điểm va chạm hoặc thực hiện hành động nào đó
                    //Debug.Log("Hit point: " + hitPoint);
                    pointEnd = new Vector3(hitPoint.x, hitPoint.y, UnityEngine.Random.Range(hitPoint.z - 0.5f, hitPoint.z + 0.5f));

                }
            }
            if (_prePos.x <= _curPos.x)
            {
                _shootDirection = Direction.Left;
            }
            else
            {
                _shootDirection = Direction.Right;
            }
            //pointEnd = hitPoint;
            if (EventShoot != null)
            {
                EventShoot();
            }
            //pointEnd = pos;
            //if (_isShoot == true)
            //{       // neu da sut' roi` thi ko cho dieu khien banh nua, tranh' truong` hop nguoi choi tao ra nhung cu sut ko the~ do~ noi~
            //        //_canControlBall = false;
            //}
        
    }

    public void shoot()
    {
        shoot(_shootDirection, _curveLevel, _difficulty);
    }

    public void shoot(Direction shootDirection, float curveLevel, float difficulty)
    {
        //		Debug.Log("AI shoot");
        //EventShoot();

         _isShoot1 = true;

        _ballPath.Clear();

        _ballPath = createPath(shootDirection, curveLevel, difficulty, _cachedTrans.position);
        _index = _ballPath.Count - 1;           // di nguoc tu cuoi' mang~ ve dau` mang~

        /*************** Debug ****************/
        _multiSelection.count = _ballPath.Count;
        for (int i = 0; i < _multiSelection._gos.Count; ++i)
        {
            Destroy(_multiSelection._gos[i]);
        }
        _multiSelection._gos.Clear();



        for (int i = 0; i < _ballPath.Count; ++i)
        {
            GameObject go = new GameObject();
            go.transform.position = _ballPath[i];
            _multiSelection._gos.Add(go);

        }
        /*******************************/
    }
    Vector3 pointEnd;
    public List<Vector3> createPath(Direction direction, float curveLevel, float difficulty, Vector3 ballPostion)
    {
        float ballDistance = new Vector2(Mathf.Abs(ballPostion.x), Mathf.Abs(ballPostion.z)).magnitude;
        int slide = 10;
        List<Vector3> listTemp = new List<Vector3>();

       // Vector3 pointEnd = Vector3.zero;                            // diem dich' den'
        Vector3 pointMiddle = Vector3.zero;
        Vector3 pointStart = ballPostion;           // diem bat dau

        if (direction == Direction.Both)
            direction = (Direction)UnityEngine.Random.Range(0, 2);



        // *********************** Tinh yMid va yEnd ************************
        float yMidMin = 0.145f;
        float yMidMax = 4.3f;
        float yEnd_Min_When_Mid_Min = 0f, yEnd_Max_When_Mid_Min = 0f;
        float yEnd_Min_When_Mid_Max = 0f, yEnd_Max_When_Mid_Max = 0f;
        float yMid = 0f;
        float yEnd = 0f;

        bool found = false;

        // y nghia: tu thuc nghiem ta thu duoc ket wa khi sut banh o khoang cach' tu` 35m ve` 16.5m
        // moi ket wa gom cac thong so:
        // distance : khoang cach
        // yMidMin : yMin cua pointMiddle 
        // yEnd_Min_When_Mid_Min	: yMin cua pointEnd khi yMiddle dat minimum
        // yEnd_Max_When_Mid_Min	: yMax cua pointEnd khi yMiddle dat minimum
        // yMidMax : yMax cua diem middle 
        // yEnd_Min_When_Mid_Min	: yMin cua pointEnd khi yMiddle dat maximum
        // yEnd_Max_When_Mid_Min	: yMax cua pointEnd khi yMiddle dat maximum

        //	Cach su dung:
        // co' vi tri dat banh, tinh duoc distance dat banh den goc toa do.
        // tu distance se suy ra duoc no' nam trong Range nao`. Duyet mang~ de lay ra cai' Range do'
        // Co' Range roi` tinh' t roi Lerp de ra duoc cac' thong so' nhu tren ung' voi khoang cach dat banh.
        // Co thong so roi` thi random de tinh yMid, roi tu yMid se tinh duoc yEndMin va yEndMax va roi la yEnd


        for (int i = 1; i < dataShootAI.Length; ++i)
        {       // dataShootAI la mang du~ lieu thu duoc tu` thuc nghiem
            DataShootAI data = dataShootAI[i];
            if (ballDistance <= data._distance)
            {           // check xem khoang cach fu hop de xet chua
                found = true;
                DataShootAI preMadeData = dataShootAI[i - 1];

                float t = (ballDistance - preMadeData._distance) / (data._distance - preMadeData._distance);

                yMidMin = Mathf.Lerp(preMadeData._yMid_Min, data._yMid_Min, t);
                yMidMax = Mathf.Lerp(preMadeData._yMid_Max, data._yMid_Max, t);
                yEnd_Min_When_Mid_Min = Mathf.Lerp(preMadeData._yEnd_Min_When_Mid_Min, data._yEnd_Min_When_Mid_Min, t);
                yEnd_Max_When_Mid_Min = Mathf.Lerp(preMadeData._yEnd_Max_When_Mid_Min, data._yEnd_Max_When_Mid_Min, t);
                yEnd_Min_When_Mid_Max = Mathf.Lerp(preMadeData._yEnd_Min_When_Mid_Max, data._yEnd_Min_When_Mid_Max, t);
                yEnd_Max_When_Mid_Max = Mathf.Lerp(preMadeData._yEnd_Max_When_Mid_Max, data._yEnd_Max_When_Mid_Max, t);

                yMid = UnityEngine.Random.Range(yMidMin, yMidMax);

                t = Mathf.Abs(yMid - yMidMin) / Mathf.Abs(yMidMax - yMidMin);
                float yEndMin = Mathf.Lerp(yEnd_Min_When_Mid_Min, yEnd_Min_When_Mid_Max, t);
                float yEndMax = Mathf.Lerp(yEnd_Max_When_Mid_Min, yEnd_Max_When_Mid_Max, t);

                yEnd = UnityEngine.Random.Range(yEndMin, yEndMax);

                break;
            }
        }

        if (found == false)
        {       // neu for loop o tren ma ko thoa du chi 1 lan co' nghia~ la vi tri' da' banh >= 35m, nhu vay lay du lieu cua 35m ra doi` dung` lien`, ko can fai lerp nhu tren
            DataShootAI data = dataShootAI[dataShootAI.Length - 1];   // dataShootAI.Length - 1 la vi tri du~ lieu khi sut banh o vi tri 35m
            yMid = UnityEngine.Random.Range(data._yMid_Min, data._yMid_Max);            // random ra yMid

            float t = Mathf.Abs(yMid - data._yMid_Min) / Mathf.Abs(data._yMid_Max - data._yMid_Min);
            float yEndMin = Mathf.Lerp(data._yEnd_Min_When_Mid_Min, data._yEnd_Min_When_Mid_Max, t);            // co yMid roi se tinh duoc yEndMin
            float yEndMax = Mathf.Lerp(data._yEnd_Max_When_Mid_Min, data._yEnd_Max_When_Mid_Max, t);            // co yMid roi se tinh duoc yEndMax

            yEnd = UnityEngine.Random.Range(yEndMin, yEndMax);          // random ra y end
        }
        // ***********************************************


        // *********************** Point End ************************
        float xTemp;
        float xMin = Mathf.Lerp(0, 3.3f, difficulty);
        //		float xMax = Mathf.Lerp(3.6f, 3.5f, (ballDistance - 16f) / (22f - 16f));		// banh cang gan 16m thi cho xmax cang ra xa, tai vi cuoi ham nay` minh se fai remove nhung diem co' z gan` khung thanh 3m hoac nho hon
        float xMax = 3.45f;

        if (direction == Direction.Right)
            xTemp = UnityEngine.Random.Range(xMin, xMax);
        else
            xTemp = UnityEngine.Random.Range(-xMax, -xMin);

       // pointEnd = new Vector3(xTemp, yEnd, 0f);        // diem dau tien duoc add vo List cung la diem cuoi' cung se duoc lay ra
        //pointEnd = new Vector3(xTemp, yEnd, 0f);        // diem dau tien duoc add vo List cung la diem cuoi' cung se duoc lay ra
                                                        //		pointEnd.y = yEnd;
                                                        //		pointEnd.x = xEnd;
                                                        // ***********************************************


        // ********************** Tim x la do be~ cong  *************************
        float x = (pointEnd.x + pointStart.x) / 2f;

        if (Mathf.Abs(pointStart.z) <= 32f)
        {
            // khi sut ra 2 goc' 2 ben cot doc thi fai co' gioi' han ve` do xoay' ra ngoai`. Do xoay' trong thi ko can care vi = 3 van an toan, banh ko bi vang ra ngoai
            // do xoay' ngoai duoc gioi' han lai theo cong thuc duoi' day khi z cua banh cach khung thanh <= 30m, > 30m thi do xoay' ngoai co' the bang maximum va van an toan, banh bao dam xoay' dzo goal
            // y tuong cua cong thuc la wa thuc nghiem ta biet duoc do xoay' ngoai an toan khi sut banh ra 2 goc (x = 3.4f hay = -3.4f) o khoang cach 30m la 3m, 16.5m la 0m. Ta se dung Mathf.Lerp de noi suy
            // khi sut banh gan` vi tri giua~ thi do xoay' ngoai` di~ nhien se duoc tang len cao hon ma ko so banh xoay' ra ngoai khung thanh, do' la ly' do tai sao co' 2 dong Lerp o duoi'

            float maxCurve = Mathf.Lerp(0, 3.5f, _animationCurve.Evaluate((Mathf.Abs(pointStart.z) - 16.5f) / (32f - 16.5f))); // maxcurve la do xoay ngoai` toi' da khi da' vao` diem~ co' x = 3.4 hoac -3.4
            _maxRight = Mathf.Lerp(3.5f, maxCurve, pointEnd.x / (3.4f));
            _maxLeft = Mathf.Lerp(-3.5f, -maxCurve, pointEnd.x / (-3.4f));
        }
        else
        {
            _maxLeft = -3.5f;
            _maxRight = 3.5f;
        }

        float curveFactor = Mathf.Lerp(0f, 0.95f, curveLevel);          // do kho cua xoay'
        float a = UnityEngine.Random.Range(_maxLeft, _maxLeft * curveFactor);       // random xoay ben fai
        float b = UnityEngine.Random.Range(_maxRight * curveFactor, _maxRight);     // random xoay ben trai
        float xCurve = (((int)UnityEngine.Random.Range(0, 2)) == 0) ? a : b;                // random chon xoay ben fai hay ben trai

        //		float xCurve = Mathf.Lerp(_maxLeft, _maxRight, curveLevel);					// do be cong duong di cua banh

        ////////////////// EventChangeCurveRange(xCurve);
        x += xCurve;        // lam cho x cua diem giua~ bi lech se duoc ket qua la lam cho duong banh bi be cong xoay'
                            // ***********************************************


        listTemp.Clear();
        if (pointStart.z >= -22f)
        {           // neu nhu diem dat banh cach' khung thanh 22m do~ lai thi tinh diem middle thuc su
            pointMiddle.x = x;
            pointMiddle.y = yMid;
            pointMiddle.z = (pointEnd.z + pointStart.z) / 2f;
        }
        else
        {   // neu nhu diem dat banh cach' khung thanh > 22m thi coi diem dat hang rao la diem middle, ket qua~ cug~ ok ko sao.
            pointMiddle.z = pointStart.z + 11f;     // z tai cho dat hang rao
            pointMiddle.x = x;
            pointMiddle.y = yMid;
        }

        // cac buoc tren la ap dung cho khi co' hang rao, neu' ko co' hang rao thi` chi can dzo if nay la ok, moi thu van dung. ko can chia 2 truong hop cho cac buoc tren
        if (Football_Wall.instance != null && !Football_Wall.instance.IsWall)
        {       // ko co hang rao
            pointEnd.y = UnityEngine.Random.Range(0.145f, 2.8f);
            pointMiddle.y = UnityEngine.Random.Range(0.145f, 4.3f);
            pointMiddle.z = (pointEnd.z + pointStart.z) / 2f;
        }

        listTemp.Clear();
        listTemp.Add(pointEnd);
        listTemp.Add(pointMiddle);
        listTemp.Add(pointStart);

        // ******************* tim diem cach khung thanh mot khoang cach mindistance *********************
        Vector3 closestPointToGoal = findPointInPathAtZ(Interpolate.Ease(easeType), listTemp.ToArray(), slide, -3f);
        // ****************************************


        // ******************** Final path ********************
        List<Vector3> retVal = new List<Vector3>();
        IEnumerator<Vector3> nodes = Interpolate.NewBezier(Interpolate.Ease(easeType), listTemp.ToArray(), slide).GetEnumerator();      // tao ra path gom cac diem
        _debugPath.Clear();

        while (nodes.MoveNext())
        {           // lay cac diem cua path ra
            _debugPath.Add(nodes.Current);

            //if(nodes.Current.z < -3f)
            if (nodes.Current.z < -_ballControlLimit)
                retVal.Add(nodes.Current);
        }
        retVal.Insert(0, closestPointToGoal);
        retVal.RemoveAt(retVal.Count - 1);

        //if( Vector3.Distance(retVal[0], retVal[1]) <= 4f)
        //    retVal.RemoveAt(1);
        // ****************************************


        return retVal;
    }


    private DataShootAI[] dataShootAI = new DataShootAI[] {
		// distance, yMid_Min, yEnd_Min_When_Mid_Min, yEnd_Max_When_Mid_Min, yMid_Max, yEnd_Min_When_Mid_Max, yEnd_Max_When_Mid_Max
		
		new DataShootAI(16.5f, 2.6f, 2.1f, 2.4f, 4.3f, 0.145f, 0.76f)
        ,new DataShootAI(19f, 2.8f, 2.2f, 2.4f, 4.3f, 0.145f, 1.5f)
        ,new DataShootAI(21f, 2.8f, 2.2f, 2.4f, 4.3f, 0.145f, 1.8f)
        ,new DataShootAI(23f, 2.8f, 2.3f, 2.55f, 4.3f, 0.145f, 2.2f)
        ,new DataShootAI(27f, 3f, 1.8f, 2.6f, 4.3f, 0.145f, 2.6f)
        ,new DataShootAI(35f, 3.2f, 1.6f, 2.8f, 4.3f, 0.145f, 2.8f)
    };

    private Vector3 findPointInPathAtZ(Interpolate.Function ease, Vector3[] points, int slide, float z)
    {
        Vector3 pointBefore = Vector3.zero;
        Vector3 pointAfter = Vector3.zero;

        List<Vector3> temp = new List<Vector3>();

        IEnumerator<Vector3> nodes = Interpolate.NewBezier(ease, points, slide).GetEnumerator();        // tao ra path gom cac diem
        while (nodes.MoveNext())
        {           // lay cac diem cua path ra
            temp.Add(nodes.Current);
        }
        for (int i = 0; i < temp.Count; ++i)
        {           // tiem diem truoc' va sau diem hang rao
            if (temp[i].z < z)
            {
                pointBefore = temp[i];
                pointAfter = temp[i - 1];
                break;
            }
        }
        return Vector3.Lerp(pointBefore, pointAfter, (z - pointBefore.z) / (pointAfter.z - pointBefore.z));
    }

    protected void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if (tag.Equals("Player") || tag.Equals("Obstacle") || tag.Equals("Net") || tag.Equals("Wall"))
        {   // banh trung thu mon hoac khung thanh hoac da vao luoi roi thi ko cho banh bay voi van toc nua, luc nay de~ cho physics engine tinh' toan' quy~ dao bay
            _isShooting = false;

            if (tag.Equals("Net"))
            {
                _ball.velocity /= 3f;
            }
        }

        EventOnCollisionEnter(other);
    }

    private void enableEffect()
    {
        //		_effect.enabled = true;
        _effect.time = 1;
    }

    public virtual void reset()
    {
        reset(-UnityEngine.Random.Range(_distanceMinX, _distanceMaxX), -UnityEngine.Random.Range(_distanceMinZ, _distanceMaxZ));
        //reset(0, -6.06f);

    }

    public virtual void reset(float x, float z)
    {
        Debug.Log(string.Format("<color=#c3ff55>Reset Ball Pos, x = {0}, z = {1}</color>", x, z));

        _effect.time = 0;
        // _effect.enabled = false;
        Invoke(nameof(enableEffect), 0.1f);
        BallPositionX = x;
        EventChangeBallX(x);
        BallPositionZ = z;
        EventChangeBallZ(z);

        _canControlBall = true; //** có thể điều khiển bóng
        _isShoot = false; //** đã sút 
        _isShooting = true; // đang sút hay chưa

        _ball.velocity = Vector3.zero;
        _ball.angularVelocity = Vector3.zero;
        _ball.transform.localEulerAngles = Vector3.zero;

        Vector3 pos = new Vector3(BallPositionX, 0.16f, BallPositionZ);
        Vector3 diff = -pos;
        diff.Normalize();
        float angleRadian = Mathf.Atan2(diff.z, diff.x); // tính góc lệch
        float angle = 90 - angleRadian * Mathf.Rad2Deg;

        _ball.transform.position = new Vector3(BallPositionX, 0.16f, BallPositionZ);

        pos = _ballTarget.position;
        pos.x = 0;
        _ballTarget.position = pos;

        float val = (Mathf.Abs(_ball.transform.localPosition.z) - _distanceMinZ) / (_distanceMaxZ - _distanceMinZ);
        _zVelocity = Mathf.Lerp(_speedMin, _speedMax, val); // k cho val vượt quá Min max
        // Những sự kiên đăng kí khi xong reset;
        EventChangeSpeedZ(_zVelocity);
        EventDidPrepareNewTurn();
    }

    public void enableTouch()
    {
        _enableTouch = true;
    }

    public void disableTouch()
    {
        StartCoroutine(_disableTouch());
    }

    private IEnumerator _disableTouch()
    {
        yield return new WaitForEndOfFrame();
        _enableTouch = false;
    }
}
