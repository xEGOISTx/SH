namespace SHBase.DevicesBaseComponents
{
	/// <summary>
	/// Режим GPIO
	/// </summary>
	public enum GPIOMode
	{
		NotDefined,
		Input,
		Output
	}

	/// <summary>
	/// Уровень на GPIO
	/// </summary>
	public enum GPIOLevel
	{
		NotDefined,
		High,
		Low
	}

	public interface IBaseGPIOAction
	{
		byte PinNumber { get; }

		GPIOMode Mode { get; }

		GPIOLevel Level { get; }
	}
}
