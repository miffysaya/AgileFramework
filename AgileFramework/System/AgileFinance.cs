using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileFramework
{
    public static class AgileFinance
    {
        /// <summary>
        /// 年利率转化为月利率
        /// </summary>
        /// <param name="yearRate">年利率</param>
        /// <returns>月利率</returns>
        public static decimal ConvertRateFromYearToMonth(decimal yearRate)
        {
            return yearRate / 12M;
        }

        /// <summary>
        /// 等额本息还款法总体情况
        /// </summary>
        /// <param name="monthRate">月利率</param>
        /// <param name="principal">本金</param>
        /// <param name="month">还款月数</param>
        /// <returns>每月还款情况</returns>
        public static List<AgileFinanceItem> OverallOfAverageCapitalMethod(decimal monthRate, decimal principal, int month)
        {
            var result = new List<AgileFinanceItem>();

            var principalAll = 0M;

            var monthlyCapital = MonthlyCapitalOfAverageCapitalMethod(monthRate, principal, month);

            for (var i = 0; i < month; i++)
            {
                var monthlyPrincipal = MonthlyPrincipalOfAverageCapitalMethod(monthRate, principal, month, i);

                var interest = monthlyCapital - monthlyPrincipal;

                var item = new AgileFinanceItem()
                {
                    Interest = interest,
                    Month = i,
                    Principal = monthlyPrincipal,
                    Capital = monthlyCapital
                };
                result.Add(item);

                if (i == month - 1)
                {
                    //最后一个月做平衡
                    item.Principal = principal - principalAll;
                    item.Interest = item.Capital - item.Principal;
                }

                principalAll += monthlyPrincipal;

                item.PrincipalBalance = principal - result.Sum(one => one.Principal);
            }

            return result;
        }

        /// <summary>
        /// 等额本息还款法每月还款金额
        /// </summary>
        /// <param name="monthRate">月利率</param>
        /// <param name="principal">本金</param>
        /// <param name="month">还款月数</param>
        /// <returns>每月还款金额</returns>
        private static decimal MonthlyCapitalOfAverageCapitalMethod(decimal monthRate, decimal principal, int month)
        {
            var monthRateDouble = Convert.ToDouble(monthRate);

            var principalDouble = Convert.ToDouble(principal);

            var monthDouble = Convert.ToDouble(month);

            var result = principalDouble * (monthRateDouble * Math.Pow((1d + monthRateDouble), monthDouble)) / (Math.Pow((1 + monthRateDouble), monthDouble) - 1d);

            return Convert.ToDecimal(result);
        }

        /// <summary>
        /// 等额本息还款法每月还款中本金部分
        /// </summary>
        /// <param name="monthRate">月利率</param>
        /// <param name="principal">本金</param>
        /// <param name="month">还款月份</param>
        /// <param name="currentMonth">当前</param>
        /// <returns>每月还款中本金部分</returns>
        private static decimal MonthlyPrincipalOfAverageCapitalMethod(decimal monthRate, decimal principal, int month, int currentMonth)
        {
            var monthRateDouble = Convert.ToDouble(monthRate);

            var principalDouble = Convert.ToDouble(principal);

            var monthDouble = Convert.ToDouble(month);

            var currentMonthDouble = Convert.ToDouble(currentMonth);

            var result = principalDouble * monthRateDouble * Math.Pow(1d + monthRateDouble, currentMonthDouble - 1d) / (Math.Pow(1d + monthRateDouble, monthDouble) - 1d);

            return Convert.ToDecimal(result);
        }
    }

    /// <summary>
    /// 金融道具类
    /// </summary>
    public class AgileFinanceItem
    {
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Capital { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        /// 利息
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// 本金余额
        /// </summary>
        public decimal PrincipalBalance { get; set; }
    }
}
