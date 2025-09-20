using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export] public Player player;
	[Export] public Node2D world;
	[Export] public RayCast2D rayCast;
	[Export] public Timer timer;
	[Export] public Label angerText;
	[Export] public float minimumDetectionRange;
	[Export] public float speed;
	[Export] public float acceleration;

	private float anger = 0;

	public override void _Process(double delta)
	{
		angerText.Text = "Anger: " + MathF.Round(anger, 2);
	}

	public override void _PhysicsProcess(double delta)
	{
		rayCast.TargetPosition = player.GlobalPosition - rayCast.GlobalPosition;

		float detectionRange = Mathf.Clamp((GlobalPosition - world.GlobalPosition).Length(), minimumDetectionRange, float.MaxValue) + anger * 5;

		Vector2 direction = Vector2.Zero;

		if (player.alive && player.hitTimer.IsStopped() && rayCast.TargetPosition.LengthSquared() <= detectionRange * detectionRange && !rayCast.IsColliding())
		{
			anger += (float)delta;
			direction = rayCast.TargetPosition.Normalized() * (speed + anger * 50);
			timer.Start();
		}
		else if (timer.IsStopped())
		{
			anger = (float)Mathf.Clamp(anger - delta * 0.5, 0, float.MaxValue);

			Vector2 blip = new Vector2(MathF.Cos(world.Rotation * 8f), MathF.Sin(world.Rotation * 8f));

			blip = blip.Normalized() * 150;

			direction = (world.GlobalPosition - GlobalPosition + blip).Normalized() * speed;
		}

		Velocity = Velocity.Lerp(direction, acceleration + (anger / 150));

		MoveAndSlide();
	}
}
