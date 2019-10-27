using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    // 玩家逃脱事件
    public delegate void EscapeEvent(GameObject patrol);
    public static event EscapeEvent OnGoalLost;
    // 巡逻兵追击事件
    public delegate void FollowEvent(GameObject patrol);
    public static event FollowEvent OnFollowing;
    // 游戏失败事件
    public delegate void GameOverEvent();
    public static event GameOverEvent GameOver;
    // 游戏胜利事件
    public delegate void WinEvent();
    public static event WinEvent Win;

    // 玩家逃脱
    public void PlayerEscape(GameObject patrol) {
        if (OnGoalLost != null) {
            OnGoalLost(patrol);
        }
    }

    // 巡逻兵追击
    public void FollowPlayer(GameObject patrol) {
        if (OnFollowing != null) {
            OnFollowing(patrol);
        }
    }

    // 玩家被捕
    public void OnPlayerCatched() {
        if (GameOver != null) {
            GameOver();
        }
    }

    // 时间结束
    public void Finished() {
        if (Win != null) {
            Win();
        } 
    }
}