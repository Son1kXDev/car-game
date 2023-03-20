using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Game
{
    public class CloudsMovement : MonoBehaviour
    {
        private void Update()
        {
            transform.position -= Vector3.left;
        }
    }
}