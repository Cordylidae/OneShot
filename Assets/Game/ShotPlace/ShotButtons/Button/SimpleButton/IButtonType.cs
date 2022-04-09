using Inspector;
using UnityEngine;
using System.Collections.Generic;

public struct ButtonType
{
	public const string Simple = "Simple";
	public const string Frost = "Frost";
};

public class IButtonType : MonoBehaviour
{
	[SerializeField, Inspector.ValueList("AllowedTypes")]
	public string buttonType;
	public virtual List<string> AllowedTypes()
	{
		return typeof(ButtonType).GetAllPublicConstantValues<string>();
	}
}
