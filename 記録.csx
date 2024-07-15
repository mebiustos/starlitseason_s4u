#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCore.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4UCam.csx"
#load "Scripts\アイドルマスター スターリットシーズン\extensions\s4u\S4U.csx"

[Action]
void S4U記録()
{
    var s4u = new S4U(@this);
    s4u.Recording();
}