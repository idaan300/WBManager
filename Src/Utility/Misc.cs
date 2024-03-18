using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using RobotManager.Utility.Debug;

namespace RobotManager.Utility {

    public static class Misc {
	    
	    
        /// <summary>
        /// Sets An Array To The Given Value / Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Arr"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T[] Populate<T>(this T[] Arr, T Value) {
            for (int i = 0; i < Arr.Length; i++) Arr[i] = Value;
            return Arr;
        }

        /// <summary>
        /// Compare And Check 2 Byte Arrays.
        /// If Either is null: false
        /// If Lengths differ: false
        /// If Any Element Differs: false
        /// else: true
        /// </summary>
        /// <param name="BArr1">Array 1</param>
        /// <param name="BArr2">Array 2</param>
        /// <returns>Arrays Contents Equall</returns>
        public static bool ByteArraysEqual(byte[]BArr1, byte[]BArr2) {
            if (BArr1 == BArr2) return true;
            if (BArr1 == null || BArr2 == null) return false;
            if (BArr1.Length != BArr2.Length) return false;
            return !BArr1.Where((T, i) => T != BArr2[i]).Any();
        }

        /// <summary>
        /// Clamp given value between two other values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T> {
	        if (val.CompareTo(min) < 0) return min;
	        return val.CompareTo(max) > 0 ? max : val;
        }


        /// <summary>
        /// Convert A Given string To The Closest Possible Representation Of One of The Supplied Enum Items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static T GetClosest<T>(this T InputEnum, string Input) where T: Enum {
            //InputEnum is unused but The Syntax For ExtentionMethods Requires It because we need to get an enum's type
            List<(int,int)> Distances = (from T Name in Enum.GetValues(typeof(T)) select (StringEx.Distance(Name.ToString(), Input), Convert.ToInt32(Name))).ToList();
            int Closest = Distances.IndexOf(Distances.Min());
            return (T)(dynamic)Distances[Closest].Item2;    //This is horrible, but it fools the conpiler into accepting
															//something that normally is possible. Casting an int to an enum.
															//It complains here because Type T Has No Native Casts to Int. Even if we explicitly state that T is an Enum,
															//object Type "Enum" has no vallid casting to int like the keyword "enum" has, which is the true type of T.
        }

        public static int WeekOfYear(DateTime date) {
	        int day = (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
	        return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date.AddDays(4 - (day == 0 ? 7 : day)), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static string GetRelativePath(string FileSpec, string FolderSpec) {
            Uri pathUri = new(FileSpec);
            // Folders must end in a slash
            if (!FolderSpec.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                FolderSpec += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new(FolderSpec);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

    }

}