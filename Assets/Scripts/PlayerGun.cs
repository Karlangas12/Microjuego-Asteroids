using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform muzzle;
    public float bulletSpeed = 20f;
    public float fireRate = 0.2f;

    float nextFire = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
    }

    void Fire()
    {
        GameObject bulletObject = ObjectPooler.Instance.SpawnFromPool("PlayerBullet", muzzle.position, muzzle.rotation);
        
        if (bulletObject == null) return; 

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Initialize(muzzle.right, bulletSpeed);
        }
    }
}