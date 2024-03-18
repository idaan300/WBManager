using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RobotManager.Utility; 

public static class StringEx {

	/// <summary>
	///Check if given string is null empty whitespace or /0, if so return "Unknown" else return given string
	/// </summary>
	public static string HandleStringEmpty(this string Str) => string.IsNullOrEmpty(Str) || string.IsNullOrWhiteSpace(Str) || Str.All(X => X.Equals('\0')) ? "Unknown" : Str;


	/// <summary>
	///Check if given string is null empty whitespace or /0, if so return "Unknown" else return given string
	/// </summary>
	public static bool IsStringEmpty(this string Str) => string.IsNullOrEmpty(Str) || string.IsNullOrWhiteSpace(Str) || Str.All(X => X.Equals('\0'));

    /// <summary>
    /// Removes Any Type Of Seperator From A given string
    /// (' ', '_' and '-')
    /// </summary>
    /// <param name="In"></param>
    /// <returns></returns>
	public static string TrimSeperators(string In) => In.Where(c => !char.IsSeparator(c)).Select(c => c.ToString()).Aggregate((a, b) => a + b);

	/// <summary>
	/// Calculate the difference between 2 strings using the Levenshtein distance algorithm
	/// </summary>
	/// <param name="String1">First string</param>
	/// <param name="String2">Second string</param>
	/// <returns>The Distance In Characters Between The 2 Strings</returns>
	public static int Distance(string String1, string String2) {
		int Length1 = String1.Length;
		int Length2 = String2.Length;

		int[,] Matrix = new int[Length1 + 1, Length2 + 1];

		// First calculation, if one entry is empty return full length
		if (Length1 == 0) return Length2;
		if (Length2 == 0) return Length1;

		// Initialization of matrix with row size Length1 and columns size Length2
		for (int i = 0; i <= Length1; Matrix[i, 0] = i++) { }
		for (int j = 0; j <= Length2; Matrix[0, j] = j++) { }

		// Calculate rows and collumns distances
		for (int i = 1; i <= Length1; i++) {
			for (int j = 1; j <= Length2; j++) {
				int cost = String2[j - 1] == String1[i - 1] ? 0 : 1;

				Matrix[i, j] = Math.Min(
					Math.Min(Matrix[i - 1, j] + 1, Matrix[i, j - 1] + 1), 
					Matrix[i - 1, j - 1] + cost);
			}
		}
		// return result
		return Matrix[Length1, Length2];
	}

    public static string GeneratePassword(int Len, bool Uppercase = false) {
	    Random Rand = new();
	    StringBuilder Build = new();
	    bool Vowel = Rand.Next(2) == 0;

        string[] Vowels = "A,A1,AU,A0,E,3,EA,3A,33,EE,1,1A,10,0,0A,0I,00,0U,U,2".Split(',');
	    string[] Consonants = "8,C,CH,CL,D,F,G,GH,GL,J,K,M,MN,N,P,PH,PS,R,RH,S,SC,SH,SK,ST,T,TH,V,W,X,Y,Z,4,5,6,7,9".Split(',');

        for (int i = 0; i < Len; i++) {
            string[] Str = Vowel ? Vowels : Consonants;
            string Chars = Str[Rand.Next(Str.Length)];
            i += Chars.Length - 1;
            Build.Append(Chars);
            Vowel =! Vowel; 
        }

        return Uppercase ? Build.ToString().Substring(0,Len) : Build.ToString().Substring(0,Len).ToLowerInvariant();
    }

    /// <summary>
    /// Check wether a given string only contains digits (0-9)
    /// Returns false on the first occurence of a non digit
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsDigitsOnly(string str) => str.All(c => c is >= '0' and <= '9');

    /// <summary>
    /// Remove all whitespace Characters from a string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string RemoveWhitespace(this string input) => new (input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
    


}