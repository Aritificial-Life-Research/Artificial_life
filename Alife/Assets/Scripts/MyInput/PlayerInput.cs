using Common;
using UnityEngine;

namespace MyInput
{
    public class PlayerInput : BaseBehaviour, IInput
    {

        Vector2 currentMove;
        Vector2 _lookAt;

        // Update is called once per frame
        void Update()
        {
            currentMove = Vector3.zero;

            currentMove.x = UnityEngine.Input.GetAxis("Horizontal");
            currentMove.y = UnityEngine.Input.GetAxis("Vertical");

            var imousePosition = UnityEngine.Input.mousePosition;
            imousePosition.z = Camera.main.transform.position.z;
            var mousePosition = Camera.main.ScreenToWorldPoint(imousePosition);
            //Debug.Log(Input.mousePosition);
            //Debug.Log(mousePosition);

            _lookAt = mousePosition;


            if (UnityEngine.Input.GetButtonDown("Fire"))
            {
                CentralBus.WeaponFire.Invoke();
            }

            if (UnityEngine.Input.GetButtonDown("Reload Level"))
            {
                InnerBus.Global.ManualReloadLevel.Invoke();
            }
        }


        public Vector2 MoveVector
        {
            get
            {
                return currentMove;
            }
        }

        public Vector2 lookAt
        {
            get
            {
                return _lookAt;
            }
        }
    }

}