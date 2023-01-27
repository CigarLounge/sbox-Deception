using Sandbox;

namespace Murder;

public enum Channel
{
	All,
	Spectator
}

public enum MuteFilter
{
	None,
	AlivePlayers,
	Spectators,
	All
}

public partial class Player
{
	/// <summary>
	/// The current chat channel to send messages to.
	/// </summary>
	[ConVar.ClientData( "channel_current" )]
	public Channel CurrentChannel { get; set; } = Channel.Spectator;

	/// <summary>
	/// Determines which players are currently muted.
	/// </summary>
	[ConVar.ClientData( "mute_filter" )]
	public MuteFilter MuteFilter { get; set; } = MuteFilter.None;

	public bool CanHearSpectators => (!this.IsAlive() || GameManager.Instance.State is not GameplayState) && MuteFilter != MuteFilter.Spectators && MuteFilter != MuteFilter.All;
	public bool CanHearAlivePlayers => MuteFilter != MuteFilter.AlivePlayers && MuteFilter != MuteFilter.All;

	public static void ToggleMute()
	{
		var player = Game.LocalPawn as Player;

		if ( ++player.MuteFilter > MuteFilter.All )
			player.MuteFilter = MuteFilter.None;
	}
}
