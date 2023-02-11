using Sandbox;
using Sandbox.UI;

namespace Murder;

public partial class Player
{
	public const float MaxHintDistance = 5000f;

	private static Panel _currentHintPanel;
	private static IEntityHint _currentHint;

	private void DisplayEntityHints()
	{
		var player = UI.Hud.DisplayedPlayer;

		if ( !player.IsFirstPersonMode || !player.TimeUntilClean )
		{
			DeleteHint( true );
			return;
		}

		var hint = FindHintableEntity();

		if ( hint is null || !hint.CanHint( player ) )
		{
			DeleteHint();
			return;
		}

		if ( hint == _currentHint )
			return;

		DeleteHint();

		_currentHintPanel = hint.DisplayHint( player );
		_currentHintPanel.Parent = UI.HintDisplay.Instance;
		_currentHintPanel.Enabled( true );

		_currentHint = hint;
	}

	private static void DeleteHint( bool immediate = false )
	{
		_currentHintPanel?.Delete( immediate );
		_currentHintPanel = null;

		_currentHint = null;
	}

	private IEntityHint FindHintableEntity()
	{
		var tr = Trace.Ray( new Ray( Camera.Position, Camera.Rotation.Forward ), MaxHintDistance )
			.Ignore( UI.Hud.DisplayedPlayer )
			.WithAnyTags( "solid", "interactable" )
			.UseHitboxes()
			.Run();

		_traceDistance = tr.Distance;
		HoveredEntity = tr.Entity;

		if ( HoveredEntity is IEntityHint hint && tr.Distance <= hint.HintDistance )
			return hint;

		return null;
	}
}
