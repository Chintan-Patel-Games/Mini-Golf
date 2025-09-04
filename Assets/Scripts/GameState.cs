public enum GameState
{
    LevelSetup,    // Play platform rise + ball spawn animation
    PlayerInput,   // Player can aim and shoot
    BallMoving,    // Wait until ball stops rolling
    LevelComplete  // Show complete UI and load next level
}