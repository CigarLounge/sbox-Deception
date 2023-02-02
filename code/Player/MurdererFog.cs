using Sandbox;

namespace Murder;

internal sealed class MurdererFog : EntityComponent<Player>
{
	private Particles _fog;

	public MurdererFog()
	{
		ShouldTransmit = false;
	}

	public override bool CanAddToEntity( Entity entity )
	{
		if ( entity is not Player player )
			return false;

		return player.Role == Role.Murderer;
	}

	protected override void OnActivate()
	{
		_fog ??= Particles.Create( "particles/black_smoke.vpcf", Entity, "arm_lower_L" );

		if ( Game.IsClient )
			UI.TextChat.AddInfo( "Your evil presence is showing! Kill someone to hide." );
	}

	protected override void OnDeactivate()
	{
		_fog?.Destroy();
		_fog = null;
	}

	[GameEvent.Player.Spawned]
	private static void OnPlayerSpawned( Player player )
	{
		if ( Game.IsServer || player.IsLocalPawn )
			player.Components.RemoveAny<MurdererFog>();
	}

	[GameEvent.Player.Killed]
	private static void OnPlayerKilled( Player player )
	{
		if ( player.Role == Role.Murderer )
		{
			player.Components.RemoveAny<MurdererFog>();
			return;
		}

		foreach ( var murderer in Role.Murderer.GetPlayers() )
		{
			if ( murderer != player.Killer )
				return;

			murderer.Components.RemoveAny<MurdererFog>();
		}
	}
}
