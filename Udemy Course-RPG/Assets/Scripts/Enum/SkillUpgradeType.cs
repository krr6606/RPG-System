using UnityEngine;

public enum SkillUpgradeType
{
    None,

     //돌진 트리
     Dash,
     Dash_CloneOnStart,
     Dash_CloneOnStartAndArrival,
     Dash_ShardOnShart,
     Dash_ShardOnStartAndArrival,

     //시간 파편 트리
     Shard,
     Shard_MoveToEnemy,
     Shard_MultiCast,
     Shard_Teleport,
     Shard_TeleportHpRewind
}
