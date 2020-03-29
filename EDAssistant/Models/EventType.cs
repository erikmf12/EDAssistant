namespace EDAssistant.Models
{
	/// <summary>
	/// To get a list of all EventTypes, go to http://edcodex.info/?m=doc#f.3
	/// Needs to be added to MainWindow.xaml.cs and LogEventFactory
	/// </summary>
	public enum EventType
	{
		Docked,
		Undocked,
		Fileheader,
		Location,
		FSDJump,
		FSDTarget,
		LoadGame,
	}
}