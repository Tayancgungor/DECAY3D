using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeThrower : MonoBehaviour
{
    public GameDirector gameDirector;
    public Grenade grenadePrefab;
    public LayerMask grenadeClickLayerMask;
    public float grenadeThrowForce;
    public PlayerMovement playerMovement;
    public Transform grenadeSpawnPoint;
    public float grenadeCooldown = 1f;
    float _lastGrenadeThrowTime;
    public GrenadeUI grenadeUI;
    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time - _lastGrenadeThrowTime >= grenadeCooldown)
        {
            StartCoroutine(ThrowGrenade());
        }
        if (gameDirector.gameState ==GameState.Gameplay)
        {
            grenadeUI.SetFillBar((Time.time - _lastGrenadeThrowTime) / grenadeCooldown);
        }
    }

    IEnumerator ThrowGrenade()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        playerMovement.ChangeAnimationState("ThrowGrenade");
        _lastGrenadeThrowTime = Time.time;


        playerMovement.SetUpperBodyLayerWeightTo1();
        playerMovement._isthrowingGrenade = true;

        if (Physics.Raycast(ray, out var hit, 50 , grenadeClickLayerMask))
        {
            var distance = (hit.point - transform.position).magnitude;
            yield return new WaitForSeconds(0.3f);

            var newGrenade = Instantiate(grenadePrefab);
            newGrenade.StartGrenade(gameDirector);

            newGrenade.transform.DOLocalRotate(newGrenade.transform.localEulerAngles + new Vector3(360, 360 , 0), distance * grenadeThrowForce, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
            newGrenade.transform.position = grenadeSpawnPoint.position;

            newGrenade.transform.DOMoveX(hit.point.x, distance * grenadeThrowForce).SetEase(Ease.Linear);
            newGrenade.transform.DOMoveZ(hit.point.z, distance * grenadeThrowForce).SetEase(Ease.Linear);
            newGrenade.transform.DOMoveY(transform.position.y + 1.5f, distance * grenadeThrowForce * .5f).SetEase(Ease.OutQuad);

            newGrenade.transform.DOMoveY(hit.point.y, distance * grenadeThrowForce * .5f).SetEase(Ease.InQuad).SetDelay(distance * grenadeThrowForce * .5f).SetEase(Ease.InQuad);

            playerMovement._isthrowingGrenade = false;


            yield return new WaitForSeconds(0.3f);


            playerMovement.SetUpperBodyLayerWeightTo0();
        }
    }
}
