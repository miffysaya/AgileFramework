using System;
using System.Collections.Generic;

namespace AgileFramework
{
    /// <summary>
    /// 数组
    /// </summary>
    public static class AgileArray
    {
        /// <summary>
        /// 获得可能
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="n">几个为true</param>
        /// <returns>结果</returns>
        public static List<Dictionary<int, bool>> GetPossibly(int length, int n)
        {
            var result = new List<Dictionary<int, bool>>();

            var maxFormat = string.Empty;
            var minFormat = string.Empty;

            for (int i = 0; i < length; i++)
            {
                maxFormat = maxFormat + "1";
                minFormat = minFormat + "0";
            }

            var max = Convert.ToInt32(maxFormat, 2);
            var min = Convert.ToInt32(minFormat, 2);

            var count = 0;

            for (var i = min; i < max; i++)
            {
                count = 0;
                var current = Convert.ToInt32(Convert.ToString(i, 2)).ToString(minFormat);
                var array = current.ToCharArray();
                var item = new Dictionary<int, bool>();

                for (int j = 0; j < array.Length; j++)
                {
                    if (array[j] == '1')
                    {
                        count++;
                    }
                    item.Add(j, array[j] == '1');
                }
                if (count == n)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
