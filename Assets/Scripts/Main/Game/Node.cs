using UnityEngine;

namespace ScrollUI
{
    class Node : MonoBehaviour
    {
        /// <summary>
        /// Node��Index�ԍ�
        /// </summary>
        public int NodeNumber { get { return transform.GetSiblingIndex(); } }
    }
}