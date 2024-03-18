using System;
using System.Reflection;

namespace RobotManager.Utility;

public static class EventEx {

    /// <summary>
    /// Get Event Fields
    /// </summary>
    /// <param name="T">Event</param>
    /// <param name="N">Event Name</param>
    /// <returns></returns>
	private static FieldInfo GetEventField(this Type T, string N) {
		FieldInfo Field = null;
		while (T != null) {
			//Find events defined as field
			Field = T.GetField(N, BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
			if (Field != null && (Field.FieldType == typeof(MulticastDelegate) || Field.FieldType.IsSubclassOf(typeof(MulticastDelegate))))
				break;

			//Find events defined as property { add; remove; }
			Field = T.GetField("EVENT_" + N.ToUpper(), BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
			if (Field != null) 
				break;
			T = T.BaseType;
		}
		return Field;
	}

    /// <summary>
    /// Clear All Registered Events From An EventHandler
    /// </summary>
    /// <param name="Event"></param>
    /// <param name="EventName"></param>
	public static void ClearEvents(this object Event, string EventName) {
		FieldInfo FInfo = Event.GetType().GetEventField(EventName);
		if (FInfo == null) return;
		FInfo.SetValue(Event, null);
	}
}

