using Godot;
using System;

public partial class EnemyCounter : Node2D
{
    [Signal]
    public delegate void EnemiesDefeatedEventHandler();

    public override void _Ready()
    {
        ChildExitingTree += OnEnemyDestroyed;
    }

    public void OnEnemyDestroyed(Node e)
    {
        if (e is Enemy)
        {
            int count = EnemyCount();
            if (count <= 1) // If the last enemy is exiting tree
            {
                EmitSignal(SignalName.EnemiesDefeated);
            }
        }
    }

    public int EnemyCount()
    {
        int count = 0;
        foreach (var c in GetChildren())
        {
            if (c is Enemy)
            {
                count++;
            }
        }

        return count;
    }
}
