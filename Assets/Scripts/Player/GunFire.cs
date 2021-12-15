using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunFire : MonoBehaviour
{
    private AnimationSM ASM;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject bigProjectile;
    [SerializeField] private CalculatePivot Pivot;
    [SerializeField] private float speed;
    [SerializeField] private PlayerAim Aim;

    [SerializeField] private Crosshair crosshair;
    [SerializeField] private float totalOvercharge;
    [SerializeField] private float overchargePerSec;
    [SerializeField] private float overheatTime;
    private float overcharge = 0;
    private float deplete = 0;
    private float overheated = 0;
    [SerializeField] private Slider heatBar, overheatBar;
    [SerializeField] private Image overheatedSign;

    private float gunAmmo = 3;
    [SerializeField] private Image gunAmmo1, gunAmmo2, gunAmmo3;
    [SerializeField] private AudioSource gunShotAudio, rifleShotAudio, rifleReloadAudio;
    private bool gunFiring, rifleFiring;
    void Start()
    {
        gunFiring = false;
        rifleFiring = false;
        ASM = this.GetComponent<AnimationSM>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;
        if (overheated == 0){
            overheatedSign.enabled = false;
            if(overcharge > overchargePerSec * Time.deltaTime) {
            overcharge -= overchargePerSec * Time.deltaTime;
            } else {
                overcharge = 0;
            }
        } else {
            overheatedSign.enabled = true;
            overheated -= Time.deltaTime;
            if (overheated <= 0) overheated = 0;
        }
        
        if(gunAmmo < 3) {
            gunAmmo += 0.5f * Time.deltaTime;
            if(gunAmmo > 3) gunAmmo = 3;
        }
        
        
        if(gunAmmo == 3){
            gunAmmo3.enabled = true;
            gunAmmo2.enabled = true;
            gunAmmo1.enabled = true;
        } else if (gunAmmo >= 2) {
            gunAmmo3.enabled = false;
            gunAmmo2.enabled = true;
            gunAmmo1.enabled = true;
        } else if (gunAmmo >= 1) {
            gunAmmo3.enabled = false;
            gunAmmo2.enabled = false;
            gunAmmo1.enabled = true;
        } else {
            gunAmmo3.enabled = false;
            gunAmmo2.enabled = false;
            gunAmmo1.enabled = false;
        }

        if (ASM.state == "rifleReload"){
            overcharge += overchargePerSec * Time.deltaTime;
            overcharge -= deplete * Time.deltaTime;
        }
        heatBar.value = overcharge / totalOvercharge;
        overheatBar.value = overheated / overheatTime;
        
        if (Input.GetButton("Fire1") && ASM.state == "blocking" && gunAmmo >= 1 && !gunFiring){
            StartCoroutine(gunFire());
        }
        
        if (Input.GetButton("Fire1") && ASM.state == "rifleReady" && overcharge < totalOvercharge && !rifleFiring){
            if (++overcharge > totalOvercharge){
                overcharge = totalOvercharge;
                overheated = overheatTime;
            }
            StartCoroutine(rifleFire());
        }
        
        if (Input.GetKeyDown("r") && ASM.state == "rifleReady" && overcharge > overchargePerSec * 3 && !rifleFiring){
            deplete = overcharge / 3f;
            StartCoroutine(rifleReload());
        }
        
    }

    float RandomAcc() {
        return Random.Range(-crosshair.crosshaiSize, crosshair.crosshaiSize) / 30f;
    }
    

    IEnumerator rifleReload() {
        ASM.state = "rifleReload";
        ASM.rifle.GetComponent<Animator>().Play("RifleReload");
        yield return new WaitForSeconds(2.1f);
        rifleReloadAudio.Play();
        yield return new WaitForSeconds(0.2f);
        rifleReloadAudio.Play();
        yield return new WaitForSeconds(0.7f);
        if (ASM.state == "rifleReload") {
            ASM.state = "rifleReady";
            ASM.rifle.GetComponent<Animator>().Play("RifleReady");
        }
        
    }

    IEnumerator gunFire() {
        gunFiring = true;
        ASM.gun.GetComponent<Animator>().Play("GunFire");
        yield return new WaitForSeconds(0.1f);
        GameObject bullet = Instantiate(projectile, transform.position + Pivot.up * 0f + Pivot.front * 0.5f, transform.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = Pivot.front * speed + Pivot.up * RandomAcc() + Pivot.right * RandomAcc();
        gunShotAudio.Play();
        gunAmmo--;
        yield return new WaitForSeconds(0.4f);
        gunFiring = false;
        if (ASM.state == "blocking") ASM.gun.GetComponent<Animator>().Play("GunReady");
    }

    IEnumerator rifleFire() {
        rifleFiring = true;
        ASM.rifle.GetComponent<Animator>().Play("RifleFire");
        yield return new WaitForSeconds(0.1f);
        rifleShotAudio.Play();
        var projectilePoint = Instantiate(bigProjectile, transform.position + Pivot.up * 0f + Pivot.front * 2.1f + Pivot.right * 0.2f, transform.rotation);
        if(Aim.aimedEnemy) projectilePoint.GetComponent<ParticleProjectile>().target = Aim.aimedEnemy;
        yield return new WaitForSeconds(0.025f);
        projectilePoint = Instantiate(bigProjectile, transform.position + Pivot.up * 0f + Pivot.front * 2.1f + Pivot.right * 0.2f, transform.rotation);
        if(Aim.aimedEnemy) projectilePoint.GetComponent<ParticleProjectile>().target = Aim.aimedEnemy;
        yield return new WaitForSeconds(0.025f);
        projectilePoint = Instantiate(bigProjectile, transform.position + Pivot.up * 0f + Pivot.front * 2.1f + Pivot.right * 0.2f, transform.rotation);
        if(Aim.aimedEnemy) projectilePoint.GetComponent<ParticleProjectile>().target = Aim.aimedEnemy;
        yield return new WaitForSeconds(0.025f);
        projectilePoint = Instantiate(bigProjectile, transform.position + Pivot.up * 0f + Pivot.front * 2.1f + Pivot.right * 0.2f, transform.rotation);
        if(Aim.aimedEnemy) projectilePoint.GetComponent<ParticleProjectile>().target = Aim.aimedEnemy;
        yield return new WaitForSeconds(0.025f);
        projectilePoint = Instantiate(bigProjectile, transform.position + Pivot.up * 0f + Pivot.front * 2.1f + Pivot.right * 0.2f, transform.rotation);
        if(Aim.aimedEnemy) projectilePoint.GetComponent<ParticleProjectile>().target = Aim.aimedEnemy;
        yield return new WaitForSeconds(0.5f);
        rifleFiring = false;
        ASM.rifle.GetComponent<Animator>().Play("RifleReady");
    }
}
