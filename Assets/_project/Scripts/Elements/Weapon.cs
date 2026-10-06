using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public GameDirector gameDirector;

    public WeaponType weaponType;
    public Bullet bulletPrefab;
    public Transform shootPosition;
    public ParticleSystem muzzlePS;
    public Light muzzleLight;

    public float attackRate;

    private float _timeSinceLastShoot;

    private void Update()
    {
        _timeSinceLastShoot += Time.deltaTime;

        if (gameDirector.gameState == GameState.Gameplay && Mouse.current.leftButton.isPressed && _timeSinceLastShoot > attackRate)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPosition.position;
        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward);
        newBullet.StartBullet(this);
        _timeSinceLastShoot = 0;
        muzzlePS.Play();
        muzzleLight.DOIntensity(15, .05f).SetLoops(2, LoopType.Yoyo);
        gameDirector.audioManager.PlayShootAS();
    }
}


public enum WeaponType
{
    Pistol,
    MachineGun,
    Shotgun,
    SniperRifle,

}