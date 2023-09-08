using Godot;

public partial class Main : Node
{
	[Export]
	public PackedScene EnemyScene { get; set; }
	
	[Export]
	public PackedScene PowerDotScene { get; set; }

	private int _score;

	private Vector2[] positions = new Vector2[] {new Vector2(100, 500), new Vector2(300, 500), new Vector2(250, 200)};

	public override void _Ready()
	{
	}
	
	private void GameOver()
	{
		GetNode<Timer>("EnemyTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<Timer>("PowerDotTimer").Stop();
		
		GetNode<HUD>("HUD").ShowGameOver();
		
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	private void NewGame()
	{
		
		// removes old enemies and dots from previous attempt
		GetTree().CallGroup("enemies", Node.MethodName.QueueFree);
		GetTree().CallGroup("powerdots", Node.MethodName.QueueFree);
		
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
	}
	
	private void PowerUp()
	{
		
		_score += 10;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("EnemyTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("PowerDotTimer").Start();
	}

	private void OnEnemyTimerTimeout()
	{
		// create new instance of Enemy scene
		Enemy enemy = EnemyScene.Instantiate<Enemy>();

		// choose random location on Path2D
		var enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");
		enemySpawnLocation.ProgressRatio = GD.Randf();

		// set enemy's direction perpendicular to path direction
		float direction = enemySpawnLocation.Rotation + Mathf.Pi / 2;

		// set enemy's position to random location
		enemy.Position = enemySpawnLocation.Position;

		// add randomness to direction
		direction += (float) GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		enemy.Rotation = direction;

		// choose velocity 
		var velocity = new Vector2((float) GD.RandRange(150.0, 250.0), 0);
		enemy.LinearVelocity = velocity.Rotated(direction);

		// spawn enemy by adding it to main scene
		AddChild(enemy);
	}
	
	private void OnPowerDotTimerTimeout()
	{
		// create new instance of Dot scene
		PowerDot dot = PowerDotScene.Instantiate<PowerDot>();

		// set dot's position to random location
		dot.Position = new Vector2(100, 100);

		// spawn dot by adding it to main scene
		AddChild(dot);
	}

}
