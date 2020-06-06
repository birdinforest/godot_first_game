using Godot;
using System;

public class Player : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	[Export]
	public int Speed = 400;
	[Signal]
	public delegate void Hit();
	private Vector2 _sceenSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hide();
		_sceenSize = GetViewport().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var velocity = new Vector2();

		if(Input.IsActionPressed("ui_right"))
		{
			velocity.x += 1;
		}
		if(Input.IsActionPressed("ui_left"))
		{
			velocity.x -= 1;
		}
		if(Input.IsActionPressed("ui_up"))
		{
			velocity.y -= 1;
		}
		if(Input.IsActionPressed("ui_down"))
		{
			velocity.y += 1;
		}

		var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

		if(velocity.Length() > 0) {
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
		}
		else
		{
			animatedSprite.Stop();
		}
		
		Position += velocity * delta;
		Position = new Vector2 (
			x: Mathf.Clamp(Position.x, 0, _sceenSize.x),
			y: Mathf.Clamp(Position.y, 0, _sceenSize.y)
		);

		if(velocity.x != 0) 
		{
			animatedSprite.Animation = "walk";
			animatedSprite.FlipV = false;
			animatedSprite.FlipH = velocity.x < 0;
		}
		else if(velocity.y != 0) {
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}
	}

	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	public void OnPlayerBodyEntered(PhysicsBody2D body)
	{
		Hide();
		EmitSignal("Hit");

		// Disable collider to ensure it only trigger once.
		// Directly disable collider might cause issue if it is in collision process, 
		// using `Godot.Object.SetDeferred` tells Godot to wait to disable the shape until it is sfae to do so.
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	}
}
