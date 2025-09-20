using Godot;
using System;

public partial class World : Area2D
{
	[Export] public int hitPoints;
	[Export] public Label hitPointsText;
	[Export] public Timer timer;
	[Export] public AudioStreamPlayer2D hurtSound;
	[Export] public float knockback;

	public override void _Ready()
	{
		BodyEntered += HitEffect;

		timer.Timeout += () =>
		{
			Engine.TimeScale = 1;
			GetTree().ReloadCurrentScene();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotate(MathF.PI * (float)delta * 0.125f);
	}

	private void HitEffect(Node2D body)
	{
		if (body is Player player && player.hitTimer.IsStopped())
		{
			hitPoints--;
			hurtSound.Play();
			hitPointsText.Text = "Health: " + hitPoints;

			player.Velocity += (player.GlobalPosition - GlobalPosition).Normalized() * knockback;

			if (hitPoints <= 0)
			{
				Engine.TimeScale = 0.4;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").QueueFree();
				GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();

				timer.Start();
			}
		}
	}
}
