/* ------------------------------------------------------------------------
* 【本类功能概述】
* 
* 创建人：俊涵(JunHan)
* 创建时间：2014/6/11 14:25:41
* 创建说明：
*
* 修改人： 
* 修改时间： 
* 修改说明：
* ------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    public class StringHelper
    {
        public static string GetUrl()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
