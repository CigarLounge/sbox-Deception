using Sandbox;
using Sandbox.UI;

namespace Murder.UI;

[UseTemplate]
public class Nameplate : EntityHintPanel
{
	private static Nameplate _previous;
	private readonly Player _player;

	private Label Name { get; init; }
	private Panel DisguiseHint { get; init; }

	public Nameplate( Player player )
	{
		_previous?.Delete( true );

		_player = player;
		_previous = this;
	}

	public override void Tick()
	{
		if ( !_player.IsValid() )
			return;

		Name.Text = _player.BystanderName;
		Name.Style.FontColor = _player.Color;

		var hinter = ((Player)Local.Pawn).CurrentPlayer;
		DisguiseHint.Enabled( hinter.IsAlive() && _player.Corpse.IsValid() && hinter.CanUse( _player.Corpse ) );
	}

	public override void Delete( bool immediate = false )
	{
		DisguiseHint.Delete( true );

		base.Delete( immediate );
	}
}
