using Godot;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

	[Signal]
	public delegate void PowerHitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;
	
	public Vector2 ScreenSize; // size of game window

	// called when node enters scene tree
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// called every frame
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // movement speed

		// move player with WASD
		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		// animate sprite based on movement
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0) 
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}
		
		if (Input.IsActionJustPressed("move_up"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Play();
		}
		if (Input.IsActionJustReleased("move_up"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Stop();
		}
		if (Input.IsActionJustPressed("move_down"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Play();
		}
		if (Input.IsActionJustReleased("move_down"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Stop();
		}
		if (Input.IsActionJustPressed("move_left"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Play();
		}
		if (Input.IsActionJustReleased("move_left"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Stop();
		}
		if (Input.IsActionJustPressed("move_right"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Play();
		}
		if (Input.IsActionJustReleased("move_right"))
		{
			GetNode<AudioStreamPlayer>("WalkSound").Stop();
		}

		// contain player within screen
		Position += velocity * (float) delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}
	
	private void OnBodyEntered(PhysicsBody2D body)
	{

		if ("PowerDot" == body.Name)
		{
			EmitSignal(SignalName.PowerHit);
		}
		else 
		{
			Hide();
			EmitSignal(SignalName.Hit);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}






