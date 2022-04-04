using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Inspector
{
    public static class TypeUtils
	{
		public static List<T> GetAllPublicConstantValues<T>(this Type type)
		{
			return type
				.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
				.Select(x => (T)x.GetRawConstantValue())
				.ToList();
		}
	}

	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class ButtonAttribute  : Attribute
	{
		public string Name = null;

		public ButtonAttribute()
        {
        }

		public ButtonAttribute(string name)
        {
            this.Name = name;
        }
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	public class ValueListAttribute : PropertyAttribute
	{
		private ListType listType;
		private enum ListType
		{
			FromClass,
			FromMethod
		}

		private Type classType;
		private string methodName;
		public string GetterSetterName;

		public ValueListAttribute(Type classType, string getset = "")
		{
			this.classType = classType;
			GetterSetterName = getset;
			listType = ListType.FromClass;
		}

		public ValueListAttribute(string methodName, string getset = "")
		{
			this.methodName = methodName;
			GetterSetterName = getset;
			listType = ListType.FromMethod;			
		}

		public virtual List<string> GetValuesList(UnityEngine.Object targetObject)
		{
			switch(listType)
			{
				case ListType.FromClass:
					return classType.GetAllPublicConstantValues<string>();

				case ListType.FromMethod:
					Type type = targetObject.GetType();
					IEnumerable<string> tValues = null;
					MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
					if (method != null)
					{
						tValues = method.Invoke(targetObject, null) as IEnumerable<string>;
					}
					else
					{
						PropertyInfo property = type.GetProperty(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
						if (property != null && property.CanRead)
						{
							tValues = property.GetValue(targetObject, null) as IEnumerable<string>;
						}
					}

					return tValues != null ? new List<string>(tValues) : null;
			}
			return null;
		}
	}

	public sealed class GetSetAttribute : PropertyAttribute
	{
		public readonly string name;
		public bool dirty;
 
		public GetSetAttribute(string name) {
			this.name = name;
		}
	}

	#if UNITY_EDITOR
	[UnityEditor.CustomPropertyDrawer(typeof(GetSetAttribute))]
	sealed class GetSetDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
		{
			GetSetAttribute attribute = (GetSetAttribute)base.attribute;

			UnityEditor.EditorGUI.BeginChangeCheck();
			UnityEditor.EditorGUI.PropertyField(position, property, label);

			if (UnityEditor.EditorGUI.EndChangeCheck())
			{
				attribute.dirty = true;
			} else if (attribute.dirty)
			{
				object parent = GetParentObject(property.propertyPath, property.serializedObject.targetObject);
 
				Type type = parent.GetType();
				PropertyInfo info = type.GetProperty(attribute.name);
 
				if (info == null)
					Debug.LogError("Invalid property name \"" + attribute.name + "\"");
				else
					info.SetValue(parent, fieldInfo.GetValue(parent), null);
 
				attribute.dirty = false;
			}
		}
 
		public object GetParentObject(string path, object obj)
		{
			string[] fields = path.Split('.');
 
			if (fields.Length == 1)
				return obj;
 
			FieldInfo info = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			obj = info.GetValue(obj);
 
			return GetParentObject(string.Join(".", fields, 1, fields.Length - 1), obj);
		}
	}



	[UnityEditor.CustomPropertyDrawer(typeof(Enum), true)]
	public sealed class EnumPropertyDrawer : UnityEditor.PropertyDrawer
	{
		public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
		{
			using (new UnityEditor.EditorGUI.PropertyScope(position, label, property))
			{
				if (HasEnumFlagsAttribute())
				{
					var intValue = UnityEditor.EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);

					if (property.intValue != intValue)
					{
						property.intValue = intValue;
					}
				}
				else
				{
					UnityEditor.EditorGUI.PropertyField(position, property, label);
				}
			}		
		}

		private bool HasEnumFlagsAttribute()
		{
			var fieldType = fieldInfo.FieldType;

			if (fieldType.IsArray)
			{
				var elementType = fieldType.GetElementType();

				return elementType.IsDefined(typeof(FlagsAttribute), false);
			}

			return fieldType.IsDefined(typeof(FlagsAttribute), false);
		}
	}


	[UnityEditor.CustomPropertyDrawer(typeof(ValueListAttribute), true)]
	public class StringPopup : UnityEditor.PropertyDrawer
	{
		PropertyInfo GettetSetterProperty = null;

		public override void OnGUI(Rect position, UnityEditor.SerializedProperty prop, GUIContent label)
		{
			List<string> values = (this.attribute as ValueListAttribute).GetValuesList(prop.serializedObject.targetObject);

			GUI.enabled = values != null && values.Count > 0;

			if(!string.IsNullOrEmpty((this.attribute as ValueListAttribute).GetterSetterName))
			{
				GettetSetterProperty = prop.serializedObject.targetObject.GetType().GetProperty((this.attribute as ValueListAttribute).GetterSetterName);
				if (GettetSetterProperty.PropertyType != typeof(string)) GettetSetterProperty = null;
			}

			if (values != null)
			{
				List<string> list = new List<string>() { "EmptyValue" };
				list.AddRange(values);
				string currentValue = GetValue(prop);
				int index = list.IndexOf(currentValue ?? string.Empty);
				if (index == -1) index = 0;
				index = UnityEditor.EditorGUI.Popup(position, label.text, index, list.ToArray());
				//prop.stringValue = index == 0 ? string.Empty : list[index];
				SetValue(prop, index == 0 ? string.Empty : list[index]);
			}
			GUI.enabled = true;
		}

		private string GetValue(UnityEditor.SerializedProperty prop)
		{
			if (GettetSetterProperty != null && GettetSetterProperty.CanRead)
			{
				try
				{
					return GettetSetterProperty.GetValue(prop.serializedObject.targetObject, null) as string;
				}
				catch
				{
					return prop.stringValue;
				}
			}
			else
			{
				return prop.stringValue;
			}
			
		}

		private void SetValue(UnityEditor.SerializedProperty prop, string value)
		{
			if (GettetSetterProperty != null && GettetSetterProperty.CanWrite)
			{
				try
				{
					GettetSetterProperty.SetValue(prop.serializedObject.targetObject, value, null);
				}
				catch
				{
					prop.stringValue = value;
				}
			}
			else
			{
				prop.stringValue = value;
			}
		}
	}
#endif

}