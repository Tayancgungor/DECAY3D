using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Weapon _weapon;
    public float speed;
    public float destroyDistance;

    public void StartBullet(Weapon w)
    {
        _weapon = w;
    }


    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        var distance = (transform.position - _weapon.transform.position).magnitude;
        if (distance>destroyDistance)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _weapon.gameDirector.fxManager.PlayImpactPS(transform.position, transform.forward);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            {
                var damage = 1;
                _weapon.gameDirector.fxManager.PlayZombieImpactPS(transform.position, transform.forward);
                var angle = Vector3.Angle(transform.forward, other.transform.forward);
                if (angle < 90)
                {
                    damage *= 2;
                }
                other.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
