using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public float speed;
	[Export] public float acceleration;
	[Export] public float deceleration;
	[Export] public Label hitPointsText;
	[Export] public Timer hitTimer;
	[Export] AnimatedSprite2D animatedSprite;

	public bool alive = true;
	private int hitPoints = 3;

	public override void _Process(double delta)
	{
		string animation = hitTimer.IsStopped() ? "default" : "hurt";

		animatedSprite.Play(animation);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Vector2.Zero;
		
		if (alive)
			direction = Input.GetVector("moveLeft", "moveRight", "moveUp", "moveDown").Normalized() * speed;

		Velocity = Velocity.Lerp(direction, (direction != Vector2.Zero) ? acceleration : deceleration);
		
		MoveAndSlide();
	}

	public void HitPlayer(Vector2 knockback, in Timer timer)
	{
		Velocity = knockback;

		hitTimer.Start();
		hitPoints--;
		hitPointsText.Text = "Health: " + hitPoints;

		if (hitPoints <= 0)
		{
			alive = false;

			Engine.TimeScale = 0.4;

			GetNode<AnimatedSprite2D>("AnimatedSprite2D").QueueFree();
			GetNode<CollisionShape2D>("CollisionShape2D").QueueFree();
			GetNode<Label>("Label").QueueFree();

			timer.Start();
			
			GD.Print("You died!");
		}
	}
}
