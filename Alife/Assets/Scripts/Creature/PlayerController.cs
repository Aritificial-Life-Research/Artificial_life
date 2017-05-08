using Common;
using UnityEngine;

namespace Creature
{
    public class PlayerController : BaseBehaviour
    {


        Vector2 _currentMove;
        Vector2 _lookAt;
       


        // Update is called once per frame
        void Update()
        {

            _currentMove = Vector3.zero;

            _currentMove.x = UnityEngine.Input.GetAxis("Horizontal");
            _currentMove.y = UnityEngine.Input.GetAxis("Vertical");

            var imousePosition = UnityEngine.Input.mousePosition;
            imousePosition.z = Camera.main.transform.position.z;
            var mousePosition = Camera.main.ScreenToWorldPoint(imousePosition);
            //Debug.Log(Input.mousePosition);
            //Debug.Log(mousePosition);

            _lookAt = mousePosition;
            CentralBus.MotorRotate.Invoke(_lookAt);
            CentralBus.MotorMove.Invoke(_currentMove);

            if (UnityEngine.Input.GetButtonDown("Fire"))
            {
                CentralBus.WeaponFire.Invoke();
            }

            if (UnityEngine.Input.GetButtonDown("Reload Level"))
            {
                InnerBus.Global.ManualReloadLevel.Invoke();
            }
        
    }

      
  

        void OnDestroy()
        {
            InnerBus.Global.OnPlayerDestroyed.Invoke();
        }

    }

}