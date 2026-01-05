using UnityEngine;

public enum SkillUpgradeType
{
    None,

     //돌진 트리
     Dash,
     Dash_CloneOnStart, //시작 지점에 복제체 생성
     Dash_CloneOnStartAndArrival, //시작 지점과 도착 지점에 복제체 생성
    Dash_ShardOnShart, //시작 지점에 시간 파편 생성
    Dash_ShardOnStartAndArrival, //시작 지점과 도착 지점에 시간 파편 생성

    //시간 파편 트리
    Shard, //기본 업그레이드
    Shard_MoveToEnemy, //가장 가까운 적에게 이동
    Shard_MultiCast, //다중 시전
    Shard_Teleport, //시간 파편으로 순간 이동
    Shard_TeleportHpRewind, //시간 파편으로 순간 이동 + 체력 롤백

    //검 던지기 트리
    SwordThrow, //기본 업그레이드
    SwordThrow_Spin, //회전
    SwordThrow_Pierce, //관통
    SwordThrow_Bounce //튕김
}
