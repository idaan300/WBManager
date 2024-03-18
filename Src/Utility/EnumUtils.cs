using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotManager.Utility; 

public static class EnumUtils {

	public static IDictionary<string, T> ConvertEnumToDictionary<T>(this Enum e) where T : notnull => 
		Enum.GetValues(e.GetType()).Cast<T>().ToDictionary(currentItem => Enum.GetName(e.GetType(), currentItem));
    
	public static IEnumerable<Enum> GetEnumValues<T>(this Enum e) where T : Enum => Enum.GetValues(e.GetType()).Cast<Enum>();
    
}