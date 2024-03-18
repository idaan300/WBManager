using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RobotManager.Utility; 

public class Compare {

    public class AlphaNumCompare : IComparer<string> {
        public int Compare(string AString1, string AString2) {
            if (AString1 == null || AString2 == null) return 0;

            if (AString1 == AString2) return 0;

            int ALen1 = AString1.Length;
            int ALen2 = AString2.Length;
            int AMarker1 = 0;
            int AMarker2 = 0;

            // Walk through two strings with two markers.
            while (AMarker1 < ALen1 && AMarker2 < ALen2) {
                char ch1 = AString1[AMarker1];
                char ch2 = AString2[AMarker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[ALen1];
                int loc1 = 0;
                char[] space2 = new char[ALen2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do {
                    space1[loc1++] = ch1;
                    AMarker1++;

                    if (AMarker1 < ALen1) {
                        ch1 = AString1[AMarker1];
                    } else {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do {
                    space2[loc2++] = ch2;
                    AMarker2++;

                    if (AMarker2 < ALen2) {
                        ch2 = AString2[AMarker2];
                    } else {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new(space1);
                string str2 = new(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0])) {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                } else {
                    result = string.Compare(str1, str2, StringComparison.Ordinal);
                }

                if (result != 0) {
                    return result;
                }
            }
            return ALen1 - ALen2;
        }
    }

    public class AlphaNumCompareTab(int TabIndex) : IComparer<string> {

        public int Group { get; set; } = TabIndex;

        public int Compare(string String1, string String2) {
            if (String1 == null || String2 == null) return 0;
            if (String1 == String2) return 0;


            string[] Reg1 = Regex.Split(String1, @"\t+");
            string[] Reg2 = Regex.Split(String2, @"\t+");


            try {

                if (Reg1.Length < Group) return 0;
                String1 = Reg1[Group];

                if (Reg1.Length < Group) return 0;
                String2 = Reg2[Group];

            } catch { return 0; }

            int Length1 = String1.Length;
            int Length2 = String2.Length;
            int Marker1 = 0;
            int Marker2 = 0;

            // Walk through the strings with two markers.
            while (Marker1 < Length1 && Marker2 < Length2) {
                char Char1 = String1[Marker1];
                char Char2 = String2[Marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] Space1 = new char[Length1];
                int Location1 = 0;
                char[] Space2 = new char[Length2];
                int Location2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do {
                    Space1[Location1++] = Char1;
                    Marker1++;

                    if (Marker1 < Length1) Char1 = String1[Marker1];
                    else break;
                } 
                while (char.IsDigit(Char1) == char.IsDigit(Space1[0]));

                do {
                    Space2[Location2++] = Char2;
                    Marker2++;

                    if (Marker2 < Length2) Char2 = String2[Marker2];
                    else break;
                    
                } while (char.IsDigit(Char2) == char.IsDigit(Space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string CharStr1 = new(Space1);
                string CharStr2 = new(Space2);

                int Result;

                if (char.IsDigit(Space1[0]) && char.IsDigit(Space2[0])) {
                    int NumChunk1 = int.Parse(CharStr1);
                    int NumChunk2 = int.Parse(CharStr2);
                    Result = NumChunk1.CompareTo(NumChunk2);
                } else Result = string.Compare(CharStr1, CharStr2, StringComparison.Ordinal);
                

                if (Result != 0) return Result;
                
            }
            return Length1 - Length2;
        }
    }

    public class NaturalCompare : IComparer<string> {

        public int Compare(string strA, string strB) {
		    return Compare(strA, strB, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
	    }

	    private static int Compare(string strA, string strB, CultureInfo culture, CompareOptions options) {
		    CompareInfo cmp = culture.CompareInfo;
		    int iA = 0;
		    int iB = 0;
		    int softResult = 0;
		    int softResultWeight = 0;
		    while (iA < strA.Length && iB < strB.Length) {
			    bool isDigitA = char.IsDigit(strA[iA]);
			    bool isDigitB = char.IsDigit(strB[iB]);
			    if (isDigitA != isDigitB) {
				    return cmp.Compare(strA, iA, strB, iB, options);
			    }

			    if (!isDigitA && !isDigitB) {
				    int jA = iA + 1;
				    int jB = iB + 1;
				    while (jA < strA.Length && !char.IsDigit(strA[jA])) jA++;
				    while (jB < strB.Length && !char.IsDigit(strB[jB])) jB++;
				    int cmpResult = cmp.Compare(strA, iA, jA - iA, strB, iB, jB - iB, options);
				    if (cmpResult != 0) {
					    // Certain strings may be considered different due to "soft" differences that are
					    // ignored if more significant differences follow, e.g. a hyphen only affects the
					    // comparison if no other differences follow
					    string sectionA = strA.Substring(iA, jA - iA);
					    string sectionB = strB.Substring(iB, jB - iB);
					    if (cmp.Compare(sectionA + "1", sectionB + "2", options) ==
					        cmp.Compare(sectionA + "2", sectionB + "1", options)) {
						    return cmp.Compare(strA, iA, strB, iB, options);
					    }

					    if (softResultWeight < 1) {
						    softResult = cmpResult;
						    softResultWeight = 1;
					    }
				    }

				    iA = jA;
				    iB = jB;
			    }
			    else {
				    char zeroA = (char)(strA[iA] - (int)char.GetNumericValue(strA[iA]));
				    char zeroB = (char)(strB[iB] - (int)char.GetNumericValue(strB[iB]));
				    int jA = iA;
				    int jB = iB;
				    while (jA < strA.Length && strA[jA] == zeroA) jA++;
				    while (jB < strB.Length && strB[jB] == zeroB) jB++;

				    int resultIfSameLength = 0;
				    do {
					    isDigitA = jA < strA.Length && char.IsDigit(strA[jA]);
					    isDigitB = jB < strB.Length && char.IsDigit(strB[jB]);
					    int numA = isDigitA ? (int)char.GetNumericValue(strA[jA]) : 0;
					    int numB = isDigitB ? (int)char.GetNumericValue(strB[jB]) : 0;
					    if (isDigitA && (char)(strA[jA] - numA) != zeroA) isDigitA = false;
					    if (isDigitB && (char)(strB[jB] - numB) != zeroB) isDigitB = false;
					    if (!isDigitA || !isDigitB) continue;
					    if (numA != numB && resultIfSameLength == 0) {
						    resultIfSameLength = numA < numB ? -1 : 1;
					    }

					    jA++;
					    jB++;
				    } while (isDigitA && isDigitB);

				    if (isDigitA != isDigitB) {
					    // One number has more digits than the other (ignoring leading zeros) - the longer
					    // number must be larger
					    return isDigitA ? 1 : -1;
				    }

				    if (resultIfSameLength != 0) {
					    // Both numbers are the same length (ignoring leading zeros) and at least one of
					    // the digits differed - the first difference determines the result
					    return resultIfSameLength;
				    }

				    int lA = jA - iA;
				    int lB = jB - iB;
				    if (lA != lB) {
					    // Both numbers are equivalent but one has more leading zeros
					    return lA > lB ? -1 : 1;
				    }

				    if (zeroA != zeroB && softResultWeight < 2) {
					    softResult = cmp.Compare(strA, iA, 1, strB, iB, 1, options);
					    softResultWeight = 2;
				    }

				    iA = jA;
				    iB = jB;
			    }
		    }

		    return iA < strA.Length || iB < strB.Length ? iA < strA.Length ? 1 : -1 : softResult != 0 ? softResult : 0;
	    }
    }
}