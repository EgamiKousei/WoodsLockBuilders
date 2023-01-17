using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

namespace ScrollUI
{
    public class FollowSelectedNode : MonoBehaviour
    {
        /// <summary>
        /// ScrollRectコンポーネント
        /// </summary>
        [SerializeField]
        private ScrollRect _scrollRect;

        /// <summary>
        /// スクロールエリアのRectTransform
        /// </summary>
        [SerializeField]
        private RectTransform _viewportRectransform;

        /// <summary>
        /// Nodeを格納するTransform
        /// </summary>
        [SerializeField]
        private Transform _contentTransform;

        /// <summary>
        /// NodeのRectTransform
        /// </summary>
        [SerializeField]
        private RectTransform _nodePrefab;

        /// <summary>
        /// VerticalLayoutGroup(Spacing取得用)
        /// </summary>
        [SerializeField]
        private VerticalLayoutGroup _verticalLayoutGroup;

        void Start()
        {
            //選択中のオブジェクトが変化したら選択中のNodeのIndexを取得してスクロールさせる
            EventSystem.current
                .ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Select(x => x != null ? x.GetComponent<Node>() : null)
                .Where(x => x != null)
                .Subscribe(x => Scroll(x.NodeNumber))
                .AddTo(this);
        }

        /// <summary>
        /// 自動スクロール
        /// </summary>
        void Scroll(int nodeIndex)
        {
            ItemCreate.itemNum = nodeIndex;
            ItemCreate.flug = true;

            //要素間の間隔
            var spacing = _verticalLayoutGroup.spacing;
            //現在のスクロール範囲の数値を計算しやすい様に上下反転
            var p = 1.0f - _scrollRect.verticalNormalizedPosition;
            //現在の要素数
            var nodeCount = _contentTransform.childCount;
            //描画範囲のサイズ
            var viewportSize = _viewportRectransform.sizeDelta.y;
            //描画範囲のサイズの半分
            var harlViewport = viewportSize * 0.5f;

            //１要素のサイズ
            var nodeSize = _nodePrefab.sizeDelta.y + spacing;

            //現在の描画範囲の中心座標
            var centerPosition = (nodeSize * nodeCount - viewportSize) * p + harlViewport;
            //現在の描画範囲の上端座標
            var topPosition = centerPosition - harlViewport;
            //現在の現在描画の下端座標
            var bottomPosition = centerPosition + harlViewport;

            // 現在選択中の要素の中心座標
            var nodeCenterPosition = nodeSize * nodeIndex + nodeSize / 2.0f;

            //選択した要素が上側にはみ出ている
            if (topPosition > nodeCenterPosition)
            {
                //選択要素が描画範囲に収まるようにスクロール
                var newP = (nodeSize * nodeIndex) / (nodeSize * nodeCount - viewportSize);
                _scrollRect.verticalNormalizedPosition = 1.0f - newP; //反転していたので戻す
                return;
            }

            //選択した要素が下側にはみ出ている
            if (nodeCenterPosition > bottomPosition)
            {
                //選択要素が描画範囲に収まるようにスクロール
                var newP = (nodeSize * (nodeIndex + 1) + spacing - viewportSize) / (nodeSize * nodeCount - viewportSize);
                _scrollRect.verticalNormalizedPosition = 1.0f - newP; //反転していたので戻す
            }
        }
    }
}