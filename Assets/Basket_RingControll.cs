using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket_RingControll : MonoBehaviour
{
    [SerializeField] GameObject net;
    [SerializeField] ParticleSystem fireRing;
    Cloth netCloth;

    private void Start()
    {
        netCloth = net.GetComponent<Cloth>();
        fireRing.Play();
    }

    //public void AddColliderCloth(Collider ball)
    //{

    //    // Kiểm tra nếu ball là SphereCollider
    //    SphereCollider sphereCollider = ball as SphereCollider;
    //    if (sphereCollider == null)
    //    {
    //        Debug.LogError("Collider không phải là SphereCollider.");
    //        return;
    //    }

    //    // Lấy danh sách hiện tại của SphereColliders
    //    SphereCollider[] currentColliders = netCloth.sphereColliders;

    //    // Tạo một mảng mới với kích thước lớn hơn
    //    SphereCollider[] newColliders = new SphereCollider[currentColliders.Length + 1];

    //    // Sao chép các colliders hiện tại vào mảng mới
    //    for (int i = 0; i < currentColliders.Length; i++)
    //    {
    //        newColliders[i] = currentColliders[i];
    //    }

    //    // Thêm collider mới vào cuối mảng
    //    newColliders[currentColliders.Length] = sphereCollider;

    //    // Gán mảng mới cho Cloth
    //    netCloth.sphereColliders = newColliders;
    //}

    //public void ClearColliderCloth()
    //{
    //    netCloth.sphereColliders = new SphereCollider[];
    //}
}
