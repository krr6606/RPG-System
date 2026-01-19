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
    SwordThrow_Bounce, //튕김

    //시간 잔상 트리
    TimeEcho, //기본 업그레이드. 플레이어 분신 생성 후 공격 
    TimeEcho_SingleAttack, //분신이 단일 타격
    TimeEcho_MultiAttack,  
    TimeEcho_ChanceToDuplicate,

    TimeEcho_HealWisp,
    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,

    //영역 확장 트리
    Domain_SlowingDown,//영역 내 적 느려짐
    Domain_EchoSpam,//영역 내 잔상 지속 생성
    Domain_ShardSpam,//영역 내 시간 파편 지속 생성

}
