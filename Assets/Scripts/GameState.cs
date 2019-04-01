using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState {
    public enum Game_State
    {
        PRE_GAME,
        IN_PROGRESS,
        DEAD
    }

    static GameState()
    {
        currentState = Game_State.PRE_GAME;
    }

    private static Game_State currentState;

    public static Game_State GetGameState()
    {

        return currentState; 
    }

    public static void SetGameState(Game_State state)
    {
        currentState = state;
    }
}
