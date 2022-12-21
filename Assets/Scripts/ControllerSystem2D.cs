using UnityEngine;

namespace KID
{
    /// <summary>
    /// 控制系統 2D 橫向卷軸
    /// </summary>
    public class ControllerSystem2D : MonoBehaviour
    {
        #region 資料
        [SerializeField, Header("移動速度"), Range(0, 100)]
        private float speedWalk = 3.5f;
        [SerializeField, Header("跳躍高度"), Range(0, 3000)]
        private float heightJump = 500f;
        [SerializeField, Header("檢查地板資料")]
        private Vector2 checkGroundOffset;
        [SerializeField]
        private Vector2 checkGroundSize = Vector2.one;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "開關走路";
        private string parJump = "開關跳躍";
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.7f, 0.1f, 0.3f, 0.5f);
            Gizmos.DrawCube(
                transform.position +
                transform.TransformDirection(checkGroundOffset),
                checkGroundSize);
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
        }

        private void Update()
        {
            Walk();
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void Walk()
        {
            float h = Input.GetAxis("Horizontal");

            rig.velocity = new Vector2(h * speedWalk, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);
            Flip(h);
        }

        /// <summary>
        /// 翻面
        /// </summary>
        /// <param name="h">水平值</param>
        private void Flip(float h)
        {
            if (Mathf.Abs(h) < 0.1f) return;
            float angleY = h > 0 ? 0 : 180;
            transform.eulerAngles = new Vector3(0, angleY, 0);
        }
    }
}
