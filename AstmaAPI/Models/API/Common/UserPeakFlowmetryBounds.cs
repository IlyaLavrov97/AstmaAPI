using AstmaAPI.Models.DBO;
using System;
using System.Collections.Generic;

namespace AstmaAPI.Models.API.Common
{
    public class UserPeakFlowmetryBounds
    {
        #region Fields

        private static readonly List<int> TableHeightValues = new List<int>
        {
            91, 99, 107, 114, 122, 130, 137, 145, 152, 160, 168, 175, 183, 190
        };

        private static readonly List<int> TableAgeValues = new List<int>
        {
            0, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70
        };

        public const int ChildMaxAge = 15;

        public const int MinHeightValueForChild = 91;
        public const int MaxHeightValueForChild = 175;

        public const int MinHeightValueForMan = 160;
        public const int MaxHeightValueForMan = 190;

        public const int MinHeightValueForWoman = 145;
        public const int MaxHeightValueForWoman = 175;

        #endregion

        #region Properties

        public double TopBound { get; set; }

        public double BottomBound { get; set; }

        public double UserRate { get; set; }

        #endregion

        #region Constructor

        public UserPeakFlowmetryBounds(int userRate)
        {
            UserRate = userRate;

            double delta = 0.13 * userRate;

            TopBound = Math.Round(userRate + delta, 2);
            BottomBound = Math.Round(userRate - delta, 2);
        }

        #endregion

        #region Private

        private static int FindHeight(int realHeight)
        {
            int result = 0;
            int currentDif = int.MaxValue;

            foreach (var height in TableHeightValues)
            {
                if (currentDif > Math.Abs(height - realHeight))
                {
                    currentDif = Math.Abs(height - realHeight);
                    result = height;
                }
            }

            return result;
        }

        private static int FindAge(int realAge)
        {
            int result = 0;
            int currentDif = int.MaxValue;

            foreach (var age in TableAgeValues)
            {
                if (currentDif > Math.Abs(age - realAge))
                {
                    currentDif = Math.Abs(age - realAge);
                    result = age;
                }
            }

            return result;
        }

        #endregion

        #region Public

        public static int GetTableHeightValue(User user)
        {
            if (IsChild(user.BirthDate, out int userAge))
            {
                if (user.Height <= MinHeightValueForChild)
                    return MinHeightValueForChild;

                if (user.Height >= MaxHeightValueForChild)
                    return MaxHeightValueForChild;

                return FindHeight(user.Height);
            }

            switch (user.Sex)
            {
                case SexEnum.Man:
                    if (user.Height <= MinHeightValueForMan)
                        return MinHeightValueForMan;

                    if (user.Height >= MaxHeightValueForMan)
                        return MaxHeightValueForMan;

                    return FindHeight(user.Height);
                case SexEnum.Woman:
                    if (user.Height <= MinHeightValueForWoman)
                        return MinHeightValueForWoman;

                    if (user.Height >= MaxHeightValueForWoman)
                        return MaxHeightValueForWoman;

                    return FindHeight(user.Height);
                default:
                    return -1;
            }
        }

        public static int GetTableAgeValue(User user)
        {
            if (IsChild(user.BirthDate, out int userAge))
                return 0;

            return FindAge(userAge);
        }

        public static bool IsChild(DateTime birthDate, out int userAge)
        {
            userAge = DateTime.UtcNow.Year - birthDate.Year;

            if (DateTime.UtcNow.Month <= birthDate.Month &&
                DateTime.UtcNow.Day < birthDate.Day)
            {
                userAge -= 1;
            }

            return userAge < ChildMaxAge;
        }

        #endregion
    }
}
