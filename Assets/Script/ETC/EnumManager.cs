#region Ƽ��
public enum GenealogyType
{
    none,
    single,
    pair,
    straightPair,
    triple,
    straightTriple,
    fullHouse,
    straight,
    fourOfKind,
    straightFlush
}
public enum Animal { dog = 0, dragon = 15, phoenix = -1, bird = 1 }
public enum Tichu { none, noTichu, largeTichu, smallTichu }
public enum RoundType { notYet, oneTwo, roundOver }
public enum CardType { Red, Yellow, Green, Blue, Animal, RedGreen }
public enum WanringType { pass, submit }
#endregion
#region ������
public enum Winner { Red, Blue, Draw, none }
#endregion
#region ������
public enum SuzumeGenealogy
{
    none,
    straight, 
    triple
}
#endregion
#region ����
public enum SoundType { game, UI, card }
public enum Direction { own = 0, left = 1, team = 2, right = 3, other = 1}
public enum GameType { Tichu_4p, NineDragon, Suzume }
public enum Team { random, Red, Blue }
#endregion