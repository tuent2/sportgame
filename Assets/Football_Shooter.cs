using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using UnityEngine.SceneManagement;
public class Football_Shooter : MonoBehaviour
{
    public static Football_Shooter instance;
    public static Action EventShoot = delegate { };
    [SerializeField] Vector3 _distanceBall;
    [SerializeField] Transform _shootedPoint;
    [SerializeField] bool isShooterRight;
    [SerializeField] Animator _animator;
    private void Awake()
    {
        instance = this;
        _distanceBall = new Vector3(1.20f, 0.14f, 1.93f);
        isShooterRight = true;
    }
    private void OnEnable()
    {
        Football_Shot.EventShoot += eventShoot;
    }

    private void OnDisable()
    {
        Football_Shot.EventShoot -= eventShoot;
    }
    public void reset()
    {
        Transform ball = Football_Shot.intance._ball.transform;

        Vector3 diff = -ball.position;
        diff.Normalize();
        float angleRadian = Mathf.Atan2(diff.x, diff.z);
        float angle = angleRadian * Mathf.Rad2Deg;          // goc lech so voi goc toa do
        angleRadian = angle * Mathf.Deg2Rad;

        Vector3 pos = ball.position;        // pos se duoc gan' la vi tri cua camera
        //pos.y = cameraMainY;

        //if (isPortrait)
        //{       // neu la portrait thi camera nam dang sau trai banh 4m va huong ve goc toa do, noi cach khac' la cung huong' voi' truc z cua parent cua ball
        //    pos.x += cameraMainDistanceToBall * Mathf.Sin(angleRadian);
        //    pos.z += cameraMainDistanceToBall * Mathf.Cos(angleRadian);
        //}
        //else
        //{       // neu la landscape thi camera nam dang sau trai banh 4m va huong ve goc toa do, noi cach khac' la cung huong' voi' truc z cua parent cua ball
        //    pos.x += cameraMainDistanceToBall * Mathf.Sin(angleRadian);
        //    pos.z += cameraMainDistanceToBall * Mathf.Cos(angleRadian);
        //}
        
        transform.position = pos - _distanceBall;

        //  float distanceBackCameraToBall = 10f; // 18.31871f;

        // float x = distanceBackCameraToBall * Mathf.Sin(angleRadian);
        // float z = distanceBackCameraToBall * Mathf.Cos(angleRadian);

        //if (_cameraBack)
        //{
        //    _cameraBack.transform.position = new Vector3(x, yBackCamera, z);

        //    Vector3 posGK = Football_GoalKeeper.instance.transform.position;
        //    posGK.y = yBackCamera;

        //    Quaternion rotationLook = Quaternion.LookRotation(Football_Shot.intance._cachedTrans.position - _cameraBack.transform.position);
        //    _cameraBack.transform.rotation = rotationLook;
        //}
        Quaternion _lookRotation =
         Quaternion.LookRotation((Football_GoalKeeper.instance.transform.position - transform.position).normalized);

        //over time
        transform.rotation =
            Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 100f);

        //instant
        transform.rotation = _lookRotation;

        Vector3 distance = _shootedPoint.position - ball.transform.position;
        
        transform.position = transform.position - new Vector3(distance.x , 0, distance.z);
        
    }

    

    [Button("CheckDistance")]
    public void CheckDistance()
    {
        Debug.Log( Football_Shot.intance._ball.transform.position - gameObject.transform.position);
    }


    private void eventShoot()
    {
        _animator.Play("rig|player_shootR");
        StartCoroutine(WaitToShot());
    }

    IEnumerator WaitToShot()
    {
        yield return new WaitForSeconds(0.7f);
        if (EventShoot != null)
        {
            EventShoot();
        }
        
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
        //Football_ShotAI.intance.reset();
        //Football_Shooter.instance.reset();

    }

    
}
