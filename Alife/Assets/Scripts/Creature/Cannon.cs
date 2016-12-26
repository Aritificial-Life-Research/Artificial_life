using UnityEngine;

namespace LD32
{
    public class Cannon : BaseBehaviour
    {
        public float distanceToSpawnBullet;
        public float bulletForce;
        public float kickbackForce;
        public float screenshakeAmount;
        public BulletMode mode = BulletMode.Damage;
        public int bulletDamage;
        public AudioClip fireSound;

        public override void Awake()
        {
            base.Awake();

            CentralBus.WeaponFire.AddListener(WeaponFire);
        }

        public void OnDestroy()
        {
            CentralBus.WeaponFire.RemoveListener(WeaponFire);
        }

        public void WeaponFire()
        {
            BulletController.Instantiate(
                transform.TransformPoint(new Vector2(1, 0)),
                transform.rotation,
                bulletForce,
                mode,
                bulletDamage);

            if (fireSound != null)
            {
                AudioSource.PlayClipAtPoint(fireSound, transform.position);
            }

            var kickback = transform.right * -1 * kickbackForce;
            CentralBus.ImpulseRequested.Invoke(kickback);

            CentralBus.Global.ScreenShakeRequested.Invoke(screenshakeAmount);
        }
    }

}