using Sandbox;

namespace Murder;

public partial class Player
{
	[Net]
	public Entity Using { get; protected set; }

	/// <summary>
	/// The entity we're currently looking at.
	/// </summary>
	public Entity HoveredEntity { get; private set; }

	public const float UseDistance = 80f;
	private float _traceDistance;

	protected void PlayerUse()
	{
		HoveredEntity = FindHovered();

		using ( Prediction.Off() )
		{
			if ( Input.Pressed( InputButton.Use ) )
			{
				if ( CanUse( HoveredEntity ) )
					StartUsing( HoveredEntity );
			}

			if ( !Input.Down( InputButton.Use ) )
			{
				StopUsing();
				return;
			}

			// There is no entity to use.
			if ( !Using.IsValid() )
				return;

			if ( !CanContinueUsing( Using ) )
				StopUsing();
		}
	}

	protected Entity FindHovered()
	{
		var trace = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * MaxHintDistance )
			.Ignore( this )
			.WithAnyTags( "solid", "interactable" )
			.Run();

		if ( !trace.Entity.IsValid() )
			return null;

		if ( trace.Entity.IsWorld )
			return null;

		_traceDistance = trace.Distance;
		return trace.Entity;
	}

	public bool CanUse( Entity entity )
	{
		if ( entity is not IUse use )
			return false;

		if ( _traceDistance > UseDistance )
			return false;

		if ( !use.IsUsable( this ) )
			return false;

		return true;
	}

	public bool CanContinueUsing( Entity entity )
	{
		if ( HoveredEntity != entity )
			return false;

		if ( _traceDistance > UseDistance )
			return false;

		if ( entity is IUse use && use.OnUse( this ) )
			return true;

		return false;
	}

	public void StartUsing( Entity entity )
	{
		Using = entity;
	}

	protected void StopUsing()
	{
		Using = null;
	}
}
