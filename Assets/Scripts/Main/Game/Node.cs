using UnityEngine;

namespace ScrollUI
{
    class Node : MonoBehaviour
    {
        /// <summary>
        /// Node‚ÌIndex”Ô†
        /// </summary>
        public int NodeNumber { get { return transform.GetSiblingIndex(); } }
    }
}