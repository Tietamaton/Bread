using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NwBeehaviourScript : MonoBehaviour
{
    public GameObject bullet;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    public bool allowInvoke = true;

    void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

    }

    void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft/bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }


    void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload;

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload;

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

        Ray ray =fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f))
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            float x = Random.Range(-sperad, spread);
            float y = Random.Range(-sperad, spread);

            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            currentBullet.Transform.forward = directionWithSpread.normalized;

            currentBullet.GetComponent<Rigibody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBullet.GetComponent<Rigibody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

            if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        


        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot"; timeBetweenShots);

        private void ResetShot()
        {
            readyToShoot = true;
            allowInvoke = true;
        } 

        private void Reload()
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }

        private void ReloadFinished()
        {
            bulletsLeft = magazineSize;
            reloading = false;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
