using Godot;
using System;

public partial class Killzone : Area2D
{
	[Export] public Timer timer;
	[Export] public AudioStreamPlayer2D hurtSound;
	[Export] public float knockback;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += Collide;

		timer.Timeout += () =>
		{
			Engine.TimeScale = 1;
			GetTree().ReloadCurrentScene();
		};
	}

	private void Collide(Node2D body) 
	{
		if (body is Player player)
			player.HitPlayer((player.GlobalPosition - GlobalPosition).Normalized() * knockback, in timer);
		
		hurtSound.Play();
	}
}
