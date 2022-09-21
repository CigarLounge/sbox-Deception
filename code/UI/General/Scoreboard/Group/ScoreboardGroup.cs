using Sandbox;
using Sandbox.UI;

namespace Murder.UI;

[UseTemplate]
public sealed class ScoreboardGroup : Panel
{

	public int GroupMembers = 0;

	public bool Spectators { get; init; }
	private Label Title { get; init; }
	private Panel Content { get; init; }

	public ScoreboardGroup( Panel parent, bool spectators ) : base( parent )
	{
		Spectators = spectators;

		Title.Text = spectators ? "Spectators" : string.Empty;
		AddClass( spectators ? "spectators" : "alive" );
	}

	public ScoreboardEntry AddEntry( Client client )
	{
		var scoreboardEntry = new ScoreboardEntry( Content, client );

		scoreboardEntry.Update();

		return scoreboardEntry;
	}
}
