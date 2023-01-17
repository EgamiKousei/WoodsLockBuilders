using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

namespace ScrollUI
{
    public class FollowSelectedNode : MonoBehaviour
    {
        /// <summary>
        /// ScrollRect�R���|�[�l���g
        /// </summary>
        [SerializeField]
        private ScrollRect _scrollRect;

        /// <summary>
        /// �X�N���[���G���A��RectTransform
        /// </summary>
        [SerializeField]
        private RectTransform _viewportRectransform;

        /// <summary>
        /// Node���i�[����Transform
        /// </summary>
        [SerializeField]
        private Transform _contentTransform;

        /// <summary>
        /// Node��RectTransform
        /// </summary>
        [SerializeField]
        private RectTransform _nodePrefab;

        /// <summary>
        /// VerticalLayoutGroup(Spacing�擾�p)
        /// </summary>
        [SerializeField]
        private VerticalLayoutGroup _verticalLayoutGroup;

        void Start()
        {
            //�I�𒆂̃I�u�W�F�N�g���ω�������I�𒆂�Node��Index���擾���ăX�N���[��������
            EventSystem.current
                .ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Select(x => x != null ? x.GetComponent<Node>() : null)
                .Where(x => x != null)
                .Subscribe(x => Scroll(x.NodeNumber))
                .AddTo(this);
        }

        /// <summary>
        /// �����X�N���[��
        /// </summary>
        void Scroll(int nodeIndex)
        {
            ItemCreate.itemNum = nodeIndex;
            ItemCreate.flug = true;

            //�v�f�Ԃ̊Ԋu
            var spacing = _verticalLayoutGroup.spacing;
            //���݂̃X�N���[���͈͂̐��l���v�Z���₷���l�ɏ㉺���]
            var p = 1.0f - _scrollRect.verticalNormalizedPosition;
            //���݂̗v�f��
            var nodeCount = _contentTransform.childCount;
            //�`��͈͂̃T�C�Y
            var viewportSize = _viewportRectransform.sizeDelta.y;
            //�`��͈͂̃T�C�Y�̔���
            var harlViewport = viewportSize * 0.5f;

            //�P�v�f�̃T�C�Y
            var nodeSize = _nodePrefab.sizeDelta.y + spacing;

            //���݂̕`��͈͂̒��S���W
            var centerPosition = (nodeSize * nodeCount - viewportSize) * p + harlViewport;
            //���݂̕`��͈͂̏�[���W
            var topPosition = centerPosition - harlViewport;
            //���݂̌��ݕ`��̉��[���W
            var bottomPosition = centerPosition + harlViewport;

            // ���ݑI�𒆂̗v�f�̒��S���W
            var nodeCenterPosition = nodeSize * nodeIndex + nodeSize / 2.0f;

            //�I�������v�f���㑤�ɂ͂ݏo�Ă���
            if (topPosition > nodeCenterPosition)
            {
                //�I��v�f���`��͈͂Ɏ��܂�悤�ɃX�N���[��
                var newP = (nodeSize * nodeIndex) / (nodeSize * nodeCount - viewportSize);
                _scrollRect.verticalNormalizedPosition = 1.0f - newP; //���]���Ă����̂Ŗ߂�
                return;
            }

            //�I�������v�f�������ɂ͂ݏo�Ă���
            if (nodeCenterPosition > bottomPosition)
            {
                //�I��v�f���`��͈͂Ɏ��܂�悤�ɃX�N���[��
                var newP = (nodeSize * (nodeIndex + 1) + spacing - viewportSize) / (nodeSize * nodeCount - viewportSize);
                _scrollRect.verticalNormalizedPosition = 1.0f - newP; //���]���Ă����̂Ŗ߂�
            }
        }
    }
}