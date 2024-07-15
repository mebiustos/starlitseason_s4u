#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCore.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCam.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UEffect.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4U.csx"

[Action]
void S4U演出スロット設定コード生成()
{
    var s4u = new S4U(@this);
    var effect = s4u.GetEffect();
    effect.CreateCode();
}