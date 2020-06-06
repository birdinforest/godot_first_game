using Godot;
using System;
using System.Threading.Tasks;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    
    async public Task ShowGameOver()
    {
        await ShowMessageAsync("Game Over");
        ShowMessage("Dodge the\nCreeps!");
        GetNode<Button>("StartButton").Show();
    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    public void ShowMessage(string text)
    {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();
    }

    public SignalAwaiter ShowMessageAsync(string text)
    {
        var message = GetNode<Label>("Message");
        message.Text = text;
        message.Show();

        var timer = GetNode<Timer>("MessageTimer");
        timer.Start();
        return ToSignal(timer, "timeout");
    }

    public void onMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
    }

    public void onStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal("StartGame");
    }
}
