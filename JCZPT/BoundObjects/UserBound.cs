using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCZPT.BoundObjects
{
    public class UserBound : IDisposable
    {
        public Action OnLogOnSuccess; //html登录成功触发的事件
        public Action OnExit;//html退出系统触发的事件
        public Action OnLogOut; //html退出登录触发的时间
        public UserBound()
        {

        }
        #region 公共方法

        #region 登录成功接口
        public void LogOnSuccess()
        {
            if (this.OnLogOnSuccess != null)
            {
                this.OnLogOnSuccess();
            }
        }
        #endregion
        #region 关闭或退出
        public void Close()
        {
            if (this.OnExit != null)
            {
                this.OnExit();
            }
        }
        #endregion
        #region 关闭或退出
        public void LogOut()
        {
            if (this.OnLogOut != null)
            {
                this.OnLogOut();
            }
        }
        #endregion
        #region Dispose
        public void Dispose()
        {

        }
        #endregion
        #endregion
    }
}
