using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

public class BattleSkill
{
    protected BaseSkill ReferenceBaseSkill;
    protected EMasterSkill MasterSkillType;
    protected BattlePokemon ReferencePokemon;

    protected int MinCount;
    protected int MaxCount;

    public BattleSkill(BaseSkill InReferenceBaseSkill, EMasterSkill InMasterSkillType, BattlePokemon InReferencePokemon)
    {
        ReferenceBaseSkill = InReferenceBaseSkill;
        ReferencePokemon = InReferencePokemon;
        MasterSkillType = InMasterSkillType;

        MinCount = ReferenceBaseSkill.GetMinCount();
        MaxCount = ReferenceBaseSkill.GetMaxCount();
    }

    public BattlePokemon GetReferencePokemon()
    {
        return ReferencePokemon;
    }

    public int GetSkillAccuracy(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        double AbilityFactor = 1.0;
        double ItemFactor = 1.0;
        if(GetSkillClass() == ESkillClass.PhysicalMove && SourcePokemon.HasAbility("活力", InManager, SourcePokemon, TargetPokemon))
        {
            AbilityFactor *= 0.8;
        }
        if(SourcePokemon.HasAbility("复眼", InManager, SourcePokemon, TargetPokemon))
        {
            AbilityFactor *= 1.3;
        }
        if(SourcePokemon.HasItem("广角镜"))
        {
            ItemFactor *= 1.1;
        }
        return (int)(ReferenceBaseSkill.GetAccuracy(InManager, SourcePokemon, TargetPokemon) * AbilityFactor * ItemFactor);
    }

    public bool IsDamageSkill()
    {
        return ReferenceBaseSkill.GetSkillClass() == ESkillClass.SpecialMove || ReferenceBaseSkill.GetSkillClass() == ESkillClass.PhysicalMove;
    }

    public int GetSkillMinCount()
    {
        return MinCount;
    }
    public int GetSkillMaxCount()
    {
        return MaxCount;
    }

    public void SetSkillMaxCount(int InMaxCount)
    {
        MaxCount = InMaxCount;
    }

    public void SetSkillMinCount(int InMinCount)
    {
        MinCount = InMinCount;
    }

    public virtual int GetCTRatio()
    {
        return ReferenceBaseSkill.GetCTRatio();
    }

    public bool JudgeCT(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        if(CastSkill.IsConstantDamage(InManager, SourcePokemon, TargetPokemon))
        {
            return false;
        }
        if(TargetPokemon.HasAbility("战斗盔甲", InManager, SourcePokemon, TargetPokemon))
        {
            return false;
        }
        if(TargetPokemon.HasAbility("硬壳盔甲", InManager, SourcePokemon, TargetPokemon))
        {
            return false;
        }
        int CTRatio = GetCTRatio() + SourcePokemon.GetCTLevel();
        CTRatio = Math.Min(3, CTRatio);
        CTRatio = Math.Max(0, CTRatio);
        int[] CTNumber = new int[4]{24, 8, 2, 1};
        System.Random rnd = new System.Random();
        int randNumber = rnd.Next(0, CTNumber[CTRatio]);
        return randNumber == 0;
    }

    public double ApplyTerrainDamageFactor(BattleManager InManager, DamageSkill CastSkill, double SourceDamage)
    {
        if(InManager.GetTerrainType() == EBattleFieldTerrain.Grass)
        {
            if(CastSkill.GetSkillName() == "地震" || CastSkill.GetSkillName() == "震级" || CastSkill.GetSkillName() == "重踏")
            {
                EditorLog.DebugLog(CastSkill.GetSkillName() + "因青草场地伤害减半了!");
                return (int)Math.Floor(SourceDamage * 0.5);
            }
        }
        return SourceDamage;
    }

    public int ConstantDamagePhase(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT, out double EffectiveFactor)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        double TypeEffectiveFactor = CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon);
        EffectiveFactor = TypeEffectiveFactor;
        if(EffectiveFactor != 0)
        {
            EffectiveFactor = 1.0;
        }
        return CastSkill.GetConstantDamage(InManager, SourcePokemon, TargetPokemon);
    }

    public int DamagePhase(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, bool CT, out double EffectiveFactor)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        if(CastSkill.IsConstantDamage(InManager, SourcePokemon, TargetPokemon))
        {
            return ConstantDamagePhase(InManager, SourcePokemon, TargetPokemon, CT, out EffectiveFactor);
        }
        double Power = CastSkill.GetPower(InManager, SourcePokemon, TargetPokemon);
        double Atk = CastSkill.GetSourceAtk(InManager, SourcePokemon, TargetPokemon, CT);
        double Def = CastSkill.GetTargetDef(InManager, SourcePokemon, TargetPokemon, CT);
        double Level = SourcePokemon.GetLevel();
        double DamageWithOutFactor = ((2.0 * Level + 10.0) / 250.0) * (Atk / Def) * Power + 2;
        double Damage = DamageWithOutFactor;

        if(CT)
        {
            double CTFactor = CastSkill.GetCTFactor(InManager, SourcePokemon, TargetPokemon);
            if(SourcePokemon.HasAbility("狙击手", InManager, SourcePokemon, TargetPokemon))
            {
                CTFactor = 2.25;
            }
            Damage = (int)Math.Floor(Damage * CTFactor);
        }

        System.Random rnd = new System.Random();
        double RandomFactor = (double)rnd.Next(85, 101) / 100.0;
        Damage = (int)Math.Floor(Damage * RandomFactor);

        if(CastSkill.IsSameType(InManager, SourcePokemon, TargetPokemon))
        {
            double SameTypeFactor = CastSkill.GetSameTypePowerFactor(InManager, SourcePokemon, TargetPokemon);
            Damage = (int)Math.Floor(Damage * SameTypeFactor);
        }
        double TypeEffectiveFactor = CastSkill.GetTypeEffectiveFactor(InManager, SourcePokemon, TargetPokemon);
        EffectiveFactor = TypeEffectiveFactor;
        Damage = (int)Math.Floor(Damage * TypeEffectiveFactor);

        if(CastSkill.IsPhysicalMove(InManager, SourcePokemon, TargetPokemon))
        {
            if(SourcePokemon.HasStatusChange(EStatusChange.Burn))
            {
                double BurnFactor = 0.5;
                Damage = (int)Math.Floor(Damage * BurnFactor);
            }
        }
        else
        {
            if(SourcePokemon.HasStatusChange(EStatusChange.Frostbite))
            {
                double FrostbiteFactor = 0.5;
                Damage = (int)Math.Floor(Damage * FrostbiteFactor);
            }
        }

        if(TargetPokemon.HasStatusChange(EStatusChange.Drowsy))
        {
            double DrowsyFactor = 1.5;
            Damage = (int)Math.Floor(Damage * DrowsyFactor);
        }

        Damage = ApplyTerrainDamageFactor(InManager, CastSkill, Damage);

        if(SourcePokemon.GetAbility())
        {
            Damage = SourcePokemon.GetAbility().ChangeSkillDamage(InManager, CastSkill, SourcePokemon, TargetPokemon, Damage);
        }
        if(TargetPokemon.GetAbility())
        {
            Damage = TargetPokemon.GetAbility().ChangeSkillDamage(InManager, CastSkill, SourcePokemon, TargetPokemon, Damage);
        }

        bool IsPlayer = !TargetPokemon.GetIsEnemy();
        if(!CT)
        {
            double WallFactor = 1.0;
            if(InManager.HasBattleFieldStatus(IsPlayer, EBattleFieldStatus.ReflectStatus) && CastSkill.IsPhysicalMove(InManager, SourcePokemon, TargetPokemon))
            {
                WallFactor = 0.5;
            }
            if(InManager.HasBattleFieldStatus(IsPlayer, EBattleFieldStatus.LightScreenStatus) && !CastSkill.IsPhysicalMove(InManager, SourcePokemon, TargetPokemon))
            {
                WallFactor = 0.5;
            }
            if(CastSkill.GetSkillName() != "劈瓦" && CastSkill.GetSkillName() != "精神之牙" && 
            !SourcePokemon.HasAbility("穿透", InManager, SourcePokemon, TargetPokemon))
            {
                Damage = (int)Math.Floor(Damage * WallFactor);                
            }
        }

        double TrainerSkillFactor = 1.0;
        if(TargetPokemon.GetHP() <= TargetPokemon.GetMaxHP() / 2)
        {
            if(TargetPokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "生命力爆发")
            {
                TrainerSkillFactor *= 0.8;
            }
        }
        if(SourcePokemon.GetHP() <= SourcePokemon.GetMaxHP() / 2)
        {
            if(SourcePokemon.GetReferenceTrainer().TrainerSkill.GetSkillName() == "生命力爆发")
            {
                TrainerSkillFactor *= 1.2;
            }
        }

        Damage = (int)Math.Floor(Damage * TrainerSkillFactor); 

        double SepcialRuleFactor = 1.0;
        if(InManager.HasSpecialRule("特殊规则(帕琦拉)") && SourcePokemon.GetIsEnemy() == false && SourcePokemon.HasStatUp())
        {
            SepcialRuleFactor = 0.3333333;
        }
        else if(InManager.HasSpecialRule("特殊规则(帕琦拉)") && TargetPokemon.GetIsEnemy() == false && TargetPokemon.HasStatUp())
        {
            SepcialRuleFactor = 2.0;
        }
        Damage = (int)Math.Floor(Damage * SepcialRuleFactor); 

        int IntDamage = (int)Math.Floor(Damage);
        IntDamage = Math.Min(IntDamage, TargetPokemon.GetHP());
        return Mathf.Max(1, IntDamage);
    }

    public void ProcessStatusEffect(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        StatusSkill CastSkill = (StatusSkill)ReferenceBaseSkill;
        CastSkill.ProcessStatusSkillEffect(InManager, SourcePokemon, TargetPokemon);
    }

    public void AfterDamageEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, int Damage)
    {
        DamageSkill CastSkill = (DamageSkill)ReferenceBaseSkill;
        CastSkill.AfterDamageEvent(InManager, SourcePokemon, TargetPokemon, Damage);
    }

    public int GetAfterSkillEffectEventProbablity(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return ReferenceBaseSkill.AfterSkillEffectEventProbablity(InManager, SourcePokemon, TargetPokemon);
    }
    public void AfterSkillEffectEvent(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        ReferenceBaseSkill.AfterSkillEffectEvent(InManager, SourcePokemon, TargetPokemon);
    }
    public ESkillClass GetSkillClass() => ReferenceBaseSkill.GetSkillClass();
    public bool CanBeProtected()
    {
        return ReferenceBaseSkill.CanBeProtected();
    }
    public bool JudgeIsEffective(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon, ref string Reason)
    {
        return ReferenceBaseSkill.JudgeIsEffective(InManager, SourcePokemon, TargetPokemon, out Reason);
    }

    public bool HasHealEffect(BattleManager InManager)
    {
        return ReferenceBaseSkill.HasHealEffect(InManager);
    }

    public int GetAttackAccuracyChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
        return ReferenceBaseSkill.GetAttackAccuracyChangeLevel(InManager, SourcePokemon, TargetPokemon);
    }

    public int GetTargetEvasionChangeLevel(BattleManager InManager, BattlePokemon SourcePokemon, BattlePokemon TargetPokemon)
    {
         return ReferenceBaseSkill.GetTargetEvasionChangeLevel(InManager, SourcePokemon, TargetPokemon);
    }

    public int GetSkillPriority(BattleManager InManager, BattlePokemon TargetPokemon)
    {
        int Priority = ReferenceBaseSkill.GetSkillPriority(InManager, ReferencePokemon, TargetPokemon);
        int AbilityPriority = 0;
        if(ReferencePokemon.HasAbility("疾风之翼", InManager, ReferencePokemon, TargetPokemon) &&
           ReferencePokemon.GetHP() == ReferencePokemon.GetMaxHP() &&
           ReferenceBaseSkill.GetSkillType(ReferencePokemon) == EType.Flying)
        {
            AbilityPriority = 1;           
        }
        Priority = Priority + AbilityPriority;
        return Priority;
    }

    public string GetSkillName() { return ReferenceBaseSkill.GetSkillName(); }
    public ERange GetSkillRange() { return ReferenceBaseSkill.GetSkillRange();}
    public PlayableDirector GetSkillAnimation(){ return ReferenceBaseSkill.GetSkillAnimation();}

    public BaseSkill GetReferenceSkill() { return ReferenceBaseSkill;}
}


public class BattleSkillMetaInfo
{
    private static Dictionary<string, bool> TouchMoves = new Dictionary<string, bool>()
    {
        {"拍击", true},
        {"空手劈", true},
        {"连环巴掌", true},
        {"连续拳", true},
        {"百万吨重拳", true},
        {"火焰拳", true},
        {"冰冻拳", true},
        {"雷电拳", true},
        {"抓", true},
        {"夹住", true},
        {"极落钳", true},
        {"居合劈", true},
        {"翅膀攻击", true},
        {"飞翔", true},
        {"绑紧", true},
        {"摔打", true},
        {"藤鞭", true},
        {"踩踏", true},
        {"二连踢", true},
        {"百万吨重踢", true},
        {"飞踢", true},
        {"回旋踢", true},
        {"头锤", true},
        {"角撞", true},
        {"乱击", true},
        {"角钻", true},
        {"撞击", true},
        {"泰山压顶", true},
        {"紧束", true},
        {"猛撞", true},
        {"大闹一番", true},
        {"舍身冲撞", true},
        {"咬住", true},
        {"啄", true},
        {"啄钻", true},
        {"深渊翻滚", true},
        {"踢倒", true},
        {"双倍奉还", true},
        {"地球上投", true},
        {"怪力", true},
        {"花瓣舞", true},
        {"挖洞", true},
        {"电光一闪", true},
        {"愤怒", true},
        {"忍耐", true},
        {"舌舔", true},
        {"攀瀑", true},
        {"贝壳夹击", true},
        {"火箭头锤", true},
        {"缠绕", true},
        {"飞膝踢", true},
        {"吸血", true},
        {"迷昏拳", true},
        {"蟹钳锤", true},
        {"乱抓", true},
        {"终结门牙", true},
        {"愤怒门牙", true},
        {"劈开", true},
        {"挣扎", true},
        {"三连踢", true},
        {"小偷", true},
        {"火焰轮", true},
        {"抓狂", true},
        {"绝处逢生", true},
        {"音速拳", true},
        {"出奇一击", true},
        {"逆鳞", true},
        {"滚动", true},
        {"点到为止", true},
        {"电光", true},
        {"连斩", true},
        {"钢翼", true},
        {"报恩", true},
        {"迁怒", true},
        {"爆裂拳", true},
        {"超级角击", true},
        {"追打", true},
        {"高速旋转", true},
        {"铁尾", true},
        {"金属爪", true},
        {"借力摔", true},
        {"十字劈", true},
        {"咬碎", true},
        {"神速", true},
        {"碎岩", true},
        {"击掌奇袭", true},
        {"硬撑", true},
        {"真气拳", true},
        {"清醒", true},
        {"蛮力", true},
        {"报复", true},
        {"劈瓦", true},
        {"拍落", true},
        {"蛮干", true},
        {"潜水", true},
        {"猛推", true},
        {"火焰踢", true},
        {"冰球", true},
        {"尖刺臂", true},
        {"剧毒牙", true},
        {"撕裂爪", true},
        {"彗星拳", true},
        {"惊吓", true},
        {"暗影拳", true},
        {"冲天拳", true},
        {"燕返", true},
        {"龙爪", true},
        {"弹跳", true},
        {"毒尾", true},
        {"渴望", true},
        {"伏特攻击", true},
        {"叶刃", true},
        {"唤醒巴掌", true},
        {"臂锤", true},
        {"陀螺球", true},
        {"啄食", true},
        {"急速折返", true},
        {"近身战", true},
        {"以牙还牙", true},
        {"恶意追击", true},
        {"王牌", true},
        {"绞紧", true},
        {"惩罚", true},
        {"珍藏", true},
        {"突袭", true},
        {"闪焰冲锋", true},
        {"发劲", true},
        {"毒击", true},
        {"暗袭要害", true},
        {"水流尾", true},
        {"十字剪", true},
        {"龙之俯冲", true},
        {"吸取拳", true},
        {"勇鸟猛攻", true},
        {"终极冲击", true},
        {"子弹拳", true},
        {"雪崩", true},
        {"暗影爪", true},
        {"雷电牙", true},
        {"冰冻牙", true},
        {"火焰牙", true},
        {"影子偷袭", true},
        {"意念头锤", true},
        {"攀岩", true},
        {"强力鞭打", true},
        {"十字毒刃", true},
        {"铁头", true},
        {"打草结", true},
        {"虫咬", true},
        {"木槌", true},
        {"水流喷射", true},
        {"双刃头锤", true},
        {"二连击", true},
        {"捏碎", true},
        {"暗影潜袭", true},
        {"山岚摔", true},
        {"重磅冲撞", true},
        {"蓄能焰袭", true},
        {"下盘踢", true},
        {"欺诈", true},
        {"逐步击破", true},
        {"自由落体", true},
        {"巴投", true},
        {"杂技", true},
        {"报仇", true},
        {"龙尾", true},
        {"疯狂伏特", true},
        {"直冲钻", true},
        {"二连劈", true},
        {"爱心印章", true},
        {"木角", true},
        {"圣剑", true},
        {"贝壳刃", true},
        {"高温重压", true},
        {"疯狂滚压", true},
        {"扫尾拍打", true},
        {"爆炸头突击", true},
        {"齿轮飞盘", true},
        {"雷击", true},
        {"Ｖ热焰", true},
        {"飞身重压", true},
        {"致命针刺", true},
        {"潜灵奇袭", true},
        {"吸取之吻", true},
        {"嬉闹", true},
        {"蹭蹭脸颊", true},
        {"手下留情", true},
        {"纠缠不休", true},
        {"增强拳", true},
        {"画龙点睛", true},
        {"皮卡皮卡必杀击", true},
        {"迎头一击", true},
        {"ＤＤ金勾臂", true},
        {"冰锤", true},
        {"十万马力", true},
        {"日光刃", true},
        {"深渊突刺", true},
        {"掷锚", true},
        {"猛扑", true},
        {"火焰鞭", true},
        {"嚣张", true},
        {"修长之角", true},
        {"热带踢", true},
        {"龙锤", true},
        {"狂舞挥打", true},
        {"极恶飞跃粉碎击", true},
        {"七星夺魂腿", true},
        {"认真起来大爆击", true},
        {"精神之牙", true},
        {"跺脚", true},
        {"冲岩", true},
        {"水流裂破", true},
        {"暗影偷盗", true},
        {"流星闪冲", true},
        {"麻麻刺刺", true},
        {"多属性攻击", true},
        {"等离子闪电拳", true},
        {"日光回旋下苍穹", true},
        {"亲密无间大乱揍", true},
        {"电电加速", true},
        {"飘飘坠落", true},
        {"熊熊火爆", true},
        {"砰砰击破", true},
        {"钢拳双击", true},
        {"紧咬不放", true},
        {"电喙", true},
        {"鳃咬", true},
        {"扑击", true},
        {"捕兽夹", true},
        {"巨兽斩", true},
        {"巨兽弹", true},
        {"广域破坏", true},
        {"木枝突刺", true},
        {"灵魂冲击", true},
        {"假跪真撞", true},
        {"铁滚轮", true},
        {"臂贝武器*", true},
        {"青草滑梯", true},
        {"爬击", true},
        {"泄愤", true},
        {"快速折返", true},
        {"三旋击", true},
        {"双翼", true},
        {"暗冥强击", true},
        {"水流连打", true},
        {"雷鸣蹴击", true},
        {"克命爪", true},
        {"屏障猛攻", true},
        {"岩斧", true},
        {"波动冲", true},
        {"突飞猛扑", true},
        {"秘剑・千重涛", true},
        {"下压踢", true},
        {"喷射拳", true},
        {"疾速转轮", true},
        {"鼠数儿", true},
        {"冰旋", true},
        {"巨剑突击", true},
        {"三连钻", true},
        {"晶光转转", true},
        {"仆刀", true},
        {"流水旋舞", true},
        {"怒牛", true},
        {"精神剑", true},
        {"全开猛撞", true},
        {"闪电猛冲", true},
        {"虫扑", true},
        {"起草", true},
        {"强力钻", true},
        {"愤怒之拳", true},
        {"悔念剑", true},
        {"电光双击", true},
        {"复仇", true},
        {"强刃攻击", true},
        {"硬压", true},
        {"豁出去", true},
        {"闪电强袭", true},
        // 在这里添加更多技能名
    };
    public static bool IsTouchingSkill(string SkillName)
    {
        if (TouchMoves.TryGetValue(SkillName, out bool isTouchMove))
        {
            return isTouchMove;
        }
        else
        {
            return false;
        }
    }
    public static bool IsSoundSkill(string SkillName)
    {
        if(SkillName == "叫声") return true;
        if(SkillName == "吼叫") return true;
        if(SkillName == "唱歌") return true;
        if(SkillName == "超音波") return true;
        if(SkillName == "刺耳声") return true;
        if(SkillName == "打鼾") return true;
        if(SkillName == "终焉之歌") return true;
        if(SkillName == "治愈铃声") return true;
        if(SkillName == "黑暗恐慌") return true;
        if(SkillName == "吵闹") return true;
        if(SkillName == "巨声") return true;
        if(SkillName == "金属音") return true;
        if(SkillName == "草笛") return true;
        if(SkillName == "长嚎") return true;
        if(SkillName == "虫鸣") return true;
        if(SkillName == "喋喋不休") return true;
        if(SkillName == "轮唱") return true;
        if(SkillName == "回声") return true;
        if(SkillName == "古老之歌") return true;
        if(SkillName == "大声咆哮") return true;
        if(SkillName == "战吼") return true;
        if(SkillName == "魅惑之声") return true;
        if(SkillName == "抛下狠话") return true;
        if(SkillName == "爆音波") return true;
        if(SkillName == "密语") return true;
        if(SkillName == "泡影的咏叹调") return true;
        if(SkillName == "鳞片噪音") return true;
        if(SkillName == "炽魂热舞烈音爆") return true;
        if(SkillName == "魂舞烈音爆") return true;
        if(SkillName == "破音") return true;
        if(SkillName == "诡异咒语") return true;
        if(SkillName == "闪焰高歌") return true;
        if(SkillName == "魅诱之声") return true;
        if(SkillName == "精神噪音") return true;
        if(SkillName == "岩石音爆") return true;
        return false;
    }
}