using UnityEngine;

namespace MyInput
{
    //can fire Cannon by raising fireEvent
    public interface IInput
    {
        Vector2 MoveVector
        {
            get;
        }

        Vector2 lookAt
        {
            get;
        }

    }
}