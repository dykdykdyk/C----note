using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace Win7FTP
{
    ///<summary>
    ///主要用于文件名的比较。
    ///</summary>
    public class FilesNameComparerClass 
    {
       // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
       ///<summary>
       ///比较两个字符串，如果含用数字，则数字按数字的大小来比较。
       ///</summary>
       ///<param name="x"></param>
       ///<param name="y"></param>
       ///<returns></returns>
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);

        #region IComparer 成员

        //public int Compare(object x, object y)
        //{
        //    return StrCmpLogicalW(x, y);
        //}

        #endregion
    }

}
